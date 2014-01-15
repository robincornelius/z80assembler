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
        public event EditorClosingHandler Closing;

        public String filename;
        public EditorWindow(String name)
        {
            InitializeComponent();
            this.Text = name;
            filename = name;

            this.FormClosing += new FormClosingEventHandler(EditorWindow_FormClosing);
        }

        void EditorWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Closing != null)
            {
                Closing(this,new EventArgs());

            }

        }

        public void settext(string data)
        {
            this.fastColoredTextBox1.Text=data;

        }
    }
}
