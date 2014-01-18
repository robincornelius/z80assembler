namespace Z80IDE
{
    partial class SolutionSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox_solutionrootfolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_solutionname = new System.Windows.Forms.TextBox();
            this.listView_filelist = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_ramstart = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.button_updatesettings = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Solution root folder";
            // 
            // textBox_solutionrootfolder
            // 
            this.textBox_solutionrootfolder.Location = new System.Drawing.Point(123, 59);
            this.textBox_solutionrootfolder.Name = "textBox_solutionrootfolder";
            this.textBox_solutionrootfolder.Size = new System.Drawing.Size(474, 20);
            this.textBox_solutionrootfolder.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Solution name";
            // 
            // textBox_solutionname
            // 
            this.textBox_solutionname.Location = new System.Drawing.Point(123, 20);
            this.textBox_solutionname.Name = "textBox_solutionname";
            this.textBox_solutionname.Size = new System.Drawing.Size(116, 20);
            this.textBox_solutionname.TabIndex = 3;
            // 
            // listView_filelist
            // 
            this.listView_filelist.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.listView_filelist.FullRowSelect = true;
            this.listView_filelist.Location = new System.Drawing.Point(24, 140);
            this.listView_filelist.Name = "listView_filelist";
            this.listView_filelist.Size = new System.Drawing.Size(433, 169);
            this.listView_filelist.TabIndex = 4;
            this.listView_filelist.UseCompatibleStateImageBehavior = false;
            this.listView_filelist.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "File";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Compile";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Order";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Ram start";
            // 
            // textBox_ramstart
            // 
            this.textBox_ramstart.Location = new System.Drawing.Point(123, 97);
            this.textBox_ramstart.Name = "textBox_ramstart";
            this.textBox_ramstart.Size = new System.Drawing.Size(129, 20);
            this.textBox_ramstart.TabIndex = 12;
            this.textBox_ramstart.Text = "16384";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(472, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(41, 24);
            this.button1.TabIndex = 14;
            this.button1.Text = "UP";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(519, 149);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(53, 24);
            this.button2.TabIndex = 15;
            this.button2.Text = "DOWN";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(478, 127);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Assemble Priority";
            // 
            // button_updatesettings
            // 
            this.button_updatesettings.Location = new System.Drawing.Point(313, 330);
            this.button_updatesettings.Name = "button_updatesettings";
            this.button_updatesettings.Size = new System.Drawing.Size(95, 27);
            this.button_updatesettings.TabIndex = 17;
            this.button_updatesettings.Text = "Update settings";
            this.button_updatesettings.UseVisualStyleBackColor = true;
            this.button_updatesettings.Click += new System.EventHandler(this.button_updatesettings_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(769, 25);
            this.toolStrip1.TabIndex = 18;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // SolutionSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 378);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.button_updatesettings);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_ramstart);
            this.Controls.Add(this.listView_filelist);
            this.Controls.Add(this.textBox_solutionname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox_solutionrootfolder);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SolutionSettings";
            this.Text = "SolutionSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox_solutionrootfolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_solutionname;
        private System.Windows.Forms.ListView listView_filelist;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox_ramstart;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button_updatesettings;
        private System.Windows.Forms.ToolStrip toolStrip1;
    }
}