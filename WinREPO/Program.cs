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
using System.Windows.Forms;

namespace WinREPO
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
