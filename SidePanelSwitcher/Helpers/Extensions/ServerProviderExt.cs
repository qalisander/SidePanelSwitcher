using System;
using System.Collections.Generic;
using Microsoft;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SidePanelSwitcher.Helpers.Extensions
{
    public static class ServerProviderExt
    {
        public static IEnumerable<IVsWindowFrame> GetVsWindowFrames(this IServiceProvider serviceProvider)
        {
            //NOTE: https://www.pmichaels.net/tag/throwifnotonuithread/
            ThreadHelper.ThrowIfNotOnUIThread();

            //NOTE: https://docs.microsoft.com/en-us/dotnet/api/microsoft.assumes?view=visualstudiosdk-2019
            var uiShell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            Assumes.Present(uiShell);

            // ReSharper disable once PossibleNullReferenceException
            ErrorHandler.ThrowOnFailure(uiShell.GetDocumentWindowEnum(out var windowEnumerator));

            if (windowEnumerator.Reset() != VSConstants.S_OK)
                yield break;

            var frames = new IVsWindowFrame[1];
            bool hasMoreWindows;
            do
            {
                hasMoreWindows = windowEnumerator.Next(1, frames, out var fetched) == VSConstants.S_OK && fetched == 1;

                if (!hasMoreWindows || frames[0] == null)
                    continue;

                yield return frames[0];
            } 
            while (hasMoreWindows);
        }

        public static IEnumerable<IVsWindowFrame> GetToolWindowFrames(this IServiceProvider serviceProvider)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var uiShell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;
            Assumes.Present(uiShell);

            // ReSharper disable once PossibleNullReferenceException
            ErrorHandler.ThrowOnFailure(uiShell.GetToolWindowEnum(out var windowEnumerator));

            if (windowEnumerator.Reset() != VSConstants.S_OK)
                yield break;

            var frames = new IVsWindowFrame[1];
            bool hasMoreWindows;
            do
            {
                hasMoreWindows = windowEnumerator.Next(1, frames, out var fetched) == VSConstants.S_OK && fetched == 1;

                if (!hasMoreWindows || frames[0] == null)
                    continue;

                yield return frames[0];
            } 
            while (hasMoreWindows);
        }
    }
}