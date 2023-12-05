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
            lblTotalSong.Text = string.Format("Total song: {0}", listSong.Items.Count.ToString());
            listSongExpander.IsExpanded = false;
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
                MessageBox.Show("Please choose file to continue", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private List<SONG> listSongPlaylist = new List<SONG>();
        private void addSong_DataReturned(object sender, SONG e)
        {
            SONG newSong = e;
            SongItem songItem = new SongItem(listSong.Items.Count + 1, newSong.SongName, string.Format("{0}(s)", FomatTimeSpan(newSong.TotalTime)));
            listSong.Items.Add(songItem);
            //listSong.Items.Add(
            //new
            //{
            //    STT = listSong.Items.Count + 1,
            //    idSong = newSong.idSong,
            //    SongName = newSong.SongName,
            //    TotalTime = string.Format("{0}(s)", FomatTimeSpan(newSong.TotalTime)),
            //});
            listSongExpander.IsExpanded = true;
            lblTotalSong.Text = string.Format("Total song: {0}", listSong.Items.Count.ToString());
            listSongPlaylist.Add(newSong);
        }

        private void btnDeleteSong_Click(object sender, RoutedEventArgs e)
        {
            if (listSong.SelectedItems != null)
            {
                listSong.Items.Remove(listSong.SelectedItems[0]);

            }
        }

        private SongItem selectedSongItem = null;
        private void listSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listSong.SelectedItems != null)
            {
                //var selectedSong = (dynamic)listSong.SelectedItems[0];
                //selectedSongItem = new SongItem(listSong.SelectedItem[0]);
            }
            else
            {
                selectedSongItem = null;
            }
        }

        MUSICAPPEntities mUSICAPPEntities = new MUSICAPPEntities();

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text == "")
            {
                lblPlaylistName.Text = "Playlist name is required";
                lblPlaylistName.Visibility = Visibility.Visible;
                txtPlaylistName.Focus();
            }

            if (listSong.Items.Count == 0)
            {
                MessageBox.Show("Please add song to continue");
            }

            if (txtPlaylistName.Text != "" && listSong.Items.Count > 0)
            {
                PLAYLIST newPlaylist = CreatePlaylist();
                mUSICAPPEntities.PLAYLISTs.Add(newPlaylist);
                mUSICAPPEntities.SaveChanges();
                int idLastPlaylist = GetLastIdPlaylist();

                foreach (SONG song in listSongPlaylist)
                {
                    song.idPlaylist = idLastPlaylist;
                    mUSICAPPEntities.SONGs.Add(song);
                    mUSICAPPEntities.SaveChanges();
                }

                Close();
            }
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
                lblPlaylistName.Text = "Playlist name is required";
                lblPlaylistName.Visibility = Visibility.Visible;
            }
            else
            {
                lblPlaylistName.Visibility = Visibility.Hidden;
            }
        }
    }
}
