using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DockSample;
using WeifenLuo.WinFormsUI.Docking;
using z80assemble;

namespace Z80IDE
{
    public partial class Form1 : Form
    {

        public Solution solution = new Solution();

        private DummySolutionExplorer m_solutionExplorer;
        private DummyOutputWindow m_outputWindow = new DummyOutputWindow();
        private Dictionary<string, EditorWindow> editors = new Dictionary<string, EditorWindow>();
        z80assembler assembler;
        

        public Form1()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);

            m_solutionExplorer = new DummySolutionExplorer(solution);
        

            m_outputWindow.Show(dockPanel, DockState.DockBottom);
            m_solutionExplorer.Show(dockPanel, DockState.DockLeft);

            //EditorWindow ew = new EditorWindow();
            //ew.MdiParent = this;
            //ew.DockPanel = this.dockPanel;
            //ew.Show();
            //editors.Add("Default.asm",ew);

            m_solutionExplorer.SelectedFile += new DummySolutionExplorer.SelectedFileEventHandler(m_solutionExplorer_SelectedFile);

          //  solution.details.basefolder = "C:\\code\\MCSD100J";
         //   solution.details.name = "MCSD100J";
         //   solution.addfile("SU200.mac", "");
         //   solution.addfile("TEST200.mac", "");

            //solution.Serialize("myz802.sol");
            //solution.Deserialize("myz802.sol");
         
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (solution.isDirty == true)
            {
                if (MessageBox.Show("Solution not saved, continue", "Unsaved changes", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        void m_solutionExplorer_SelectedFile(object sender, SelectedFileEventArgs e)
        {

            //FIXME better document tracking required,
            //if you undock the document its not in dockPanel anymore

            foreach (IDockContent d in dockPanel.Documents)
            {
                //Only one solution supported at a time currently
                if (d.GetType() == typeof(NewSolution) && e.rootnode == true)
                {
                    NewSolution ss = (NewSolution)d;
                    ss.Show();
                    return;
                }

                if (d.GetType() == typeof(EditorWindow) && e.rootnode == false)
                {
                    EditorWindow ew = (EditorWindow)d;
                    if (ew.filename == e.name)
                    {
                        ew.Show();
                        return;
                    }

                }
            }

           if(e.rootnode==true)
           {
                NewSolution ss2 = new NewSolution(solution);
                ss2.MdiParent = this;
                ss2.DockPanel = this.dockPanel;
                ss2.Show();
                return;
           }
           else
           {
                 loadfile(e.name);
           }

         
        }

        public void loadfile(string name)
        {
            string data = solution.loadfile(name);
            EditorWindow ew = new EditorWindow(name);
            ew.Closing += new EditorWindow.EditorClosingHandler(ew_Closing);

            ew.settext(data);
            ew.MdiParent = this;
            ew.DockPanel = this.dockPanel;
            ew.Show();
            editors.Add(name, ew);
           
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = 0;

            
            String filename = "New";

            while (solution.isnameused(filename) == true)
            {
                index++;
                filename = string.Format("New {0}", index);
            }
            
            EditorWindow ew = new EditorWindow(filename);
            ew.MdiParent = this;
            ew.DockPanel = this.dockPanel;
            ew.Show();
            editors.Add(filename,ew);

            ew.Closing += new EditorWindow.EditorClosingHandler(ew_Closing);
            
            solution.addfile(filename);
        }

        void ew_Closing(object sender, EventArgs e)
        {
            EditorWindow ew = (EditorWindow)sender;
            editors.Remove(ew.filename);
            ew.Dispose();
        }

        private void toolStripButtonBuild_Click(object sender, EventArgs e)
        {
            m_outputWindow.clear();
            m_outputWindow.appendmsg("Starting build");
            BuildManager bm = new BuildManager(solution, m_outputWindow);
            bm.build();
        }

        private void openSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Solution files (*.sol)|*.sol";
            //dialog.InitialDirectory = @"C:\";
            dialog.Title = "Please select a solution to open";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                solution.Deserialize(dialog.FileName);
            }             

        }

        private void saveSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //SaveFileDialog dialog = new SaveFileDialog();
            //dialog.Filter = "Solution files (*.sol)|*.sol";
            ////dialog.InitialDirectory = @"C:\";
            //dialog.Title = "Please select a solution to open";
            //if (dialog.ShowDialog() == DialogResult.OK)
            //{
                solution.Serialize();
            //}   
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Assembler files (*.asm)|*.asm|Assembler Files (*.mac)|*.mac|Def files (*.def)|*.def";
            //dialog.InitialDirectory = @"C:\";
            dialog.Title = "Select a file to insert to solution";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                solution.addfile(dialog.FileName);
                solution.Serialize();
            }     
        }

        private void newSolutionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSolution ns = new NewSolution();
            if (ns.ShowDialog() == DialogResult.OK)
            {
                solution.updatedetails(ns.getsolution());
                solution.Serialize();
                
            }
        }

        

      

      

        
    }
}
