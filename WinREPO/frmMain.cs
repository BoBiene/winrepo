﻿/*
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management.Automation.Runspaces;
using System.Management.Automation;
using System.IO;

namespace WinREPO
{
    public partial class frmMain : Form
    {
        private frmOptions _frmOptions = null;
        private ManifestParser _manifestParser;
        private Runspace _powerShellRunSpace;
        private PipelineExecutor _pipelineExecutor;

        private event EventHandler ProjectCheckoutDone;

        private uint _threadExecutionState;

        private int _currentProjectNumber = 0;
        private Boolean _isRepoSyncing = false;
        private Boolean _movDirectoryToRightLocation = false;
        private const String _tempCheckoutPath = "C:\\tmpRepo";

        private String _strRootDir = "";

        private const String _strSetExecutionPolicy = "set-executionpolicy Unrestricted\r\n";
        public const String _strNewline = "\r\n";
        private const String _strHideRepoDir = "attrib +h .repo";
        private const String _strGitProgress = " --progress";
        private const String _strCDRepo = "cd .repo";

        private void initializeWinREPO()
        {
            this.ProjectCheckoutDone += frmMain_ProjectCheckoutDone;
            _powerShellRunSpace = RunspaceFactory.CreateRunspace();
            _powerShellRunSpace.Open();
            try
            {
                RunspaceInvoke scriptInvoker = new RunspaceInvoke(_powerShellRunSpace);
                scriptInvoker.Invoke("Set-ExecutionPolicy Unrestricted");
            }
            catch (Exception)
            {
                Console.WriteLine("OK! We don't have access bla bla. I will fix it soon...");
            }

            _frmOptions = new frmOptions(this);
            _frmOptions.readRegistryKeysIfAny();

            if (_frmOptions._strGitFolderPath == null || _frmOptions._strPowerShellPath == null)
            {
                MessageBox.Show("Please Setup the proper variables in Settings and fix your PowerShell and SSHConfigs. Otherwise WinREPO will not work!!!");
                _frmOptions.ShowDialog();
            }

            String strInit = "& \"" + Directory.GetCurrentDirectory() + "\\gitshell.ps1\" ";
            strInit += "-gitPath \"" + _frmOptions._strGitFolderPath + "\"" + _strNewline;
            Console.WriteLine("Current Working Directory: " + strInit);
            startPowerShellScript(strInit);
        }

        private void frmMain_ProjectCheckoutDone(object sender, EventArgs e)
        {
            if (!_isRepoSyncing)
            {
                return;
            }
            if (_movDirectoryToRightLocation)
            {
                _movDirectoryToRightLocation = false;
                String strTemp = _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath;
                ZetaLongPaths.ZlpIOHelper.MoveDirectory(_tempCheckoutPath + "\\" + strTemp.Substring(strTemp.LastIndexOf("/") + 1), 
                    txtLocalRepoDirPath.Text + "\\" + _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath);
            }
            _currentProjectNumber++;
            Invoke(new MethodInvoker(delegate()
            {
                checkoutNextProject(_currentProjectNumber);
            }));
        }
        public frmMain()
        {
            InitializeComponent();
            initializeWinREPO();
        }

        public void startPowerShellScript(String strScript)
        {
            _pipelineExecutor = new PipelineExecutor(_powerShellRunSpace, this, strScript);
            _pipelineExecutor.OnDataEnd += new PipelineExecutor.DataEndDelegate(PipelineExecutor_OnDataEnd);
            _pipelineExecutor.OnDataReady += new PipelineExecutor.DataReadyDelegate(PipelineExecutor_OnDataReady);
            _pipelineExecutor.OnErrorReady += new PipelineExecutor.ErrorReadyDelegate(PipelineExecutor_OnErrorReady);

            _pipelineExecutor.Start();
        }

        private void stopPowerShellScript()
        {
            if (_pipelineExecutor != null)
            {
                _pipelineExecutor.OnDataEnd -= new PipelineExecutor.DataEndDelegate(PipelineExecutor_OnDataEnd);
                _pipelineExecutor.OnDataReady -= new PipelineExecutor.DataReadyDelegate(PipelineExecutor_OnDataReady);
                _pipelineExecutor.OnErrorReady -= new PipelineExecutor.ErrorReadyDelegate(PipelineExecutor_OnErrorReady);
                _pipelineExecutor.Stop();
                _pipelineExecutor = null;
                System.Threading.Thread.Sleep(100);
            }
        }

        private bool listBoxChanged = false;
        private void AppendLine(string line)
        {
            listBoxChanged = true;
            if (listBoxOutput.Items.Count > 200)
            {
                listBoxOutput.Items.RemoveAt(0);
            }
            listBoxOutput.Items.Add(line);
            listBoxOutput.TopIndex = listBoxOutput.Items.Count - 1;
        }

        private void PipelineExecutor_OnErrorReady(PipelineExecutor sender, ICollection<object> data)
        {
            foreach (object e in data)
            {
                AppendLine(e.ToString());
            }
        }

        private void PipelineExecutor_OnDataReady(PipelineExecutor sender, ICollection<System.Management.Automation.PSObject> data)
        {
            foreach (PSObject obj in data)
            {
                AppendLine(obj.ToString());
            }
        }

        private void PipelineExecutor_OnDataEnd(PipelineExecutor sender)
        {
            EventArgs e = new EventArgs();
            if (sender.Pipeline.PipelineStateInfo.State == PipelineState.Failed)
            {
                AppendLine(string.Format("Error in script: {0}", sender.Pipeline.PipelineStateInfo.Reason));
            }
            else
            {
                AppendLine("Ready.");
                ProjectCheckoutDone(this, e);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnOptions_Click(object sender, EventArgs e)
        {
            _frmOptions.ShowDialog();
        }

        private void setRootDirectory()
        {
            String strGitClonePath = txtRepoURL.Text;
            if (txtRepoURL.Text.Contains("tags"))
            {
                strGitClonePath = strGitClonePath.Substring(0, txtRepoURL.Text.IndexOf("-b"));
            }

            _strRootDir = txtRepoURL.Text.Substring(0, txtRepoURL.Text.IndexOf("-b") - 1);
            _strRootDir = _strRootDir.Substring(_strRootDir.LastIndexOf("/") + 1);
        }

        private Boolean checkIfRepoExists(String strPath)
        {
            Boolean result = false;
            if (Directory.Exists(strPath))
            {
                result = true;
            }
            return result;
        }

        private Boolean IsDirectoryEmpty(string path)
        {
            IEnumerable<string> items = Directory.EnumerateFileSystemEntries(path);
            using (IEnumerator<string> en = items.GetEnumerator())
            {
                return !en.MoveNext();
            }
        }

        private void btnRepoInit_Click(object sender, EventArgs e)
        {
            stopPowerShellScript();
            String strGitTag = "";
            String strGitClonePath = txtRepoURL.Text;

            setRootDirectory();
            if (txtRepoURL.Text.Contains("tags"))
            {
                Console.WriteLine("Found a tag. Convert it go Git Stuff... ");
                strGitTag = txtRepoURL.Text.Substring(txtRepoURL.Text.IndexOf("tags"));
                strGitTag = "git checkout " + strGitTag;

                strGitClonePath = strGitClonePath.Substring(0, txtRepoURL.Text.IndexOf("-b"));
            }

            /* Do a git pull if a directory (.repo) exits in the path i.e. there is an already existing
             repo. Otherwise do a new git clone. */
            String strCommand = "cd " + txtLocalRepoDirPath.Text + _strNewline;
            if (!IsDirectoryEmpty(txtLocalRepoDirPath.Text))
            {
                if((MessageBox.Show("Selected Repo Directory is NOT EMPTY! Do you still want to Continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No))
                {
                    return;
                }
            }
            if (checkIfRepoExists(txtLocalRepoDirPath.Text + "\\.repo"))
            {
                strCommand += _strCDRepo + _strNewline + "git pull" + _strNewline + strGitTag + _strNewline + "cd .." + _strNewline;
            }
            else
            {
                strCommand += "git clone " + strGitClonePath + _strGitProgress + _strNewline + "cd " + _strRootDir +
                    _strNewline + strGitTag + _strNewline + "cd .." + _strNewline +
                    "mv " + _strRootDir + " .repo" + _strNewline + _strHideRepoDir + _strNewline;
            }
            /* At both places we are back in the directory where the .repo is located.
             Now we need to create a manifests directory and put the xml file as a default.xml in there.
             * No need to check if the directory is there since we will just get an error and we continue */
            strCommand += _strCDRepo + _strNewline + "mkdir manifests" + _strNewline + "cp *.xml manifests\\default.xml" + _strNewline;
            /* Now come back to the .repo folder and create a manifests.git where all the .git should be copied */

            AppendLine(strCommand);
            startPowerShellScript(strCommand);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            _powerShellRunSpace.Close();
        }

        private void timerOutput_Tick(object sender, EventArgs e)
        {
            if (listBoxChanged)
            {
                listBoxOutput.EndUpdate();
                listBoxOutput.Update();
                listBoxChanged = false;
                listBoxOutput.BeginUpdate();
            }
            else
            {
                listBoxOutput.EndUpdate();
            }
        }

        private const int MAX_PATH = 250;
        private Boolean isPathExceedingMaxPath(String strPath)
        {
            if (strPath.Length > MAX_PATH)
            {
                return true;
            }
            return false;
        }

        private void checkoutNextProject(int iProjectNumber)
        {
            stopPowerShellScript();
            String strCommand = "";

            strCommand += "cd " + txtLocalRepoDirPath.Text + _strNewline;
            if (_currentProjectNumber > _manifestParser._manifestConfig._projectPathConfigs.Length - 1)
            {
                Console.WriteLine("All projects Checkedout as needed. RepoSync Done");
                AppendLine("Repo Sync DONE!");
                _isRepoSyncing = false;
                _currentProjectNumber = 0;
                return;
            }
            int iServerConfIt = _manifestParser.getServerConfigForProject(_currentProjectNumber);
            if (iServerConfIt == -1)
            {
                AppendLine("Error: Encountered while synching. Please recheck the manifest.xml file... EXITING SYNC!");
                _isRepoSyncing = false;
                _currentProjectNumber = 0;
                return;
            }
            if (isPathExceedingMaxPath(txtLocalRepoDirPath.Text + "\\" + _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath))
            {
                _movDirectoryToRightLocation = true;
                /* Prepare the nice command... */
                strCommand += "git clone " + _manifestParser._manifestConfig._remoteServerConfigs[iServerConfIt]._strRemoteFetch +
                    _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strName + " " +
                    _tempCheckoutPath /*+ " -b " +
                    ((_manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch == null) ?
                    _manifestParser._manifestConfig._strDefaultRevision : _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch)*/ +
                    _strGitProgress + _strNewline;
                /* Now move into that directory where we just cloned!*/
                String strTemp = _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath;
                strCommand += "cd " + _tempCheckoutPath + "\\" + strTemp.Substring(strTemp.LastIndexOf("/") + 1) + _strNewline;
            }
            else
            {
                _movDirectoryToRightLocation = false;
                /* Prepare the nice command... */
                strCommand += "git clone " + _manifestParser._manifestConfig._remoteServerConfigs[iServerConfIt]._strRemoteFetch +
                    _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strName + " " +
                    _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath /*+ " -b " +
                    ((_manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch == null) ?
                    _manifestParser._manifestConfig._strDefaultRevision : _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch)*/ +
                    _strGitProgress + _strNewline;
                /* Now move into that directory where we just cloned!*/
                strCommand += "cd " + _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath + _strNewline;
            }

            /* And do a checkout */
            strCommand += "git checkout " +
                ((_manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strRevision == null) ?
                    _manifestParser._manifestConfig._strDefaultRevision :
                    _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strRevision) + _strNewline;

            AppendLine(strCommand);
            startPowerShellScript(strCommand);
        }

        /* TODO:
         * 1. Get the valid XML paths for all the files in the .repo structure.
         * 2. Create a manifest.xml for only the file outside the local_manifest.xml.
         * 3. Parse any local_manifest.xml as well. Improve the sync functionality to
         *    look at all the xml files including the local_manifest.xml file.
         */
        private String getRepoXMLFilePath(String strBasePath)
        {
            String strResult = "";
            if (File.Exists(strBasePath + "\\.repo\\default.xml"))
            {
                strResult = strBasePath + "\\.repo\\default.xml";
            }
            else if (File.Exists(strBasePath + "\\.repo\\manifest.xml"))
            {
                strResult = strBasePath + "\\.repo\\manifest.xml";
            }
            return strResult;
        }

        private void btnRepoSync_Click(object sender, EventArgs e)
        {
            stopPowerShellScript();

            _threadExecutionState = NativeMethods.SetThreadExecutionState(NativeMethods.ES_SYSTEM_REQUIRED | NativeMethods.ES_CONTINUOUS);
            if (0 == _threadExecutionState)
            {
                MessageBox.Show("The system may go to sleep and disconnect from network. Please be aware of long running syncs...",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            String strGitClonePath = getRepoXMLFilePath(txtLocalRepoDirPath.Text);

            AppendLine("Loading Manifest file... ");

            /* Load the manifest, parse it & kör */
            _manifestParser = new ManifestParser();
            _manifestParser.loadManifestFile(strGitClonePath);

            AppendLine("Manifest file & associated projects Loaded... ");
            _isRepoSyncing = true;
            _currentProjectNumber = 0;

            checkoutNextProject(_currentProjectNumber);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            AboutWinREPO aboutFrm = new AboutWinREPO();
            aboutFrm.ShowDialog();
        }
    }

    internal static class NativeMethods
    {
        // Import SetThreadExecutionState Win32 API and necessary flags
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;
    }
}
