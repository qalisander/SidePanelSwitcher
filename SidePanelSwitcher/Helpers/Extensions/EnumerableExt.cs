using System.Collections.Generic;
using System.Windows.Controls;
using Microsoft.VisualStudio.Platform.WindowManagement;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SidePanelSwitcher.Helpers.Extensions
{
    public static class EnumerableExt
    {
        public static void DockBookmarks(this IEnumerable<WindowFrame> bookmarkedFrames)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var frame in bookmarkedFrames)
            {
                frame.GetProperty((int) __VSFPROPID.VSFPROPID_FrameMode, out var frameMode);

                // We should make sure that bookmarks were not docked before
                if ((VsFrameMode) frameMode != VsFrameMode.AutoHide)
                    continue;

                frame.SetProperty((int) __VSFPROPID.VSFPROPID_FrameMode, VsFrameMode.Dock);
            }
        }

        public static void AutoHideDockPanel(this IEnumerable<WindowFrame> bookmarkedFrames)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            foreach (var frame in bookmarkedFrames)
            {
                frame.GetProperty((int) __VSFPROPID.VSFPROPID_FrameMode, out var frameMode);

                // We should make sure that bookmarks were not auto hidden before
                if ((VsFrameMode) frameMode != VsFrameMode.Dock)
                    continue;

                frame.SetProperty((int) __VSFPROPID.VSFPROPID_FrameMode, VsFrameMode.AutoHide);
            }
        }
    }
}