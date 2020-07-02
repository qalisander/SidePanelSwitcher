using Microsoft.VisualStudio.PlatformUI.Shell;

namespace SidePanelSwitcher.Helpers.Extensions
{
    public static class Extension
    {
        //public static WindowFrame GetActiveWindowFrame(IEnumerable<IVsWindowFrame> frames, DTE2 dte)
        //{
        //    return (from vsWindowFrame in frames
        //            let window = GetWindow(vsWindowFrame)
        //            where window == dte.ActiveWindow
        //            select vsWindowFrame as WindowFrame)
        //        .FirstOrDefault();
        //}

        //public static Window GetWindow(IVsWindowFrame vsWindowFrame)
        //{
        //    object window;
        //    ErrorHandler.ThrowOnFailure(vsWindowFrame.GetProperty((int) __VSFPROPID.VSFPROPID_ExtWindowObject,
        //        out window));

        //    return window as Window;
        //}

        public static int FindSubtreeIndex(this ViewGroup rootElement, ViewElement subtreeElement)
        {
            while (subtreeElement.Parent != rootElement)
                subtreeElement = subtreeElement.Parent;

            return rootElement.Children.IndexOf(subtreeElement);
        }

        public static ViewElement GetAutoHideCenter(this ViewElement element)
        {
            return ((ViewSite) ViewElement.FindRootElement(element)).Find(AutoHideRoot.GetIsAutoHideCenter);
        }
    }
}