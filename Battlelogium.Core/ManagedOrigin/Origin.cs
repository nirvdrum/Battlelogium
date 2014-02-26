﻿using Battlelogium.Core.Utilities;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Drawing;
using Battlelogium.Core.Battlelog;
namespace Battlelogium.Core.ManagedOrigin
{
    /// <summary>
    /// Represents an instance of the Origin process
    /// </summary>
    public class Origin
    {
        string originPath;
        string commandLineOptions;
        bool restartOrigin;

        public Process originProcess;

        public delegate void OriginCloseEvent(object sender, OriginCloseEventArgs e);
        public event OriginCloseEvent OriginClose;
        public Origin(string commandLineOptions)
        {
            this.commandLineOptions = commandLineOptions;
            this.OriginClose += Origin_OriginClose;
        }
        public Origin() : this("/StartClientMinimized") { } //Minimized Origin by default


        #region Origin
        private void Origin_OriginClose(object sender, OriginCloseEventArgs args)
        {
            if (args.restartOrigin) Origin.CreateUnmanagedInstance();
        }

        public void StartOrigin()
        {
            this.originPath = GetOriginPath();
            var originProcessInfo = new ProcessStartInfo(this.originPath, this.commandLineOptions);
            if (OriginRunning())  //We must relaunch Origin as a child process for Steam to properly apply the overlay hook.
            {
                ProcessTools.KillProcess("Origin", true, false);
                this.restartOrigin = true;
            }
            this.originProcess = Process.Start(originProcessInfo);

        }

        public static bool OriginRunning()
        {
            Process[] pname = Process.GetProcessesByName("Origin");
            if (pname.Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string GetOriginPath()
        {
            string originDefaultPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Origin", "Origin.exe");
            string originPath;
            try
            {
                if (Environment.Is64BitOperatingSystem)
                {
                    originPath =
                        Registry.GetValue(
                            @"HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Origin", "ClientPath", originDefaultPath).ToString();

                }
                else
                {
                    originPath =
                        Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Origin", "ClientPath", originDefaultPath).ToString();

                }
                return originPath;
            }
            catch (Exception)
            {
                return originDefaultPath;
            }
        }

        public void KillOrigin()
        {
            OriginClose(this, new OriginCloseEventArgs(this.restartOrigin));
            ProcessTools.KillProcess(this.originProcess, true, false);
            ProcessTools.KillProcess("sonarhost", false, false);
        }

        public void KillOrigin(int timeout)
        {
            Thread.Sleep(timeout);
            this.KillOrigin(); 
            
        }
     
        public static void CreateUnmanagedInstance()
        {
            ProcessTools.CreateOrphanedProcess(GetOriginPath(), "/StartClientMinimized");
        }
        #endregion
    }

    public class OriginCloseEventArgs
    {
        public bool restartOrigin;
        public OriginCloseEventArgs(bool restartOrigin)
        {
            this.restartOrigin = restartOrigin;
        }
    }
}