namespace WinREPO
{
    partial class frmOptions
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
            this.txtGitShellPath = new System.Windows.Forms.TextBox();
            this.btnBrowseGitShellPath = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dlgSelectFile = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPowerShellPath = new System.Windows.Forms.TextBox();
            this.btnBrowsePowerShellPath = new System.Windows.Forms.Button();
            this.btnFixPowerShell = new System.Windows.Forms.Button();
            this.dlgSelectDir = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "msysgit Path";
            // 
            // txtGitShellPath
            // 
            this.txtGitShellPath.Location = new System.Drawing.Point(105, 13);
            this.txtGitShellPath.Name = "txtGitShellPath";
            this.txtGitShellPath.Size = new System.Drawing.Size(278, 20);
            this.txtGitShellPath.TabIndex = 1;
            // 
            // btnBrowseGitShellPath
            // 
            this.btnBrowseGitShellPath.Location = new System.Drawing.Point(389, 13);
            this.btnBrowseGitShellPath.Name = "btnBrowseGitShellPath";
            this.btnBrowseGitShellPath.Size = new System.Drawing.Size(31, 23);
            this.btnBrowseGitShellPath.TabIndex = 2;
            this.btnBrowseGitShellPath.Text = "...";
            this.btnBrowseGitShellPath.UseVisualStyleBackColor = true;
            this.btnBrowseGitShellPath.Click += new System.EventHandler(this.btnBrowseGitShellPath_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(9, 72);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 3;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dlgSelectFile
            // 
            this.dlgSelectFile.FileName = "shell.ps1";
            this.dlgSelectFile.InitialDirectory = "C:\\Users";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "PowerShell Path";
            // 
            // txtPowerShellPath
            // 
            this.txtPowerShellPath.Location = new System.Drawing.Point(105, 40);
            this.txtPowerShellPath.Name = "txtPowerShellPath";
            this.txtPowerShellPath.Size = new System.Drawing.Size(278, 20);
            this.txtPowerShellPath.TabIndex = 6;
            // 
            // btnBrowsePowerShellPath
            // 
            this.btnBrowsePowerShellPath.Location = new System.Drawing.Point(389, 38);
            this.btnBrowsePowerShellPath.Name = "btnBrowsePowerShellPath";
            this.btnBrowsePowerShellPath.Size = new System.Drawing.Size(31, 23);
            this.btnBrowsePowerShellPath.TabIndex = 7;
            this.btnBrowsePowerShellPath.Text = "...";
            this.btnBrowsePowerShellPath.UseVisualStyleBackColor = true;
            this.btnBrowsePowerShellPath.Click += new System.EventHandler(this.btnBrowsePowerShellPath_Click);
            // 
            // btnFixPowerShell
            // 
            this.btnFixPowerShell.Location = new System.Drawing.Point(105, 72);
            this.btnFixPowerShell.Name = "btnFixPowerShell";
            this.btnFixPowerShell.Size = new System.Drawing.Size(113, 23);
            this.btnFixPowerShell.TabIndex = 8;
            this.btnFixPowerShell.Text = "Fix PowerShell";
            this.btnFixPowerShell.UseVisualStyleBackColor = true;
            this.btnFixPowerShell.Click += new System.EventHandler(this.btnFixPowerShell_Click);
            // 
            // frmOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(427, 107);
            this.ControlBox = false;
            this.Controls.Add(this.btnFixPowerShell);
            this.Controls.Add(this.btnBrowsePowerShellPath);
            this.Controls.Add(this.txtPowerShellPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnBrowseGitShellPath);
            this.Controls.Add(this.txtGitShellPath);
            this.Controls.Add(this.label1);
            this.Name = "frmOptions";
            this.Text = "WinREPO Options/Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGitShellPath;
        private System.Windows.Forms.Button btnBrowseGitShellPath;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog dlgSelectFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPowerShellPath;
        private System.Windows.Forms.Button btnBrowsePowerShellPath;
        private System.Windows.Forms.Button btnFixPowerShell;
        private System.Windows.Forms.FolderBrowserDialog dlgSelectDir;
    }
}