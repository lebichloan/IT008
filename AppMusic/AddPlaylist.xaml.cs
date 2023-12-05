using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AppMusic
{
    /// <summary>
    /// Interaction logic for AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        public AddPlaylist()
        {
            InitializeComponent();
            SetValues();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SetValues()
        {
            txtPlaylistName.Text = string.Empty;
            txtTotalSong.Text = "0";
            listSong.Items.Clear();
        }

        static string FomatTimeSpan(TimeSpan time)
        {
            return time.Hours == 0
                ? $"{time.Minutes:D2}:{time.Seconds:D2}"
                : $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        private void btnAddSong_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Chọn một tệp tin";
            openFileDialog.Filter = "File MP3|*.mp3";

            bool? result = openFileDialog.ShowDialog();
            if (result == true)
            {
                var selectedFilePath = (dynamic)openFileDialog.FileName;
                AddSong addSong = new AddSong(selectedFilePath);
                addSong.DataReturned += addSong_DataReturned;
                addSong.ShowDialog();
            }
            else
            {
                MessageBox.Show("Vui long chon file", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<SONG> listSongPlaylist = new List<SONG>();
        private void addSong_DataReturned(object sender, SONG e)
        {
            SONG newSong = e;
            listSong.Items.Add(
            new
            {
                STT = listSong.Items.Count + 1,
                idSong = newSong.idSong,
                SongName = newSong.SongName,
                TotalTime = FomatTimeSpan(newSong.TotalTime),
            });
            listSongPlaylist.Add(newSong);
        }

        private void btnDeleteSong_Click(object sender, RoutedEventArgs e)
        {

        }

        private void listSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        MUSICAPPEntities mUSICAPPEntities = new MUSICAPPEntities();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text == "")
            {
                lblPlaylistName.Text = "Playlist name í required";
                lblPlaylistName.Visibility = Visibility.Visible;
                txtPlaylistName.Focus();
            }
            if (listSong.Items.Count == 0)
            {
                MessageBox.Show("Vui long chon bai hat");
            }
            PLAYLIST newPlaylist = CreatePlaylist();
            mUSICAPPEntities.PLAYLISTs.Add(newPlaylist);
            mUSICAPPEntities.SaveChanges();
            int idLastPlaylist = GetLastIdPlaylist();

            foreach(SONG song in listSongPlaylist)
            {
                song.idPlaylist = idLastPlaylist;
                mUSICAPPEntities.SONGs.Add(song);
                mUSICAPPEntities.SaveChanges();
            }

            Close();
        }

        private PLAYLIST CreatePlaylist()
        {
            PLAYLIST playlist = new PLAYLIST();
            playlist.PlaylistName = txtPlaylistName.Text;
            playlist.TotalSong = listSong.Items.Count;
            playlist.Created = DateTime.Now;
            return playlist;
        }

        private int GetLastIdPlaylist()
        {
            var query = from playlist in mUSICAPPEntities.PLAYLISTs
                        orderby playlist.idPlaylist descending
                        select new
                        {
                            idPlaylist = playlist.idPlaylist,
                        };
            var lastPlaylist = query.FirstOrDefault();
            if (lastPlaylist != null)
            {
                return lastPlaylist.idPlaylist;
            }
            return 0;
        }

        private void txtPlaylistName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text == "")
            {
                lblPlaylistName.Text = "Playlist name í required";
                lblPlaylistName.Visibility = Visibility.Visible;
            }
            else
            {
                lblPlaylistName.Visibility = Visibility.Hidden;
            }
        }
    }
}
