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
using AppMusic.UserControls;

namespace AppMusic.Pages
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        private SongItem[] songs;
        public Home()
        {
            InitializeComponent();
            //songs = new SongItem[] { SongItem1, SongItem2, SongItem3, SongItem4, SongItem5, SongItem6 };
        }

        private void SongItem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        
        }
        
        private void IsPlayingHome(int song)
        {
            for (int i = 0; i < songs.Length; i++)
            {
                //if (i == song - 1)
                //{
                //    songs[i].IsActive = true;
                //}
                //else
                //    songs[i].IsActive = false;
            }
            MediaPlayerManager.MediaPlayer.Stop();
        }
        private void SongItem1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPlayingHome(1);
            MediaPlayerManager.filePath = "Music/ALoi.mp3";
            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
        }

        private void SongItem2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPlayingHome(2);
            MediaPlayerManager.filePath = "Music/ThanhAmMienNui.mp3";
            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);

        }

        private void SongItem3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPlayingHome(3);
            MediaPlayerManager.filePath = "Music/NguoiMienNuiChat.mp3";
            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);

        }

        private void SongItem4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPlayingHome(4);
            MediaPlayerManager.filePath = "Music/ChaiDiepNoong.mp3";
            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);

        }

        private void SongItem5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPlayingHome(5);
            MediaPlayerManager.filePath = "Music/TayLaiPr0.mp3";
            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);

        }

        private void SongItem6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPlayingHome(6);
            MediaPlayerManager.filePath = "Music/KeoEmVeLamVo.mp3";
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
