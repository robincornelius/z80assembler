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
using System.Collections;

namespace Z80IDE
{
    public partial class SolutionSettings : DockContent
    {
        Solution sol;
        private ListViewColumnSorter lvwColumnSorter;

        public SolutionSettings(Solution sol)
        {
            InitializeComponent();

            this.sol = sol;
            textBox_solutionname.Text = sol.details.name;
            textBox_solutionrootfolder.Text = sol.details.basefolder;
            textBox_ramstart.Text = sol.details.ramstart.ToString();

            foreach(file f in sol.details.files)
            {

                ListViewItem p = new ListViewItem(f.name);

                ListViewItem.ListViewSubItem si = new ListViewItem.ListViewSubItem(p, f.assemblefile.ToString());
                p.SubItems.Add(si);

                if (f.assemblefile == true)
                {
                    si = new ListViewItem.ListViewSubItem(p, f.order.ToString());
                    p.SubItems.Add(si);
                }
                else
                {
                    si = new ListViewItem.ListViewSubItem(p, "");
                    p.SubItems.Add(si);
                }

                listView_filelist.Items.Add(p);

                lvwColumnSorter = new ListViewColumnSorter();
                listView_filelist.ListViewItemSorter = lvwColumnSorter;

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem i in this.listView_filelist.SelectedItems)
            {
                foreach (file f in sol.details.files)
                {
                    if (f.name == i.Text)
                    {
                        f.order++;
                        i.SubItems[2].Text = f.order.ToString();
                        break;
                    }

                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem i in this.listView_filelist.SelectedItems)
            {
                foreach (file f in sol.details.files)
                {
                    if (f.name == i.Text)
                    {
                        f.order--;
                        i.SubItems[2].Text = f.order.ToString();
                        break;
                    }

                }

            }
        }

        private void button_updatesettings_Click(object sender, EventArgs e)
        {
            MessageBox.Show("FIX ME NOT FINISHED");
        }
    }

    public class ListViewColumnSorter : IComparer
    {
        private CaseInsensitiveComparer ObjectCompare = new CaseInsensitiveComparer();

        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;
            int ColumnToSort = 2;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            if (listviewX.SubItems[1].Text == "True" && listviewY.SubItems[1].Text == "False")
                return -1;

            if (listviewX.SubItems[1].Text == "False" && listviewY.SubItems[1].Text == "True")
                return 1;


            if (listviewX.SubItems[1].Text == "False" && listviewY.SubItems[1].Text == "False")
                return 0;

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            return compareResult;
        }

    }
}
