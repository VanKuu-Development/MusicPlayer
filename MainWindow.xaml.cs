using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MediaToolkit;
using MediaToolkit.Model;
using VideoLibrary;



namespace MusicPlayer
{
    
    public partial class MainWindow : Window
    {
        bool isMp3 = false;
        bool isMp4 = false;
        string source = @"C:\RepoMainPC\MusicPlayer\bin\Debug\net6.0-windows\Converted\";
        string destination;
        MediaFile inputFile;
        MediaFile outputFile;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void Mp4Btn_Click(object sender, RoutedEventArgs e)
        {
            isMp4 = true;
            ConvertAndSave(LinkText.Text);
        }
        private void Mp3Btn_Click(object sender, RoutedEventArgs e)
        {
            isMp3 = true;
            ConvertAndSave(LinkText.Text);
        }
        void ConvertAndSave(string url)
        {
            var youtube = YouTube.Default;
            var video = youtube.GetVideo(url);
            progressBar.Value = 0;

            try
            {
                //File.WriteAllBytes(source + video.FullName, video.GetBytes());
                byte[] videoBytes = video.GetBytes();
                using(var writer = new BinaryWriter(File.Open(source + video.FullName, FileMode.Create)))
                {
                    progressBar.Maximum = videoBytes.Length;
                   
                    var bytesLeft = videoBytes.Length;
                    var bytesWritten = 0;
                    while(bytesLeft > 0)
                    {
                        int chunk = Math.Min(64, bytesLeft);
                        writer.Write(videoBytes, bytesWritten, chunk);
                        bytesWritten += chunk;
                        bytesLeft -= chunk;
                        progressBar.Value += chunk;
                        
                    }

                }

                if (isMp4)
                {
                    destination = @"C:\RepoMainPC\MusicPlayer\bin\Debug\net6.0-windows\MP4\";
                    inputFile = new MediaFile { Filename = source + video.FullName };
                    outputFile = new MediaFile { Filename = $"{destination + video.FullName}.mp4" };
                }
                if (isMp3)
                {
                    destination = @"C:\RepoMainPC\MusicPlayer\bin\Debug\net6.0-windows\MP3\";
                    inputFile = new MediaFile { Filename = source + video.FullName };
                    outputFile = new MediaFile { Filename = $"{destination + video.FullName}.mp3" };
                }

                if (outputFile != null & inputFile != null)
                {
                    using (var engine = new Engine())
                    {
                        engine.GetMetadata(inputFile);

                        engine.Convert(inputFile, outputFile);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
          
            


        }

        
    }
}
