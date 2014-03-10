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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Windows.Forms;

namespace WinREPO
{
    public class RemoteServerConfigs
    {
        /* We will only take the fetch and the names so we can keep track of stuff */
        public String _strRemoteFetch;
        public String _strRemoteName;
    }

    public class ProjectPathConfigs
    {
        /* This is the class which has all the project details which will be individually fetched by git */
        public String _strPath;
        public String _strName;
        public String _strBranch;
        public String _strRemoteName;
        public String _strRevision;
    }

    public class ManifestConfigs
    {
        public RemoteServerConfigs[] _remoteServerConfigs;
        public ProjectPathConfigs[] _projectPathConfigs;
        /* Optional revision for the default remote */
        public String _strDefaultRevision;
        public String _strDefaultRemote;
        /* Optional number of threads to be used for Sync */
        public int _intSyncThreads;
    }
    class ManifestParser
    {
        public ManifestConfigs _manifestConfig;
        private XDocument _xmlManifest;

        private const String _strManifest = "manifest";
        private const String _strRemote = "remote";
        private const String _strDefault = "default";
        private const String _strproject = "project";

        private int getCountOfNodes(String strNodeNames)
        {
            XmlDocument tempDoc = new XmlDocument();
            tempDoc.Load(_xmlManifest.CreateReader());
            return tempDoc.SelectNodes(strNodeNames).Count;
        }

        private void parseProjectConfigs()
        {
            int iCountOfProjectConfigs = getCountOfNodes(_strManifest + "/" + _strproject);
            Console.WriteLine("No Of ProjectConfigs = " + iCountOfProjectConfigs);

            _manifestConfig._projectPathConfigs = new ProjectPathConfigs[iCountOfProjectConfigs];
            int count = 0;

            var nodes = _xmlManifest.Element(_strManifest).Elements(_strproject);
            foreach (XElement item in nodes)
            {
                _manifestConfig._projectPathConfigs[count] = new ProjectPathConfigs();

                _manifestConfig._projectPathConfigs[count]._strPath = item.Attribute("path").Value;
                _manifestConfig._projectPathConfigs[count]._strName = item.Attribute("name").Value;

                try
                {
                    _manifestConfig._projectPathConfigs[count]._strRemoteName = item.Attribute("remote").Value;
                }
                catch(Exception) 
                {
                    _manifestConfig._projectPathConfigs[count]._strRemoteName = null;
                }
                try
                {
                    _manifestConfig._projectPathConfigs[count]._strBranch = item.Attribute("branch").Value;
                }
                catch(Exception) 
                {
                    _manifestConfig._projectPathConfigs[count]._strBranch = null;
                }
                try 
                {
                    _manifestConfig._projectPathConfigs[count]._strRevision = item.Attribute("revision").Value;
                }
                catch (Exception)
                {
                    _manifestConfig._projectPathConfigs[count]._strRevision = null;
                }

                count++;
            }
        }

        private void parseRemoteServerConfigs()
        {
            int iCountOfServerConfigs = getCountOfNodes(_strManifest + "/" + _strRemote);
            Console.WriteLine("No Of ServerConfigs = " + iCountOfServerConfigs);

            var nodes = _xmlManifest.Element(_strManifest).Elements(_strRemote);
            _manifestConfig._remoteServerConfigs = new RemoteServerConfigs[iCountOfServerConfigs];
            int count = 0;

            foreach (XElement item in nodes)
            {
                _manifestConfig._remoteServerConfigs[count] = new RemoteServerConfigs();

                _manifestConfig._remoteServerConfigs[count]._strRemoteFetch = item.Attribute("fetch").Value;
                _manifestConfig._remoteServerConfigs[count]._strRemoteName = item.Attribute("name").Value;
                count++;
            }
        }

        private void parseDefaultConfig()
        {
            var node = _xmlManifest.Element(_strManifest).Element(_strDefault);
            _manifestConfig._strDefaultRevision = node.Attribute("revision").Value;
            _manifestConfig._strDefaultRemote = node.Attribute("remote").Value;
            try
            {
                _manifestConfig._intSyncThreads = Convert.ToInt16(node.Attribute("sync-j").Value);
            }
            catch (Exception)
            {
                Console.WriteLine("No sync-j specified! We are NOT USING it anyways. SKIP!");
                _manifestConfig._intSyncThreads = 0;
            }            
        }
        public void loadManifestFile(String strManifestName)
        {
            _manifestConfig = new ManifestConfigs();
            _xmlManifest = XDocument.Load(strManifestName);
            parseRemoteServerConfigs();
            parseDefaultConfig();
            parseProjectConfigs();
            Console.WriteLine("Loading Manifests DONE!");
        }

        public int getServerConfigForProject(int iProjectConfig)
        {
            if (iProjectConfig > _manifestConfig._projectPathConfigs.Length)
            {
                Console.WriteLine("Wrong array enum value for Project Path Configurations");
                return -1;
            }
            for (int i = 0; i < _manifestConfig._remoteServerConfigs.Length; i++)
            {
                if (_manifestConfig._projectPathConfigs[iProjectConfig]._strRemoteName == null)
                {
                    if (_manifestConfig._strDefaultRemote == _manifestConfig._remoteServerConfigs[i]._strRemoteName)
                    {
                        return i;
                    }
                }
                else if (_manifestConfig._projectPathConfigs[iProjectConfig]._strRemoteName == _manifestConfig._remoteServerConfigs[i]._strRemoteName)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
