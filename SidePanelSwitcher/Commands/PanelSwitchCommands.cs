using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Controls;
using Microsoft.VisualStudio.Shell;
using SidePanelSwitcher.Helpers;
using Task = System.Threading.Tasks.Task;

namespace SidePanelSwitcher.Commands
{
    /// <summary>
    ///     Command handler
    /// </summary>
    public sealed class PanelSwitchCommands
    {
        /// <summary>
        ///     VS Package that provides this command, not null.
        /// </summary>
        private static AsyncPackage _package;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PanelSwitchCommands" /> class.
        ///     Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner provider, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private PanelSwitchCommands(AsyncPackage package, OleMenuCommandService commandService)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            RegisterCommands(ServiceProvider, commandService);
        }

        /// <summary>
        ///     Gets the service provider from the owner provider.
        /// </summary>
        private static IAsyncServiceProvider AsyncServiceProvider => _package;

        /// <summary>
        ///     Gets the service provider from the owner provider.
        /// </summary>
        private static IServiceProvider ServiceProvider => _package;

        /// <summary>
        ///     Gets the instance of the command.
        /// </summary>
        public static PanelSwitchCommands Instance { get; private set; }

        private static void RegisterCommands(IServiceProvider package, IMenuCommandService commandService)
        {
            foreach (var command in CreateCommands())
                commandService.AddCommand(command.MenuCommand);
        }

        private static IEnumerable<CommandWrapper> CreateCommands()
        {
            yield return new CommandWrapper(PackageIds.cmdidBottomPanelSwitchCommand, Dock.Bottom);
            yield return new CommandWrapper(PackageIds.cmdidRightPanelSwitchCommand, Dock.Right);
            yield return new CommandWrapper(PackageIds.cmdidTopPanelSwitchCommand, Dock.Top);
            yield return new CommandWrapper(PackageIds.cmdidLeftPanelSwitchCommand, Dock.Left);
        }

        /// <summary>
        ///     Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner provider, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in PanelSwitchCommands's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            _package = package;

            var commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new PanelSwitchCommands(package, commandService);
        }

        private class CommandWrapper
        {
            private readonly PanelSwitcher _panelSwitcher;

            internal CommandWrapper(int commandId, Dock dock)
            {
                _panelSwitcher = new PanelSwitcher(ServiceProvider, dock);

                var bottomCommandId = new CommandID(PackageGuids.guidSidePanelSwitcherPackageCmdSet, commandId);
                MenuCommand = new MenuCommand(ExecuteCommand, bottomCommandId);
            }

            internal MenuCommand MenuCommand { get; }

            private void ExecuteCommand(object sender, EventArgs e)
            {
                _panelSwitcher.Switch();
            }
        }
    }
}