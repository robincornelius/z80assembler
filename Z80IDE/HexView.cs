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
    public partial class HexView :  DockContent
    {
        MemoryByteProvider mb = new MemoryByteProvider();
        public HexView(byte[] data)
        {
            InitializeComponent();
           
            hexBox1.ByteProvider = mb;
           
            long pos=0;
            foreach(byte b in data)
            {
                mb.WriteByte(pos++, b);
                
            }

           
            hexBox1.Update();

           
        }
    }
}
