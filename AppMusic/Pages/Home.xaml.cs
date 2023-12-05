using System;
using System.Collections.Generic;
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
using AppMusic;
using NAudio.Wave;
using Microsoft.Win32;
using TagLib;
using static TagLib.File;

namespace AppMusic.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();
        }

        private void SongItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MediaPlayerManager.filePath = @"C:\Users\Laptop MSI\Downloads\ALoi.mp3";
            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
        }

        private void btnLoadPlaylist_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn một tệp tin";
            openFileDialog.Filter = "File MP3|*.mp3";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var selectedFilePath = (dynamic)openFileDialog.FileName;
                LoadMusicFiles(selectedFilePath);
            }
        }

        private void LoadMusicFiles(string filePath)
        {
            try
            {
                Mp3FileReader mp3FileReader = new Mp3FileReader(filePath);

                //playlistListBox.Items.Add(new Playlist(1, "ten bai hat",filePath, mp3FileReader.TotalTime));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void btnDeletePlaylist_Click(object sender, RoutedEventArgs e)
        {

        }

        private void playlistListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
