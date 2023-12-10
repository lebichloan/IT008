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
using System.Windows.Shapes;

namespace AppMusic
{
    /// <summary>
    /// Interaction logic for AllPlaylist.xaml
    /// </summary>
    public partial class AllPlaylist : Window
    {
        public SONG song { get; set; }
        public AllPlaylist()
        {
            InitializeComponent();
            LoadAllPlaylist();
            
        }

        private void LoadAllPlaylist()
        {
            var queryallplaylist = DataProvider.Ins.DB.PLAYLISTs.OrderBy(s => s.idPlaylist);

            listPlaylist.ItemsSource = queryallplaylist.ToArray();
        }

        private void listPlaylist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(listPlaylist.SelectedItem != null)
            {
                PLAYLIST playlist = (PLAYLIST)listPlaylist.SelectedItem;
                if(playlist != null)
                {
                    SONG songnew = new SONG()
                    {
                        SongName = song.SongName,
                        TotalTime = song.TotalTime,
                        FilePath = song.FilePath,
                        Created = DateTime.Now,
                        idPlaylist = playlist.idPlaylist,

                    };
                    if(song.Artist != null)
                    {
                        songnew.Artist = song.Artist;
                    }
                    else
                    {
                        songnew.Artist = "";
                    }
                    DataProvider.Ins.DB.SONGs.Add(songnew);
                    PLAYLIST playlistTrigger = DataProvider.Ins.DB.PLAYLISTs.Find(playlist.idPlaylist);
                    playlistTrigger.TotalSong++;
                    DataProvider.Ins.DB.SaveChanges();
                    this.Close();
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
