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
using System.IO;

namespace Z80IDE
{
    public partial class Form1 : Form
    {

        public Solution solution = new Solution();

        private DummySolutionExplorer m_solutionExplorer;
        private DummyOutputWindow m_outputWindow = new DummyOutputWindow();
        private Dictionary<string, EditorWindow> editors = new Dictionary<string, EditorWindow>();
        z80assembler assembler;

        private List<string> _mru = new List<string>();

        public Form1()
        {
         
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            this.FormClosed += new FormClosedEventHandler(Form1_FormClosed);

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

            updatemru();
           
         
        }

        void updatemru()
        {
            if (Properties.Settings.Default.MRU != null)
            {
                foreach (string s in Properties.Settings.Default.MRU)
                {
                    if (File.Exists(s) && !_mru.Contains(s))
                    {
                        _mru.Add(s);
                    }
                }
            }

            foreach (var path in _mru)
            {
                var item = new ToolStripMenuItem(path);
                item.Tag = path;
                item.Click += OpenRecentFile;
                mru_menu.DropDownItems.Add(item);
            }

        }

        void OpenRecentFile(object sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender;
            var filepath = (string)menuItem.Tag;
            solution.Deserialize(filepath);
           
        }

        void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (string s in _mru)
            {
                Properties.Settings.Default.MRU.Add(s);

            }

           Properties.Settings.Default.Save();
        

        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (solution.isDirty == true)
            {
                if (MessageBox.Show("Solution not saved, continue", "Unsaved changes", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {

                    e.Cancel = true;
                    return;
                }
            }

          

           // File.WriteAllLines(mruFilePath, _mru);
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
            ew.EditorClosing += new EditorWindow.EditorClosingHandler(ew_Closing);

            ew.settext(data);
            ew.MdiParent = this;
            ew.DockPanel = this.dockPanel;
            ew.Show();
            editors.Add(name, ew);
           
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newfile();
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

            HexView hv = new HexView(bm.getoutput());
            hv.MdiParent = this;
            hv.DockPanel = this.dockPanel;
            hv.Show();
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
                _mru.Insert(0, dialog.FileName);
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
            openfile();
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

        private void toolStripButton_new_Click(object sender, EventArgs e)
        {
            newfile();
        }

        private void toolStripButton_open_Click(object sender, EventArgs e)
        {
            openfile();
        }

        private void toolStripButton_outputwindow_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStripButton_solutionexplorer_Click(object sender, EventArgs e)
        {
            if (m_solutionExplorer == null)
            {
                m_solutionExplorer = new DummySolutionExplorer(solution);
                m_solutionExplorer.Show(dockPanel, DockState.DockLeft);
            }
            else
            {
                m_solutionExplorer.Close();
                m_solutionExplorer = null;
            }

        }

        private void newfile()
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

            ew.EditorClosing += new EditorWindow.EditorClosingHandler(ew_Closing);
            
            solution.addfile(filename,false);

        }

        private void openfile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Assembler files (*.asm,*.mac)|*.asm;*.mac|Def files (*.def)|*.def";
            //dialog.InitialDirectory = @"C:\";
            dialog.Title = "Select a file to insert to solution";
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filename in dialog.FileNames)
                {
                    solution.addfile(filename);
                }
                solution.Serialize();
            }     

        }

      

      

        
    }
}
