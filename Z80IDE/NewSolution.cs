using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Z80IDE
{
    public partial class NewSolution : Form
    {
        Solution sol = new Solution();

        public NewSolution()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {

            sol.details.name = textBox_solutionname.Text;
            sol.details.basefolder = textBox_solutionroot.Text + System.IO.Path.DirectorySeparatorChar + sol.details.name;
           sol.details.filefolder = sol.details.basefolder + System.IO.Path.DirectorySeparatorChar + "files";
           sol.details.name = textBox_solutionname.Text;

           if (Directory.Exists(sol.details.basefolder))
           {
               MessageBox.Show("Directory " + sol.details.basefolder + " already exists");
               return;
           }

           Directory.CreateDirectory(sol.details.basefolder);
           Directory.CreateDirectory(sol.details.filefolder);
           
           DialogResult = DialogResult.OK;
           Close();

        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();

        }

        private void button_changefolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Choose a root folder for your solution";
            fbd.ShowNewFolderButton = true;
            fbd.RootFolder = Environment.SpecialFolder.Personal;

            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK)
            {
                string folderName = fbd.SelectedPath;
                textBox_solutionroot.Text = folderName;

            }

        }

        public solutiondetails getsolution()
        {
            return sol.details;
        }
    }
}
