using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockSample
{
    public partial class DummyOutputWindow : ToolWindow, IOutput
    {
        public DummyOutputWindow()
        {
            InitializeComponent();
        }

        public void clear()
        {
            textBox1.Clear();
        }

        public void appendmsg(string msg)
        {
            textBox1.AppendText(msg+"\r\n");

        }
    }

    public interface IOutput
    {
         void clear();
         void appendmsg(string msg);
       
    }
}