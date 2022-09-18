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

        private void Window_Closed(object sender, System.EventArgs e)
        {
            ShutdownApp();
        }

        private void StartupApp()
        {
            try
            {
                // Kill Store.exe
                var processes = Process.GetProcessesByName("Store");
                if (processes.Length > 0)
                {
                    foreach (var process in processes)
                        process.Kill();
                }

                // Unzip
                var filename = $"{AppDomain.CurrentDomain.BaseDirectory}{_param.UpdateFile}";
                ExtractFile(filename, AppDomain.CurrentDomain.BaseDirectory);

                // Delete files
                if (File.Exists(_param.VersionFile))
                    File.Delete(_param.VersionFile);
                if (File.Exists(_param.UpdateFile))
                    File.Delete(_param.UpdateFile);

                // Shutdown app
                Application.Current.Shutdown();
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
        }

        public void ExtractFile(string source, string destination)
        {
            string zPath = @"C:\Program Files\7-Zip\7zG.exe";
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = zPath,
                    Arguments = "x \"" + source + "\" -o" + destination
                };
                Process process = Process.Start(processInfo);
                process.WaitForExit();
            }
            catch
            {

            }
        }
    }
}
