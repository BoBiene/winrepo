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
        private frmOptions _frmOptions = new frmOptions();
        private ManifestParser _manifestParser;
        private Runspace _powerShellRunSpace;
        private PipelineExecutor _pipelineExecutor;

        private event EventHandler ProjectCheckoutDone;

        private int _currentProjectNumber = 0;
        private Boolean _isRepoSyncing = false;
        private Boolean _movDirectoryToRightLocation = false;
        private const String _tempCheckoutPath = "C:\\tmpRepo";

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

            _frmOptions.readRegistryKeysIfAny();
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

        private void startPowerShellScript(String strScript)
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
            if (listBoxOutput.Items.Count > 1000) listBoxOutput.Items.RemoveAt(0);
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

        private const String _strSetExecutionPolicy = "set-executionpolicy remotesigned\r\n";
        private const String _strNewline = "\r\n";
        private const String _strStreamRedirect = " | Out-String -stream";
        private const String _strHideRepoDir = "attrib +h .repo";
        private String _strRootDir = "";
        private const String _strGitProgress = " --progress";

        private void setRootDirectory()
        {
            String strGitClonePath = txtRepoURL.Text;
            if (txtRepoURL.Text.Contains("tags"))
            {
                strGitClonePath = strGitClonePath.Substring(0, txtRepoURL.Text.IndexOf("-b"));
            }

            _strRootDir = strGitClonePath.Substring(strGitClonePath.LastIndexOf("/") + 1);
        }

        private Boolean checkIfRepoExists(String strPath)
        {
            Boolean result = false;
            return result;
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

            String strCommand = _frmOptions._strGitHubFileName + _strNewline +
                "cd " + txtLocalRepoDirPath.Text + _strNewline + "git clone " +
                strGitClonePath + _strGitProgress + _strNewline + "cd " + _strRootDir +
                _strNewline + strGitTag + _strNewline + "cd .." + _strNewline +
                "mv " + _strRootDir + " .repo" + _strNewline +_strHideRepoDir + _strNewline;
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

        private const int MAX_PATH = 200;
        private Boolean isPathExceedingMaxPath(String strPath)
        {
            if (strPath.Length > MAX_PATH)
            {
                return true;
            }
            return false;
        }

        private static Boolean firstTime = true;

        private void checkoutNextProject(int iProjectNumber)
        {
            stopPowerShellScript();
            String strCommand = "";
            if (firstTime)
            {
                strCommand = _frmOptions._strGitHubFileName + _strNewline;
                firstTime = false;
            }

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
                    _tempCheckoutPath + " -b " +
                    ((_manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch == null) ?
                    _manifestParser._manifestConfig._strDefaultRevision : _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch) +
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
                    _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strPath + " -b " +
                    ((_manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch == null) ?
                    _manifestParser._manifestConfig._strDefaultRevision : _manifestParser._manifestConfig._projectPathConfigs[_currentProjectNumber]._strBranch) +
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
}
