//------------------------------------------------------------------------------
// <copyright file="JikamePackage.cs" company="Microsoft">
//     Copyright (c) Microsoft.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Jikame {
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0.2017.0117", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [ProvideOptionPage(typeof(JikameOption), "Jikame", "General", 0, 0, true)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [ProvideAutoLoad(UIContextGuids80.SolutionExists)]
    [ProvideAutoLoad(UIContextGuids80.EmptySolution)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class JikamePackage : Package {
        /// <summary>
        /// JikamePackage GUID string.
        /// </summary>
        public const string PackageGuidString = "497f856e-7fdc-4c28-b86a-c081c050bacb";
        
        private DTEEvents events;
        private JikameOption option;
        private JikamePanel jikame;
        private Timer timer;

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize() {
            base.Initialize();

            var dte = (DTE) GetService(typeof(DTE));
            events = dte.Events.DTEEvents;
            events.OnStartupComplete += OnStartupComplete;
            events.OnBeginShutdown += OnBeginShutdown;
        }

        private void OnBeginShutdown() {
            timer.Stop();
        }

        private void OnStartupComplete() {
            timer = new Timer(1000);
            timer.Elapsed += TimerOnElapsed;

            option = GetDialogPage(typeof(JikameOption)) as JikameOption;
            jikame = new JikamePanel(option?.Format);
            ForcefullyPlaceIntoStatusBar(jikame);

            timer.Enabled = true;
        }

        private void TimerOnElapsed(object sender, ElapsedEventArgs e) {
            jikame.Dispatcher.Invoke(() => jikame.Time = DateTime.Now);
        }

        private void ForcefullyPlaceIntoStatusBar(FrameworkElement toBePlaced) {
            var vsWindow = Application.Current.MainWindow;
            var statusBar = vsWindow.Find("StatusBarPanel") as DockPanel;
            var found = statusBar.Find(toBePlaced.Name) as UIElement;
            if (found != null) statusBar?.Children.Remove(found);
            statusBar?.Dispatcher.Invoke(() => {
                toBePlaced.SetValue(DockPanel.DockProperty, Dock.Right);
                statusBar.Children.Insert(1, toBePlaced);
            });
        }

        public void UpdateFormatInstantly(String format) {
            jikame.Format = format;
        }
    }
}