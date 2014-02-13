using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;

/*
 1. Open power Shell using Admin Priviledges
 * 1.1 Type-in "set-executionpolicy remotesigned" 
 * 1.2 Answer Y
 2. Now invoke powershell using: %SystemRoot%\system32\WindowsPowerShell\v1.0\powershell.exe -ExecutionPolicy remotesigned -noexit "& "C:\Users\emehnar\AppData\Local\GitHub\shell.ps1"
 
 */
namespace WinREPO
{
    public partial class frmOptions : Form
    {
        private const String _strUserRoot = "HKEY_CURRENT_USER";
        private const String _strRegKey = "WinREPO";
        private const String _strRegGitHubPath = "GitHubPath";
        private const String _strRegPowerShellPath = "PowerShellPath";
        private const String _strKeyName = _strUserRoot + "\\" + _strRegKey;

        private frmMain _frmMainInstanceHandle = null;

        //private const String strPowerShellPath = "%SystemRoot%\\system32\\WindowsPowerShell\\v1.0\\powershell.exe ";
        public String _strGitFolderPath { get; set; }
        public String _strPowerShellPath { get; set; }

        public frmOptions(frmMain parent)
        {
            InitializeComponent();
            _frmMainInstanceHandle = parent;
            readRegistryKeysIfAny();
        }

        public void readRegistryKeysIfAny()
        {
            _strGitFolderPath = (String)Registry.GetValue(_strKeyName, _strRegGitHubPath, "");
            _strPowerShellPath = (String) Registry.GetValue(_strKeyName, _strRegPowerShellPath, "");
            txtGitShellPath.Text = _strGitFolderPath;
            txtPowerShellPath.Text = _strPowerShellPath;
        }

        private void btnBrowseGitShellPath_Click(object sender, EventArgs e)
        {
            dlgSelectDir.RootFolder = Environment.SpecialFolder.ProgramFilesX86;
            if (dlgSelectDir.ShowDialog() == DialogResult.OK)
            {
                _strGitFolderPath = dlgSelectDir.SelectedPath;
                txtGitShellPath.Text = _strGitFolderPath;
            }
        }

        private void startPowerShellPrompt()
        {
            if (_strPowerShellPath.Length > 0)
            {
                System.Diagnostics.Process _process = new System.Diagnostics.Process();
                _process.StartInfo.FileName = _strPowerShellPath;
                _process.StartInfo.Verb = "runas";
                _process.StartInfo.Arguments = "-ExecutionPolicy remotesigned -noexit \" & \"set-executionpolicy remotesigned\r\n";
                _process.StartInfo.UseShellExecute = true;
                _process.Start();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_strGitFolderPath == null)
            {
                _strGitFolderPath = txtGitShellPath.Text;
            }
            if (_strPowerShellPath == null)
            {
                _strPowerShellPath = txtPowerShellPath.Text;
            }
            Registry.SetValue(_strKeyName, _strRegGitHubPath, _strGitFolderPath);
            Registry.SetValue(_strKeyName, _strRegPowerShellPath, _strPowerShellPath);
            this.Hide();
        }

        private void btnBrowsePowerShellPath_Click(object sender, EventArgs e)
        {
            dlgSelectFile.FileName = "powershell.exe";
            dlgSelectFile.InitialDirectory = "C:\\Windows\\System32\\WindowsPowerShell\\v1.0";
            if (dlgSelectFile.ShowDialog() == DialogResult.OK)
            {
                _strPowerShellPath = dlgSelectFile.FileName;
                txtPowerShellPath.Text = _strPowerShellPath;
            }
        }

        private void btnFixPowerShell_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Type Exit in the resulting command prompt or close the window. This process needs to be done only once!");
            startPowerShellPrompt();
        }

        private void btnFixSSHConfig_Click(object sender, EventArgs e)
        {
            String homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            homePath += "\\.ssh";
            String strConfigFileName = "config";
            String strConfigFilePath = homePath + "\\" + strConfigFileName;
            Console.WriteLine("SSH Config path is :" + strConfigFilePath);
            if (File.Exists(strConfigFilePath))
            {
                /* Config File Already exists. Lets try to write to the end of the config file. Maybe create a backup first. */
                if ((MessageBox.Show("SSH Config file Found! A backup of the file will be created and modifications done. Continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                {
                    return;
                }
                /* Create a backup now... */
                File.Copy(strConfigFilePath, homePath + "\\" + "WinREPO_ssh_backup_config", true);
            }
            /* Now write to the file as needed. We will execute a PowerShell Script to do so. Use the utility function in frmMain... */
            String strCommand = "\"" + Directory.GetCurrentDirectory() + "\\modifySSHConfig.ps1 " + "-configPath " + strConfigFilePath + "\"" + frmMain._strNewline;
            Console.WriteLine("SSH Config Fix Command is :" + strCommand);
            _frmMainInstanceHandle.startPowerShellScript(strCommand);
            MessageBox.Show("SSH Config has been modified to work with WinREPO at " + strConfigFilePath + ". Please restore the backup when you uninstall WinREPO in future!");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
