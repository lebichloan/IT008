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

        // Khai báo các biến cần sử dụng
        public static MediaPlayerManager MediaPlayerManager { get; private set; }
        private bool isSliderDragged = false;
        private double volumePre;
        private string _playTiming;
        private DispatcherTimer timer;

        public string PlayTiming
        {
            get
            {
                return _playTiming;
            }
            set
            {
                _playTiming = value;
                OnPropertyChanged(nameof(PlayTiming));
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            fContainerPage.Navigate(new System.Uri("Pages/Home.xaml", UriKind.RelativeOrAbsolute));
            MediaPlayerManager.IsPlayingChanged += MediaPlayerManager_IsPlayingChanged;
            volumePre = 0;
            MediaPlayerManager = new MediaPlayerManager();
            PausePlayMusic.DataContext = MediaPlayerManager;
            PlayTiming = "0:00";
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

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            fContainerPage.Navigate(new System.Uri("Pages/Home.xaml", UriKind.RelativeOrAbsolute));
        }
        
        // Sự kiện nut dừng phát nhạc
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
                MediaPlayerManager.filePath = "Music/ALoi.mp3";
                MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                SetupTimer();
            }
        }


        // Sự kiện xảy ra khi IsPlaying thay đổi giá trị(khi phát nhạc)
        private void MediaPlayerManager_IsPlayingChanged(object sender, EventArgs e)
        {
            SetupTimer();
        }


        // Volume
        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaPlayerManager.MediaPlayer.Volume = slider.Value/1000;
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
        // Thay đổi volume
        private void volumeButton_Click(object sender, RoutedEventArgs e)
        {

            if (MediaPlayerManager.MediaPlayer.Volume != 0)
            {
                volumePre = MediaPlayerManager.MediaPlayer.Volume;
                MediaPlayerManager.MediaPlayer.Volume = 0;
                slider.Value = 0;
            }
            else
            {
                MediaPlayerManager.MediaPlayer.Volume = volumePre;
                slider.Value = volumePre * slider.Maximum;
            }
        }
        // Hết Volume


        // Thời gian bài nhạc
        private void sliderPlayer_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Xử lý khi giá trị của Slider thay đổi
            if (MediaPlayerManager.filePath == string.Empty)
            {
                MediaPlayerManager.filePath = "Music/ALoi.mp3"; //Test thử
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
                double newTimeInSeconds = (newPosition / 1000) * totalDuration;

                // Đặt vị trí của bài hát tại thời điểm mới
                MediaPlayerManager.MediaPlayer.Position = TimeSpan.FromSeconds(newTimeInSeconds);
            }
            catch { }
            
        }

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

            if (!isSliderDragged)
            {
                // Kiểm tra xem trình phát nhạc có đang chạy không
                if (MediaPlayerManager.MediaPlayer != null && MediaPlayerManager.MediaPlayer.Source != null)
                {
                    // Kiểm tra xem NaturalDuration có giá trị Automatic không
                    if (MediaPlayerManager.MediaPlayer.NaturalDuration.HasTimeSpan)
                    {
                        double currentMusicPosition = MediaPlayerManager.MediaPlayer.Position.TotalSeconds;
                        double totalDuration = MediaPlayerManager.MediaPlayer.NaturalDuration.TimeSpan.TotalSeconds;

                        if (totalDuration > 0)
                        {
                            double newPosition = (currentMusicPosition / totalDuration) * 1000;
                            sliderPlayer.Value = newPosition;
                            UpdatePlayTiming();
                        }
                    }
                }
            }

        }
        // Update thời gian phát nhạc
        private void UpdatePlayTiming()
        {
            if (MediaPlayerManager.MediaPlayer.Source != null && MediaPlayerManager.MediaPlayer.NaturalDuration.HasTimeSpan)
            {
                TimeSpan currentPosition = MediaPlayerManager.MediaPlayer.Position;
                PlayTiming = currentPosition.ToString(@"m\:ss");
            }
        }

        // Sự kiện ấn vào một vị trí ở trên slider
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
            // Khi kéo thì nhạc vẫn phát bình thường, khi thả chuột thì nhạc mới cập nhật vị trí
            isSliderDragged = true;
        }

        private void sliderPlayer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isSliderDragged)
            {
                double newPosition = sliderPlayer.Value;
                UpdateMusicPosition(newPosition);
                isSliderDragged = false;
                UpdatePlayTiming();

            }
        }

        // Hết thời gian bài nhạc
        private void PlaySpeed_Click(object sender, RoutedEventArgs e)
        {
            speedPopup.IsOpen = true;
        }

        private void speed025_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 0.25f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed05_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if(MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 0.5f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed075_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 0.75f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 1;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed125_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 1.25f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed15_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 1.5f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed175_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 1.75f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void speed2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.MediaPlayer.SpeedRatio = 2;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }
    }
}
