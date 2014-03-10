/*
    WinREPO - Repo For Windows! No more messing around with cygwin, go native on Winows!
    Copyright (C) 2014  Naresh Mehta naresh.mehta@gmail.com

    This file is part of WinREPO.

    WinREPO is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    WinREPO is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with WinREPO.  If not, see <http://www.gnu.org/licenses/>. Or
    write to the Free Software Foundation, Inc., 51 Franklin Street, 
    Fifth Floor, Boston, MA 02110-1301  USA
 */
namespace WinREPO
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.txtRepoURL = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRepoInit = new System.Windows.Forms.Button();
            this.btnRepoSync = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAbout = new System.Windows.Forms.Button();
            this.txtLocalRepoDirPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOptions = new System.Windows.Forms.Button();
            this.listBoxOutput = new System.Windows.Forms.ListBox();
            this.timerOutput = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtRepoURL
            // 
            this.txtRepoURL.Location = new System.Drawing.Point(73, 3);
            this.txtRepoURL.Name = "txtRepoURL";
            this.txtRepoURL.Size = new System.Drawing.Size(476, 20);
            this.txtRepoURL.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Repo URL";
            // 
            // btnRepoInit
            // 
            this.btnRepoInit.Location = new System.Drawing.Point(3, 84);
            this.btnRepoInit.Name = "btnRepoInit";
            this.btnRepoInit.Size = new System.Drawing.Size(75, 23);
            this.btnRepoInit.TabIndex = 2;
            this.btnRepoInit.Text = "Repo Init";
            this.btnRepoInit.UseVisualStyleBackColor = true;
            this.btnRepoInit.Click += new System.EventHandler(this.btnRepoInit_Click);
            // 
            // btnRepoSync
            // 
            this.btnRepoSync.Location = new System.Drawing.Point(84, 84);
            this.btnRepoSync.Name = "btnRepoSync";
            this.btnRepoSync.Size = new System.Drawing.Size(75, 23);
            this.btnRepoSync.TabIndex = 3;
            this.btnRepoSync.Text = "Repo Sync";
            this.btnRepoSync.UseVisualStyleBackColor = true;
            this.btnRepoSync.Click += new System.EventHandler(this.btnRepoSync_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAbout);
            this.panel1.Controls.Add(this.txtLocalRepoDirPath);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnRepoSync);
            this.panel1.Controls.Add(this.btnOptions);
            this.panel1.Controls.Add(this.btnRepoInit);
            this.panel1.Controls.Add(this.txtRepoURL);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(16, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(571, 110);
            this.panel1.TabIndex = 4;
            // 
            // btnAbout
            // 
            this.btnAbout.Location = new System.Drawing.Point(474, 84);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(75, 23);
            this.btnAbout.TabIndex = 7;
            this.btnAbout.Text = "About...";
            this.btnAbout.UseVisualStyleBackColor = true;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // txtLocalRepoDirPath
            // 
            this.txtLocalRepoDirPath.Location = new System.Drawing.Point(73, 35);
            this.txtLocalRepoDirPath.Name = "txtLocalRepoDirPath";
            this.txtLocalRepoDirPath.Size = new System.Drawing.Size(476, 20);
            this.txtLocalRepoDirPath.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(3, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 28);
            this.label2.TabIndex = 4;
            this.label2.Text = "Local Repo Directory";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(359, 84);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 6;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOptions
            // 
            this.btnOptions.Location = new System.Drawing.Point(234, 84);
            this.btnOptions.Name = "btnOptions";
            this.btnOptions.Size = new System.Drawing.Size(119, 23);
            this.btnOptions.TabIndex = 5;
            this.btnOptions.Text = "Options / Settings";
            this.btnOptions.UseVisualStyleBackColor = true;
            this.btnOptions.Click += new System.EventHandler(this.btnOptions_Click);
            // 
            // listBoxOutput
            // 
            this.listBoxOutput.FormattingEnabled = true;
            this.listBoxOutput.HorizontalScrollbar = true;
            this.listBoxOutput.Location = new System.Drawing.Point(16, 131);
            this.listBoxOutput.Name = "listBoxOutput";
            this.listBoxOutput.Size = new System.Drawing.Size(571, 186);
            this.listBoxOutput.TabIndex = 8;
            // 
            // timerOutput
            // 
            this.timerOutput.Enabled = true;
            this.timerOutput.Tick += new System.EventHandler(this.timerOutput_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 325);
            this.Controls.Add(this.listBoxOutput);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "WinREPO (c) 2014 Naresh Mehta, http://www.naresh.se/";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtRepoURL;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRepoInit;
        private System.Windows.Forms.Button btnRepoSync;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLocalRepoDirPath;
        private System.Windows.Forms.Button btnOptions;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.ListBox listBoxOutput;
        private System.Windows.Forms.Timer timerOutput;
        private System.Windows.Forms.Button btnAbout;
    }
}

