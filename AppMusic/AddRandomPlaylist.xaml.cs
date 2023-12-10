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
    /// Interaction logic for AddRandomPlaylist.xaml
    /// </summary>
    public partial class AddRandomPlaylist : Window
    {
        public AddRandomPlaylist()
        {
            InitializeComponent();
            //SetValues();
        }

        List<SONG> songs = new List<SONG>();
        MUSICAPPEntities mUSICAPPEntities = new MUSICAPPEntities();
        public AddRandomPlaylist(List<SONG> s, MUSICAPPEntities m)
        {
            InitializeComponent();
            songs = s;
            mUSICAPPEntities = m;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var querAllSong = from song in mUSICAPPEntities.SONGs
                              orderby song.idSong
                              select song;
            var countSong = querAllSong.ToList().Count;

            if (txtPlaylistName.Text == "")
            {
                lblPlaylistName.Text = "Playlist name is required";
                lblPlaylistName.Visibility = Visibility.Visible;
                txtPlaylistName.Focus();
            }

            if (songs.Count == 0)
            {
                MessageBox.Show("Please add song to continue");
            }

            if (txtPlaylistName.Text != "" && songs.Count > 0)
            {
                PLAYLIST newPlaylist = CreatePlaylist();
                mUSICAPPEntities.PLAYLISTs.Add(newPlaylist);
                mUSICAPPEntities.SaveChanges();
                int idLastPlaylist = GetLastIdPlaylist();
                foreach (SONG song in songs)
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
            playlist.TotalSong = songs.Count;
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
