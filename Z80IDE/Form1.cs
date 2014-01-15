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

            m_solutionExplorer = new DummySolutionExplorer(solution);
        

            m_outputWindow.Show(dockPanel, DockState.DockBottom);
            m_solutionExplorer.Show(dockPanel, DockState.DockLeft);

            //EditorWindow ew = new EditorWindow();
            //ew.MdiParent = this;
            //ew.DockPanel = this.dockPanel;
            //ew.Show();
            //editors.Add("Default.asm",ew);

            m_solutionExplorer.SelectedFile += new DummySolutionExplorer.SelectedFileEventHandler(m_solutionExplorer_SelectedFile);

            solution.basefolder = "C:\\code\\MCSD100J";
            solution.name = "MCSD100J";
            solution.addfile("SU200.mac", "");
            solution.addfile("TEST200.mac", "");
         
        }

        void m_solutionExplorer_SelectedFile(object sender, SelectedFileEventArgs e)
        {

            if(editors.ContainsKey(e.name))
            {
                 editors[e.name].Select();
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
            
            solution.addfile(filename, "");
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
            solution.build(m_outputWindow);
        }

      

      

        
    }
}
