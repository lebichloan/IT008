using NAudio.Wave;
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
    /// Interaction logic for AddSong.xaml
    /// </summary>
    public partial class AddSong : Window
    {
        private readonly string selectedFilePath = "";
        public AddSong()
        {
            InitializeComponent();
        }

        public AddSong(string filePath)
        {
            InitializeComponent();
            selectedFilePath = filePath;
            SetValue();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private TimeSpan GetTotalTime(string filePath)
        {
            try
            {
                Mp3FileReader mp3FileReader = new Mp3FileReader(filePath);
                return mp3FileReader.TotalTime;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return TimeSpan.Zero;
        }

        static string FomatTimeSpan(TimeSpan time)
        {
            return time.Hours == 0 
                ? $"{time.Minutes:D2}:{time.Seconds:D2}"
                : $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
        
        private void SetValue()
        {
            txtSongName.Text = string.Empty;
            txtArtist.Text = string.Empty;
            txtTotalTime.Text = string.Format("{0}(s)", FomatTimeSpan(GetTotalTime(selectedFilePath)));
            txtTotalTime.IsReadOnly = true;
        }

        public event EventHandler<SONG> DataReturned;

        protected virtual void OnDataReturned(SONG data)
        {
            DataReturned?.Invoke(this, data);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtSongName.Text != "")
            {
                SONG newSong = LoadMusicFiles(selectedFilePath);
                OnDataReturned(newSong);
                Close();
            }
            else
            {
                lblSongName.Text = "Song name is requried";
                lblSongName.Visibility = Visibility.Visible;
                txtSongName.Focus();
            }
        }

        private SONG LoadMusicFiles(string filePath)
        {
            SONG song = new SONG();
            song.SongName = txtSongName.Text.ToString();
            song.Artist = txtArtist.Text.ToString();
            song.TotalTime = (TimeSpan)GetTotalTime(filePath);
            song.FilePath = filePath;
            song.Created = DateTime.Now;
            return song;
        }

        private void txtSongName_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtSongName.Text == "")
            {
                lblSongName.Text = "Song name is requried";
                lblSongName.Visibility = Visibility.Visible;
            }
            else
            {
                lblSongName.Visibility = Visibility.Hidden;
            }
        }
    }
}
