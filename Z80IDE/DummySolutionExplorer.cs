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
        TreeNode selected;

        public DummySolutionExplorer(Solution solution )
        {
            this.solution = solution;
            this.solution.Changed += new Solution.ChangedEventHandler(solution_Changed);
            InitializeComponent();
            updatetreeview();

            treeView1.AfterSelect += new TreeViewEventHandler(treeView1_AfterSelect);
            treeView1.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeView1_NodeMouseClick);
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && e.Node!=null && e.Node.Parent!=null)
            {
                selected = e.Node;

              
                ((ToolStripMenuItem)(contextMenuStrip1.Items[0])).Checked = solution.isassemblyrequired(selected.Text);
                
          

                contextMenuStrip1.Show(treeView1,e.Location);
            }
        }

        void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (SelectedFile != null && e.Node!=null )
            {
                SelectedFileEventArgs e2 = new SelectedFileEventArgs(e.Node.Name,e.Node.Parent==null);
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

            TreeNode parent = new TreeNode(solution.details.name, 0,0);
            treeView1.Nodes.Add(parent);

            foreach (file f in solution.details.files)
            {
                int type = 7;

                if (f.assemblefile == false)
                    type = 6;

                TreeNode child = new TreeNode(f.name, type, type);
                child.Name = f.name;
                parent.Nodes.Add(child);

            }

            treeView1.ExpandAll();
        }

        private void removeFromSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selected == null)
                return;

            if (selected.Parent != null)
            {
                solution.removefile(selected.Name);
            }

        }

        private void assembleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            ToolStripMenuItem m = (ToolStripMenuItem) sender;
            bool ischecked= ((ToolStripMenuItem)(contextMenuStrip1.Items[0])).Checked;
            solution.setassemblyrequired(selected.Text, ischecked);

            if (ischecked == true)
            {
                selected.ImageIndex = 7;
                selected.StateImageIndex = 7;

            }
            else
            {
                selected.ImageIndex = 6;
                selected.StateImageIndex = 6;
            }

            treeView1.Update();
        }
    }

    public class SelectedFileEventArgs : EventArgs
    {
        public SelectedFileEventArgs(string name,bool rootnode=false)
        {
            this.name = name;
            this.rootnode = rootnode;
        }

        public string name;
        public bool rootnode;
    }
}