using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Z80IDE;

namespace DockSample
{
    public partial class DummySolutionExplorer : ToolWindow
    {

        public delegate void SelectedFileEventHandler(object sender, SelectedFileEventArgs e);
        public event  SelectedFileEventHandler SelectedFile;


        Solution solution;
        public DummySolutionExplorer(Solution solution )
        {
            this.solution = solution;
            this.solution.Changed += new Solution.ChangedEventHandler(solution_Changed);
            InitializeComponent();
            updatetreeview();

            treeView1.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);
        }

        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectedFile != null && e.Node!=null && e.Node.Name!="")
            {
                SelectedFileEventArgs e2 = new SelectedFileEventArgs(e.Node.Name);
                SelectedFile(this, e2);
            }

        }

        void solution_Changed(object sender, EventArgs e)
        {
            updatetreeview();
        }

        protected override void OnRightToLeftLayoutChanged(EventArgs e)
        {
            treeView1.RightToLeftLayout = RightToLeftLayout;
        }

        public void updatetreeview()
        {
            treeView1.Nodes.Clear();

            TreeNode parent = new TreeNode(solution.name, 0,0);
            treeView1.Nodes.Add(parent);

            foreach (file f in solution.files)
            {
                TreeNode child = new TreeNode(f.name, 7, 7);
                child.Name = f.name;
                parent.Nodes.Add(child);

            }

         
            


        }
    }

    public class SelectedFileEventArgs : EventArgs
    {
        public SelectedFileEventArgs(string name)
        {
            this.name = name;

        }

        public string name;
    }
}