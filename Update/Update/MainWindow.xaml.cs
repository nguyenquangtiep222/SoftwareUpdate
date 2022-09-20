using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Update
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Properties.Settings _param = Properties.Settings.Default;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title = "Update";
            StartupApp();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ShutdownApp();
        }

        private void StartupApp()
        {
            try
            {
                Console.WriteLine("Update =====> Startup application");

                var processes = Process.GetProcessesByName(_param.AppName);
                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                        process.Kill();
                }

                var filename = $"{AppDomain.CurrentDomain.BaseDirectory}{_param.UpdateFile}";
                if (File.Exists(filename))
                    ExtractFile(filename, AppDomain.CurrentDomain.BaseDirectory);

                if (File.Exists(_param.VersionFile))
                    File.Delete(_param.VersionFile);
                if (File.Exists(_param.UpdateFile))
                    File.Delete(_param.UpdateFile);

                Process.Start(_param.AppName);

                ShutdownApp();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShutdownApp()
        {
            try
            {

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Update =====> Shutdown application");
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
        }

        public void ExtractFile(string source, string destination)
        {
            string zPath = @"C:\Program Files\7-Zip\7z.exe";
            var processInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = zPath,
                Arguments = $"x \"{source}\" -aoa -o\"{destination}\""
            };
            var process = Process.Start(processInfo);
            process?.WaitForExit();
            process?.Dispose();
        }
    }
}
