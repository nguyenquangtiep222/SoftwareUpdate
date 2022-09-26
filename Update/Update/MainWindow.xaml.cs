using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Update
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Properties.Settings _param = Properties.Settings.Default;

        #region Binding Property
        public bool AutoRun
        {
            get => _param.AutoRun;
            set => _param.AutoRun = value;
        }
        public string AppName
        {
            get => _param.AppName;
            set => _param.AppName = value;
        }
        public string VersionFile
        {
            get => _param.VersionFile;
            set => _param.VersionFile = value;
        }
        public string UpdateFile
        {
            get => _param.UpdateFile;
            set => _param.UpdateFile = value;
        }

        // Declare event
        public event PropertyChangedEventHandler PropertyChanged;
        // OnPropertyChanged method to update property value in binding
        private void OnPropertyChanged(string info = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartupApp();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            ShutdownApp();
        }

        private void btnPrimaryButton_Click(object sender, RoutedEventArgs e)
        {
            _param.Save();
            this.Close();
        }

        private void btnSecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void StartupApp()
        {
            try
            {
                this.Title = "Update";
                Console.WriteLine("Update =====> Mở ứng dụng");
                if (_param.AutoRun)
                {
                    var processes = Process.GetProcessesByName(_param.AppName.Replace(".exe", ""));
                    if (processes.Length > 0)
                    {
                        foreach (var process in processes)
                        {
                            process.Kill();
                        }
                    }

                    var filename = $"{AppDomain.CurrentDomain.BaseDirectory}{_param.UpdateFile}";
                    if (File.Exists(filename))
                    {
                        ExtractFile(filename, AppDomain.CurrentDomain.BaseDirectory);
                    }

                    if (File.Exists(_param.VersionFile))
                    {
                        File.Delete(_param.VersionFile);
                    }
                    if (File.Exists(_param.UpdateFile))
                    {
                        File.Delete(_param.UpdateFile);
                    }

                    if (File.Exists(_param.AppName))
                    {
                        Process.Start(_param.AppName, AppDomain.CurrentDomain.BaseDirectory);
                        ShutdownApp();
                    }
                }
                else
                {
                    MessageBox.Show("Chưa cài đặt tự động chạy.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK, MessageBoxOptions.DefaultDesktopOnly);
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
                Console.WriteLine("Update =====> Tắt ứng dụng");
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
