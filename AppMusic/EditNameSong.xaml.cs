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
    /// Interaction logic for EditNameSong.xaml
    /// </summary>
    public partial class EditNameSong : Window
    {
        public SONG song { get; set; }
        public MainWindow mainWindow { set; get; }   
        public EditNameSong()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void txtPlaylistName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSongName.Text == "")
            {
                lblSongName.Text = "Song name is required";
                lblSongName.Visibility = Visibility.Visible;
            }
            else
            {
                lblSongName.Visibility = Visibility.Hidden;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(song != null)
            {
                SONG s = DataProvider.Ins.DB.SONGs.Find(song.idSong);
                s.SongName = txtSongName.Text;
                s.Artist = txtArtistName.Text;
                DataProvider.Ins.DB.SaveChanges();
                this.Close();
            }
        }
    }
}
