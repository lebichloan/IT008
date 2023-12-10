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
    /// Interaction logic for EditPlaylistName.xaml
    /// </summary>
    public partial class EditPlaylistName : Window
    {
        public PLAYLIST playlist { get; set; }
        public EditPlaylistName()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void txtPlaylistName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtPlaylistName.Text == "")
            {
                lblPlaylistName.Text = "Song name is required";
                lblPlaylistName.Visibility = Visibility.Visible;
            }
            else
            {
                lblPlaylistName.Visibility = Visibility.Hidden;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if(playlist != null)
            {
                PLAYLIST p = DataProvider.Ins.DB.PLAYLISTs.Find(playlist.idPlaylist);
                p.PlaylistName = txtPlaylistName.Text;
                DataProvider.Ins.DB.SaveChanges();
                this.Close();
            }
        }
    }
}
