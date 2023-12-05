using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;
using AppMusic.Pages;
using Microsoft.Win32;
using NAudio.Wave;

namespace AppMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public static MediaPlayerManager MediaPlayerManager { get; private set; }
        private bool isSliderDragged = false;
        private double volumePre;
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            MediaPlayerManager.IsPlayingChanged += MediaPlayerManager_IsPlayingChanged;
            volumePre = 0;
            MediaPlayerManager = new MediaPlayerManager();
            PausePlayMusic.DataContext = MediaPlayerManager;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonRestore_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
                WindowState = WindowState.Maximized;
            else
                WindowState = WindowState.Normal;
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        
    // Dừng và phát nhạc
        private void PausePlayMusic_Click(object sender, RoutedEventArgs e)
        {
            if(MediaPlayerManager.MediaPlayer != null && MediaPlayerManager.IsPlaying && MediaPlayerManager.filePath != string.Empty)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.IsPlaying = false;
            }
            else if(MediaPlayerManager.MediaPlayer != null && !MediaPlayerManager.IsPlaying && MediaPlayerManager.filePath != string.Empty)
            {
                MediaPlayerManager.MediaPlayer.Play();
                MediaPlayerManager.IsPlaying = true;
            }
            else if(MediaPlayerManager.filePath == string.Empty)
            {
                MediaPlayerManager.filePath = @"C:\Users\Laptop MSI\Downloads\ALoi.mp3";
                MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                SetupTimer();
            }
        }
        // Sự kiện xảy ra khi IsPlaying thay đổi giá trị
        private void MediaPlayerManager_IsPlayingChanged(object sender, EventArgs e)
        {

            // Đồng thời gọi hàm này để slider chạy theo thời gian bài nhạc
            SetupTimer();
        }

        // Volume
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayerManager.MediaPlayer.Volume = slider.Value/100;
        }

        private void slider_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Lấy vị trí của chuột
                Point mousePosition = e.GetPosition(slider);

                // Tính toán giá trị dựa trên vị trí của chuột và chiều dài của Slider
                double value = mousePosition.X / slider.ActualWidth * (slider.Maximum - slider.Minimum);

                // Đặt giá trị mới cho Slider
                slider.Value = value;
            }
        }
    // Hết Volume
    
    // Thời gian bài nhạc
        private void sliderPlayer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Xử lý khi giá trị của Slider thay đổi
            if (MediaPlayerManager.filePath == string.Empty)
            {
                MediaPlayerManager.filePath = @"C:\Users\Laptop MSI\Downloads\ALoi.mp3";
                MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                SetupTimer();
            }
            if (!isSliderDragged)
            {
                double newPosition = sliderPlayer.Value;
                UpdateMusicPosition(newPosition);
            }
        }
        private void UpdateMusicPosition(double newPosition)
        {
            try
            {
                // Tính toán và đặt vị trí của bài hát dựa trên giá trị mới của Slider
                double totalDuration = MediaPlayerManager.MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;
                double newTimeInSeconds = (newPosition / 100) * totalDuration;

                // Đặt vị trí của bài hát tại thời điểm mới
                MediaPlayerManager.MediaPlayer.Position = TimeSpan.FromSeconds(newTimeInSeconds);
            }
            catch { }
            
        }
        private DispatcherTimer timer;

        // Gọi hàm này khi bắt đầu phát nhạc
        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // Cập nhật mỗi giây
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            
            if (!isSliderDragged) // Chỉ cập nhật khi slider không được kéo
            {
                double currentMusicPosition = MediaPlayerManager.MediaPlayer.Position.TotalSeconds;
                double totalDuration = MediaPlayerManager.MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

                if (totalDuration > 0)
                {
                    double newPosition = (currentMusicPosition / totalDuration) * 100;
                    sliderPlayer.Value = newPosition;
                }
            }
        }

        private void sliderPlayer_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // Lấy vị trí của chuột
                Point mousePosition = e.GetPosition(sliderPlayer);

                // Tính toán giá trị dựa trên vị trí của chuột và chiều dài của Slider
                double value = mousePosition.X / sliderPlayer.ActualWidth * (sliderPlayer.Maximum - sliderPlayer.Minimum);

                // Đặt giá trị mới cho Slider
                sliderPlayer.Value = value;
            }
        }
        private void sliderPlayer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isSliderDragged = true;
        }

        private void sliderPlayer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isSliderDragged)
            {
                double newPosition = sliderPlayer.Value;
                UpdateMusicPosition(newPosition);
                isSliderDragged = false;
            }
        }

        // Hết thời gian bài nhạc
        private void volumeButton_Click(object sender, RoutedEventArgs e)
        {

            if(MediaPlayerManager.MediaPlayer.Volume != 0)
            {
                volumePre = MediaPlayerManager.MediaPlayer.Volume;
                MediaPlayerManager.MediaPlayer.Volume = 0;
                slider.Value = 0;
            }
            else
            {
                MediaPlayerManager.MediaPlayer.Volume = volumePre;
                slider.Value = volumePre*slider.Maximum;
            }
        }

        private void btnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylist addPlaylist = new AddPlaylist();
            addPlaylist.ShowDialog();
        }


        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllPlaylist();
            LoadAllSong();
        }

        MUSICAPPEntities musicappentities = new MUSICAPPEntities();
        private void LoadAllPlaylist()
        {
            var queryallplaylist = from playlist in musicappentities.PLAYLISTs
                                   orderby playlist.idPlaylist
                                   select playlist;

            listPlaylist.ItemsSource = queryallplaylist.ToList();
        }

        private void LoadAllSong()
        {
            var querAllSong = from song in musicappentities.SONGs
                              orderby song.idSong
                              select song;

            listSong.ItemsSource = querAllSong.ToList();
        }

        private void listSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void listPlaylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
