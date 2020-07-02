using System;
using EnvDTE80;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace SidePanelSwitcher.Helpers
{
    public static class GlobalServices
    {
        // VS Services - not used. Fyi
        public static IServiceProvider ServiceProvider { get; set; }

        public static IComponentModel ComponentModel
            => GetService<SComponentModel, IComponentModel>();

        public static DTE2 DTE => GetService<SDTE, DTE2>();


        //public static OutputWindow OutputWindow => new OutputWindow(ServiceProvider);

        public static IVsSolution Solution => GetService<SVsSolution, IVsSolution>();
        public static IVsSolution2 Solution2 => GetService<SVsSolution, IVsSolution2>();
        public static IVsSolution3 Solution3 => GetService<SVsSolution, IVsSolution3>();
        public static IVsSolution4 Solution4 => GetService<SVsSolution, IVsSolution4>();

        public static IVsSolution5 Solution5 => GetService<SVsSolution, IVsSolution5>();

        //public static IVsSolution6 Solution6 => GetService<SVsSolution, IVsSolution6>();
        public static IVsSolution7 Solution7 => GetService<SVsSolution, IVsSolution7>();
        public static IVsSolution8 Solution8 => GetService<SVsSolution, IVsSolution8>();

        public static IVsShell Shell => GetService<SVsShell, IVsShell>();
        public static IVsShell2 Shell2 => GetService<SVsShell, IVsShell2>();
        public static IVsShell3 Shell3 => GetService<SVsShell, IVsShell3>();
        public static IVsShell4 Shell4 => GetService<SVsShell, IVsShell4>();
        public static IVsShell5 Shell5 => GetService<SVsShell, IVsShell5>();
        public static IVsShell6 Shell6 => GetService<SVsShell, IVsShell6>();
        public static IVsShell7 Shell7 => GetService<SVsShell, IVsShell7>();

        public static IVsUIShell UIShell => GetService<SVsUIShell, IVsUIShell>();
        public static IVsUIShell2 UIShell2 => GetService<SVsUIShell, IVsUIShell2>();
        public static IVsUIShell3 UIShell3 => GetService<SVsUIShell, IVsUIShell3>();
        public static IVsUIShell4 UIShell4 => GetService<SVsUIShell, IVsUIShell4>();
        public static IVsUIShell5 UIShell5 => GetService<SVsUIShell, IVsUIShell5>();
        public static IVsUIShell6 UIShell6 => GetService<SVsUIShell, IVsUIShell6>();
        public static IVsUIShell7 UIShell7 => GetService<SVsUIShell, IVsUIShell7>();

        public static TServiceInterface GetService<TService, TServiceInterface>()
        {
            return (TServiceInterface) Package.GetGlobalService(typeof(TService));
        }

        public static TServiceInterface GetComponentModelService<TServiceInterface>() where TServiceInterface : class
        {
            return ComponentModel.GetService<TServiceInterface>();
        }
    }
}