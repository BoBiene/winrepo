using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

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

        //private const String strPowerShellPath = "%SystemRoot%\\system32\\WindowsPowerShell\\v1.0\\powershell.exe ";
        public String _strGitHubFileName { get; set; }
        public String _strPowerShellPath { get; set; }

        public frmOptions()
        {
            InitializeComponent();
            readRegistryKeysIfAny();
        }

        public void readRegistryKeysIfAny()
        {
            _strGitHubFileName = (String) Registry.GetValue(_strKeyName, _strRegGitHubPath, "");
            _strPowerShellPath = (String) Registry.GetValue(_strKeyName, _strRegPowerShellPath, "");
            txtGitShellPath.Text = _strGitHubFileName;
            txtPowerShellPath.Text = _strPowerShellPath;
        }

        private void btnBrowseGitShellPath_Click(object sender, EventArgs e)
        {
            dlgSelectFile.FileName = "shell.ps1";
            dlgSelectFile.InitialDirectory = "C:\\Users";
            if (dlgSelectFile.ShowDialog() == DialogResult.OK)
            {
                _strGitHubFileName = dlgSelectFile.FileName;
                txtGitShellPath.Text = _strGitHubFileName;
            }
        }

/*
        private void startPowerShellPrompt()
        {
            if (_strPowerShellPath.Length > 0)
            {
                System.Diagnostics.Process _process = new System.Diagnostics.Process();
                _process.StartInfo.FileName = _strPowerShellPath;
                _process.StartInfo.Verb = "runas";
                _process.StartInfo.Arguments = _strPowerShellExePolicyArg;
                _process.StartInfo.UseShellExecute = true;
                _process.Start();
            }
        }
*/
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_strGitHubFileName == null)
            {
                _strGitHubFileName = txtGitShellPath.Text;
            }
            if (_strPowerShellPath == null)
            {
                _strPowerShellPath = txtPowerShellPath.Text;
            }
            Registry.SetValue(_strKeyName, _strRegGitHubPath, _strGitHubFileName);
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
    }
}
