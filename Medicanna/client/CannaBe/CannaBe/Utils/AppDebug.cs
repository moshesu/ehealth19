using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.Storage;
using Windows.UI.Xaml;

namespace CannaBe
{
    static class AppDebug
    {
        private static StorageFile LogFile = null;
        public static bool IsRunningOnPc = true;

        public static void Line(object msg, bool OmitDate, string caller)
        { // Print to output console
            if (!OmitDate)
                msg = DateTime.Now.ToString("HH:mm:ss.ffffff") + " " + msg;

            Debug.WriteLine(msg);

            try
            { // Write to log file
                if (LogFile == null)
                {
                    Init();
                }

                FileIO.AppendTextAsync(LogFile, msg.ToString() + Environment.NewLine).GetAwaiter().GetResult();
            }
            catch (Exception exc)
            {
                Line($"!!! *** Exception caught in [{caller} => AppDebug.Line] *** !!!");
                Debug.WriteLine(exc);
            }
        }

        public static void Init()
        { // Initiate log file
            Application.Current.UnhandledException += UnhandledExceptionEventHandler;
            EasClientDeviceInformation info = new EasClientDeviceInformation();
            string platform = "";
            if(info.OperatingSystem == "WindowsPhone")
            {
                platform = "Phone";
                IsRunningOnPc = false;
            }
            else if(info.OperatingSystem == "WINDOWS")
            {
                platform = "PC";
                IsRunningOnPc = true;
            }

            if (LogFile == null)
            {
                Task.Run(() =>
                {
                    StorageFolder storageFolder = KnownFolders.DocumentsLibrary;
                    LogFile = storageFolder.CreateFileAsync("CannaBeLogFile.txt", CreationCollisionOption.OpenIfExists).GetAwaiter().GetResult();
                    var log = $"*** Start New Log Session at {DateTime.Now.ToString("dd/MM/yy HH:mm:ss.ffffff")} / {platform} ***";
                    var log2 = $"Log file saved in {LogFile.Path}";
                    FileIO.AppendTextAsync(LogFile, log + Environment.NewLine).GetAwaiter().GetResult();
                    Debug.WriteLine(log);
                    Debug.WriteLine(log2);
                });
            }
        }

        public static void Line(object msg, [CallerMemberName] string functionCaller = "")
        {
            Line(msg, false, functionCaller);
        }

        public static void Exception(Exception e, string caller,
            [CallerLineNumber] int lineNumber = 0, [CallerFilePath] string filename = "")
        {
            Line($"!!! *** Exception caught in [{caller}] *** !!!");
            try { Line($"File: [{Path.GetFileName(filename)}:{lineNumber}]"); } catch { }
            Line(e);
        }

        private static void UnhandledExceptionEventHandler(object sender, UnhandledExceptionEventArgs e)
        {
            Line($"!!! *** UNHANDLED Exception *** !!!");
            Line(e.Exception);
        }
    }
}
