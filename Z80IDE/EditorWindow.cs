using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using DockSample;
using FastColoredTextBoxNS;
using System.Drawing.Drawing2D;

namespace Z80IDE
{ 
    public partial class EditorWindow :  DockContent
    {

        public delegate void EditorClosingHandler(object sender, EventArgs e);
        public event EditorClosingHandler EditorClosing;
        bool dirty = false;

        public String filename;
        Solution sol;

        MarkerStyle RedStyle = new MarkerStyle(new SolidBrush(Color.FromArgb(180, Color.Red)));

        List<int> errorlines = new List<int>();

        public EditorWindow(String name,Solution sol)
        {
            this.sol = sol;
            InitializeComponent();
            this.Text = name;
            filename = name;

            this.FormClosing += new FormClosingEventHandler(EditorWindow_FormClosing);

            fastColoredTextBox1.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(fastColoredTextBox1_TextChanged);

            fastColoredTextBox1.Language = FastColoredTextBoxNS.Language.Custom;

            fastColoredTextBox1.DescriptionFile = "syntax.xml";

            fastColoredTextBox1.AddStyle(RedStyle);//red will be rendering over yellow
           

        }

        void fastColoredTextBox1_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            setdirty();
        }

        void EditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (EditorClosing != null)
            {
                EditorClosing(this, new EventArgs());

            }

        }

        public void settext(string data)
        {
            this.fastColoredTextBox1.Text=data;
            setclean();
        }

        public string gettext()
        {
            return fastColoredTextBox1.Text;
        }

        public void setdirty()
        {
            this.TabText = "*" + filename;
            dirty = true;
        }

        public void setclean()
        {
            this.TabText = filename;
            dirty = false;
        }

        public bool isdirty()
        {
            return dirty;
        }

        private void toolStripMenuItem_redo_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Redo();
        }

        private void toolStripMenuItem_undo_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fastColoredTextBox1.Paste();
        }

        private void toolStripButton_save_Click(object sender, EventArgs e)
        {
            sol.savefile(filename);
        }

        public void highlighterror(int lineno)
        {
            //FastColoredTextBoxNS.VisualMarker vs = new FastColoredTextBoxNS.VisualMarker(

            errorlines.Add(lineno);
            //FIX ME not painting if in view refresh required   
        }

        private void fastColoredTextBox1_PaintLine(object sender, PaintLineEventArgs e)
        {
            if(errorlines.Contains(e.LineIndex))
            {
                using (var brush = new LinearGradientBrush(new Rectangle(0, e.LineRect.Top, 15, 15), Color.LightPink, Color.Red, 45))
                    e.Graphics.FillEllipse(brush, 0, e.LineRect.Top, 15, 15);
            }
        }

    }
}
