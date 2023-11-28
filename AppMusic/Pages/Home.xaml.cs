using System;
using System.Collections.Generic;
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
            songs = new SongItem[] { SongItem1, SongItem2, SongItem3, SongItem4, SongItem5, SongItem6 };
        }
        private void IsPlayingHome(int song)
        {
            for (int i = 0; i < songs.Length; i++)
            {
                if (i == song - 1)
                {
                    songs[i].IsActive = true;
                }
                else
                    songs[i].IsActive = false;
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
    }
}
