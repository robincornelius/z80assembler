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

namespace Z80IDE
{ 
    public partial class EditorWindow :  DockContent
    {

        public delegate void EditorClosingHandler(object sender, EventArgs e);
        public event EditorClosingHandler EditorClosing;
        bool dirty = false;

        public String filename;
        public EditorWindow(String name)
        {
            InitializeComponent();
            this.Text = name;
            filename = name;

            this.FormClosing += new FormClosingEventHandler(EditorWindow_FormClosing);

            fastColoredTextBox1.TextChanged += new EventHandler<FastColoredTextBoxNS.TextChangedEventArgs>(fastColoredTextBox1_TextChanged);
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

    }
}
