﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using GamepadMapper.Configuration;
using GamepadMapper.Configuration.Parsing;
using GamepadMapper.Infrastructure;
using Ninject;

namespace GamepadMapper.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        private static string[] appArgs;
        private static IKernel kernel;
        private static CancellationTokenSource cancellation;
        private static Task loopTask;
        private static readonly ILogger Logger = new FileLogger();

        [STAThread]
        public static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Logger.WriteLine("=== Critical failure ===");
                Logger.WriteLine(e.ExceptionObject.ToString());
            };

            appArgs = args ?? new string[0];
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        private void OnAppStartup(object sender, StartupEventArgs e)
        {
            var configPath = "config.txt";
            if (appArgs.Length > 1)
            {
                configPath = appArgs[0];
            }

            RootConfiguration config;
            try
            {
                config = Parser.Parse(configPath, Logger);
            }
            catch
            {
                MessageBox.Show(
                    "Could not initialize configuration. See log file for details.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Current?.Shutdown(1);
                return;
            }

            kernel = new StandardKernel(new ApplicationModule(config));
            var mainLoop = kernel.Get<ApplicationLoop>();
            var window = new MainWindow(mainLoop.MenuController);
            window.Show();
            cancellation = new CancellationTokenSource();
            loopTask = Task.Run(() => mainLoop.Run(cancellation.Token));
        }

        private void OnAppExit(object sender, ExitEventArgs e)
        {
            cancellation?.Cancel();
            try
            {
                loopTask?.Wait(1000);
            }
            catch (AggregateException ex)
            when (ex.InnerExceptions.FirstOrDefault() is TaskCanceledException)
            {
            }

            kernel?.Dispose();
        }
    }
}
