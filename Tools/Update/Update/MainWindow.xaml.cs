using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Update
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closed(object sender, System.EventArgs e)
        {

        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            var directLink = "https://dl.dropboxusercontent.com/s/evb3atcaba7ke7d/Update.7z?dl=0";
            var localPath = @"C:\Users\nguyenquangtiep\Desktop\Update.7z";
            //var directLink = "https://people.sc.fsu.edu/~jburkardt/data/csv/addresses.csv";
            //var localPath = @"C:\Users\nguyenquangtiep\Desktop\addresses.csv";
            DownloadFile(directLink, localPath);
        }

        public void DownloadFile(string directLink, string localPath)
        {
            Task.Run(() =>
            {
                try
                {
                    Console.WriteLine("Downloading...");
                    using (var client = new WebClient())
                    {
                        client.DownloadProgressChanged += Client_DownloadProgressChanged;
                        client.DownloadFileCompleted += Client_DownloadFileCompleted;
                        var buffer = client.DownloadData(directLink);
                        using (var stream = new FileStream(localPath, FileMode.Create))
                        {
                            var bw = new BinaryWriter(stream);
                            bw.Write(buffer);

                            //// Read text
                            //var bytes = new byte[stream.Length];
                            //stream.Read(bytes, 0, (int)stream.Length);
                            //var data = System.Text.Encoding.ASCII.GetString(bytes);
                        }
                    }
                    Console.WriteLine("Download completed!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });
        }

        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {

        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {

        }
    }
}
