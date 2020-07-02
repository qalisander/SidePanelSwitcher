using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Microsoft.VisualStudio.Platform.WindowManagement;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.PlatformUI.Shell;
using Microsoft.VisualStudio.Shell;
using SidePanelSwitcher.Helpers.Extensions;

namespace SidePanelSwitcher.Helpers
{
    public class PanelSwitcher
    {
        private readonly Dock _dockDirection;
        private readonly IServiceProvider _package;

        public PanelSwitcher(IServiceProvider package, Dock dockDirection)
        {
            _package = package;
            _dockDirection = dockDirection;
        }

        public void Switch()
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            try
            {
                var toolWindowFrames = _package
                    .GetToolWindowFrames()
                    .Select(fr => fr as WindowFrame);

                ApplySwitch(
                    toolWindowFrames.Where(IsDocked),
                    toolWindowFrames.Where(IsBookmarked));
            }
            catch (Exception e)
            {
                VsShellUtilities.LogError(e.Source, e.ToString());
#if DEBUG
                throw;
#endif
            }
        }

        private static void ApplySwitch(IEnumerable<WindowFrame> docked, IEnumerable<WindowFrame> bookmarked)
        {
            if (bookmarked.Any())
                bookmarked.DockBookmarks();
            else if (docked.Any())
                docked.AutoHideDockPanel();
        }

        private bool IsDocked(WindowFrame frame)
        {
            if (!(frame.FrameView is ViewElement viewElement))
                return false;

            //verifying whether there is viewElement's ancestor type of DockRoot
            if (!DockOperations.CanAutoHide(viewElement))
                return false;

            if (OrientationOfDocked(viewElement) == _dockDirection)
                return true;

            return false;
        }

        private bool IsBookmarked(WindowFrame frame)
        {
            if (!(frame.FrameView is ViewElement viewElement))
                return false;

            //verifying whether there is viewElement's ancestor type of AutoHideChannel
            if (!AutoHideChannel.IsAutoHidden(viewElement))
                return false;

            if (OrientationOfAutoHidden(viewElement) == _dockDirection)
                return true;

            return false;
        }

        private static Dock OrientationOfDocked(ViewElement viewElement)
        {
            var autoHideCenter = viewElement.GetAutoHideCenter();
            var commonAncestor = viewElement.FindCommonAncestor(autoHideCenter, el => el.Parent);

            if (commonAncestor is null)
                throw new ArgumentNullException(nameof(commonAncestor));

            if (!(commonAncestor is DockGroup dockGroup))
                throw new InvalidOperationException();

            var subtreeIndex1 = dockGroup.FindSubtreeIndex(autoHideCenter);
            var subtreeIndex2 = dockGroup.FindSubtreeIndex(viewElement);
            var dock = dockGroup.Orientation != Orientation.Horizontal
                ? subtreeIndex2 >= subtreeIndex1 ? Dock.Bottom : Dock.Top
                : subtreeIndex2 >= subtreeIndex1 ? Dock.Right : Dock.Left;

            return dock;
        }

        private static Dock OrientationOfAutoHidden(ViewElement viewElement)
        {
            return viewElement.FindAncestor<AutoHideChannel>().Dock;
        }
    }
}