using System;
using System.Globalization;
using Microsoft;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SidePanelSwitcher.Helpers
{
    //TODO: To delete
    /// <summary>
    ///     Logging into VS activity log
    /// </summary>
    public static class Logger
    {
        private static AsyncPackage _package;

        private static IServiceProvider ServiceProvider => _package;

        public static void Initialize(AsyncPackage serviceProvider)
        {
            _package = serviceProvider;
        }

        // NOTE: https://docs.microsoft.com/en-us/visualstudio/extensibility/how-to-use-the-activity-log?view=vs-2019
        public static void Log(string message, __ACTIVITYLOG_ENTRYTYPE entryType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var log = ServiceProvider.GetService(typeof(SVsActivityLog)) as IVsActivityLog;
            Assumes.Present(log);

            // ReSharper disable once PossibleNullReferenceException
            log.LogEntry((uint) entryType, _package.ToString(), message);
        }

        public static void Log(Exception e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var log = ServiceProvider.GetService(typeof(SVsActivityLog)) as IVsActivityLog;
            Assumes.Present(log);

            var fullOutput = string.Format(
                CultureInfo.CurrentCulture,
                "Exception Message: {0} \n Stack Trace: {1}",
                e.Message,
                e.StackTrace);

            // ReSharper disable once PossibleNullReferenceException
            log.LogEntry((uint) __ACTIVITYLOG_ENTRYTYPE.ALE_ERROR, _package.ToString(), fullOutput);
        }

        public static void MessageBox(string title, string message, __ACTIVITYLOG_ENTRYTYPE entryType)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var icon = OLEMSGICON.OLEMSGICON_INFO;
            if (entryType == __ACTIVITYLOG_ENTRYTYPE.ALE_ERROR)
                icon = OLEMSGICON.OLEMSGICON_CRITICAL;
            else if (entryType == __ACTIVITYLOG_ENTRYTYPE.ALE_WARNING)
                icon = OLEMSGICON.OLEMSGICON_WARNING;

            var uiShell = ServiceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            Assumes.Present(uiShell);

            var clsid = Guid.Empty;
            // ReSharper disable once PossibleNullReferenceException
            uiShell.ShowMessageBox(
                0,
                ref clsid,
                title,
                message,
                string.Empty,
                0,
                OLEMSGBUTTON.OLEMSGBUTTON_OK,
                OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST,
                icon,
                0,
                out _);
        }
    }
}