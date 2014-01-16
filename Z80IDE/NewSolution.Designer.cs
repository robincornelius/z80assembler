namespace Z80IDE
{
    partial class NewSolution
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
            this.button_changefolder = new System.Windows.Forms.Button();
            this.textBox_solutionroot = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_solutionname = new System.Windows.Forms.TextBox();
            this.button_ok = new System.Windows.Forms.Button();
            this.button_cancel = new System.Windows.Forms.Button();
            this.textBox_ramstart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button_changefolder
            // 
            this.button_changefolder.Location = new System.Drawing.Point(609, 61);
            this.button_changefolder.Name = "button_changefolder";
            this.button_changefolder.Size = new System.Drawing.Size(85, 27);
            this.button_changefolder.TabIndex = 5;
            this.button_changefolder.Text = "Change";
            this.button_changefolder.UseVisualStyleBackColor = true;
            this.button_changefolder.Click += new System.EventHandler(this.button_changefolder_Click);
            // 
            // textBox_solutionroot
            // 
            this.textBox_solutionroot.Location = new System.Drawing.Point(126, 64);
            this.textBox_solutionroot.Name = "textBox_solutionroot";
            this.textBox_solutionroot.Size = new System.Drawing.Size(474, 20);
            this.textBox_solutionroot.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 69);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Solution root folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Solution name";
            // 
            // textBox_solutionname
            // 
            this.textBox_solutionname.Location = new System.Drawing.Point(126, 20);
            this.textBox_solutionname.Name = "textBox_solutionname";
            this.textBox_solutionname.Size = new System.Drawing.Size(129, 20);
            this.textBox_solutionname.TabIndex = 7;
            // 
            // button_ok
            // 
            this.button_ok.Location = new System.Drawing.Point(204, 165);
            this.button_ok.Name = "button_ok";
            this.button_ok.Size = new System.Drawing.Size(85, 27);
            this.button_ok.TabIndex = 8;
            this.button_ok.Text = "OK";
            this.button_ok.UseVisualStyleBackColor = true;
            this.button_ok.Click += new System.EventHandler(this.button_ok_Click);
            // 
            // button_cancel
            // 
            this.button_cancel.Location = new System.Drawing.Point(398, 165);
            this.button_cancel.Name = "button_cancel";
            this.button_cancel.Size = new System.Drawing.Size(85, 27);
            this.button_cancel.TabIndex = 9;
            this.button_cancel.Text = "Cancel";
            this.button_cancel.UseVisualStyleBackColor = true;
            this.button_cancel.Click += new System.EventHandler(this.button_cancel_Click);
            // 
            // textBox_ramstart
            // 
            this.textBox_ramstart.Location = new System.Drawing.Point(126, 103);
            this.textBox_ramstart.Name = "textBox_ramstart";
            this.textBox_ramstart.Size = new System.Drawing.Size(129, 20);
            this.textBox_ramstart.TabIndex = 10;
            this.textBox_ramstart.Text = "16384";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Ram start";
            // 
            // NewSolution
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(770, 223);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_ramstart);
            this.Controls.Add(this.button_cancel);
            this.Controls.Add(this.button_ok);
            this.Controls.Add(this.textBox_solutionname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button_changefolder);
            this.Controls.Add(this.textBox_solutionroot);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "NewSolution";
            this.Text = "NewSolution";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button_changefolder;
        private System.Windows.Forms.TextBox textBox_solutionroot;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_solutionname;
        private System.Windows.Forms.Button button_ok;
        private System.Windows.Forms.Button button_cancel;
        private System.Windows.Forms.TextBox textBox_ramstart;
        private System.Windows.Forms.Label label3;
    }
}