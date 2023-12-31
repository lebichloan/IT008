﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using AppMusic.Pages;
using Microsoft.Win32;
using NAudio.Wave;
using Id3;

namespace AppMusic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window, INotifyPropertyChanged
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
        public int indexPre;
        public int indexPlaylistPre;
        private bool isSelectedSong;
        private bool isShuffle = false;
        public bool IsShuffle { get { return isShuffle; } set { isShuffle = value; OnPropertyChanged(nameof(IsShuffle)); } }
        private bool isRepeat = false;
        public bool IsRepeat { get { return isRepeat; } set { isRepeat = value; OnPropertyChanged(nameof(IsRepeat)); } }
        private bool isRepeatOnce = false;
        public bool IsRepeatOnce { get { return isRepeatOnce; } set { isRepeatOnce = value; OnPropertyChanged(nameof(IsRepeatOnce)); } }



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
            MediaPlayerManager.IsPlayingChanged += MediaPlayerManager_IsPlayingChanged;
            volumePre = 0;
            MediaPlayerManager = new MediaPlayerManager();
            PausePlayMusic.DataContext = MediaPlayerManager;
            PlayTiming = "0:00";
            MediaPlayerManager.MediaPlayer.MediaEnded += OnMediaEnded;
            indexPlaylistPre = -1;
            indexPre = -1;
            isSelectedSong = false;
        }

        private void OnMediaEnded(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
            if (IsRepeat && !IsRepeatOnce)
            {
                if (listSong.SelectedIndex < listSong.Items.Count - 1)
                {
                    listSong.SelectedIndex++;
                    SONG song = (SONG)listSong.SelectedItem;
                    MediaPlayerManager.filePath = song.FilePath;
                    MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                    tblNameSongPlaying.Text = song.SongName.ToString();
                    tblNameArtistPlaying.Text = song.Artist.ToString();
                }
                else
                {
                    listSong.SelectedIndex = 0;
                    SONG song = (SONG)listSong.SelectedItem;
                    MediaPlayerManager.filePath = song.FilePath;
                    MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                    tblNameSongPlaying.Text = song.SongName.ToString();
                    tblNameArtistPlaying.Text = song.Artist.ToString();
                }
            }
            else if (!IsRepeat && IsRepeatOnce)
            {
                SONG song = (SONG)listSong.SelectedItem;
                MediaPlayerManager.filePath = song.FilePath;
                MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                tblNameSongPlaying.Text = song.SongName.ToString();
                tblNameArtistPlaying.Text = song.Artist.ToString();
            }
            else
            {
                if (listSong.SelectedIndex < listSong.Items.Count - 1)
                {
                    listSong.SelectedIndex++;
                    SONG song = (SONG)listSong.SelectedItem;
                    MediaPlayerManager.filePath = song.FilePath;
                    MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                    tblNameSongPlaying.Text = song.SongName.ToString();
                    tblNameArtistPlaying.Text = song.Artist.ToString();
                }
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
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

        // Sự kiện nut dừng phát nhạc
        private void PausePlayMusic_Click(object sender, RoutedEventArgs e)
        {
            if (MediaPlayerManager.MediaPlayer != null && MediaPlayerManager.IsPlaying && MediaPlayerManager.filePath != string.Empty)
            {
                MediaPlayerManager.MediaPlayer.Pause();
                MediaPlayerManager.IsPlaying = false;
            }
            else if (MediaPlayerManager.MediaPlayer != null && !MediaPlayerManager.IsPlaying && MediaPlayerManager.filePath != string.Empty)
            {
                MediaPlayerManager.MediaPlayer.Play();
                MediaPlayerManager.IsPlaying = true;
            }
            else if (MediaPlayerManager.filePath == string.Empty)
            {

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
            MediaPlayerManager.MediaPlayer.Volume = slider.Value / 1000;
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
            if (MediaPlayerManager.MediaPlayer != null)
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
                MediaPlayerManager.MediaPlayer.SpeedRatio = 2f;
                MediaPlayerManager.MediaPlayer.Play();
            }
        }

        private void btnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylist addPlaylist = new AddPlaylist();
            addPlaylist.ShowDialog();

            LoadAllPlaylist();
            LoadAllSong(0);
        }

        private void mainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAllPlaylist();
            LoadAllSong(0);
        }
        static string FomatTimeSpan(TimeSpan time)
        {
            return time.Hours == 0
                ? $"{time.Minutes:D2}:{time.Seconds:D2}"
                : $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        MUSICAPPEntities musicappentities = new MUSICAPPEntities();
        public void LoadAllPlaylist()
        {
            //var queryallplaylist = from playlist in musicappentities.PLAYLISTs
            //                       orderby playlist.idPlaylist
            //                       select playlist;
            var queryallplaylist = DataProvider.Ins.DB.PLAYLISTs.OrderBy(s => s.idPlaylist);

            listPlaylist.ItemsSource = queryallplaylist.ToArray();
        }

        public void LoadAllSong(int idPlaylist)
        {
            if (idPlaylist == 0)
            {
                //var querAllSong = from song in musicappentities.SONGs
                //                  orderby song.idSong
                //                  select song;
                var querAllSong = DataProvider.Ins.DB.SONGs.OrderBy(s => s.idSong);
                listSong.ItemsSource = querAllSong.ToList();
                lblPlaylistName.Text = "All song";
                if (querAllSong.ToList().Count == 0)
                {
                    lblTotalSong.Text = "";
                }
                else
                {
                    lblTotalSong.Text = string.Format("Total song: {0}", querAllSong.ToList().Count.ToString());
                }
            }
            else
            {
                var querAllSong = DataProvider.Ins.DB.SONGs.Where(s => s.idPlaylist == idPlaylist).OrderBy(s => s.idSong);
                listSong.ItemsSource = querAllSong.ToList();
                var selectedPlaylist = DataProvider.Ins.DB.PLAYLISTs.Where(p => p.idPlaylist == idPlaylist);
                var lst = selectedPlaylist.ToList();
                lblPlaylistName.Text = lst[0].PlaylistName;
                if (querAllSong.ToList().Count == 0)
                {
                    lblTotalSong.Text = "";
                }
                else
                {
                    lblTotalSong.Text = string.Format("Total song: {0}", querAllSong.ToList().Count.ToString());
                }
            }
        }
        private void btnNextSong_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (listSong.SelectedIndex < listSong.Items.Count - 1)
                {
                    listSong.SelectedIndex++;
                    SONG song = (SONG)listSong.SelectedItem;
                    if(song != null)
                    {
                        MediaPlayerManager.filePath = song.FilePath;
                        MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                        tblNameSongPlaying.Text = song.SongName.ToString();
                        tblNameArtistPlaying.Text = song.Artist.ToString();
                    }
                }
                else
                {
                    listSong.SelectedIndex = 0;
                    SONG song = (SONG)listSong.SelectedItem;
                    if (song != null)
                    {
                        MediaPlayerManager.filePath = song.FilePath;
                        MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                        tblNameSongPlaying.Text = song.SongName.ToString();
                        tblNameArtistPlaying.Text = song.Artist.ToString();
                    }
                }
            }
            
            catch { }
            
        }

        private void btnPreviousSong_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (listSong.SelectedIndex > 0)
                {
                    listSong.SelectedIndex--;
                    SONG song = (SONG)listSong.SelectedItem;
                    if(song != null)
                    {
                        MediaPlayerManager.filePath = song.FilePath;
                        MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                        tblNameSongPlaying.Text = song.SongName.ToString();
                        tblNameArtistPlaying.Text = song.Artist.ToString();
                    }
                }
                else
                {
                    listSong.SelectedIndex = listSong.Items.Count - 1;
                    SONG song = (SONG)listSong.SelectedItem;
                    if(song != null )
                    {
                        MediaPlayerManager.filePath = song.FilePath;
                        MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                        tblNameSongPlaying.Text = song.SongName.ToString();
                        tblNameArtistPlaying.Text = song.Artist.ToString();
                    }
                }
            }
            catch { }
            
        }

        //MUSICAPPEntities mUSICAPPEntities = new MUSICAPPEntities();
        private void btnAddFolder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CommonOpenFileDialog dialog = new CommonOpenFileDialog();
                dialog.InitialDirectory = "C:\\Users";
                dialog.IsFolderPicker = true;
                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    
                }

                if (Directory.GetFiles(dialog.FileName, "*.mp3").Length <= 0)
                {
                    var Result = MessageBox.Show("This folder has no .mp3 file! Do you want to proceed?", "Folder has no mp3 file", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (Result == MessageBoxResult.Yes)
                    {

                    }
                    else if (Result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                PLAYLIST newPlaylist = CreatePlaylist(dialog.FileName);
                newPlaylist.idPlaylist = GetLastIdPlaylist();
                musicappentities.PLAYLISTs.Add(newPlaylist);
                musicappentities.SaveChanges();


                string directory = dialog.FileName;
                string[] musicFiles = Directory.GetFiles(directory, "*.mp3");
                foreach (string musicFile in musicFiles)
                {

                    SONG temp = new SONG();
                    using (var mp3 = new Mp3(musicFile))
                    {
                        Id3Tag tag = mp3.GetTag(Id3TagFamily.Version2X);
                        if (tag != null)
                        {
                            temp.SongName = tag.Title;
                            temp.Artist = tag.Artists;

                        }
                        else
                        {
                            temp.SongName = System.IO.Path.GetFileNameWithoutExtension(musicFile);
                            temp.Artist = "";
                        }

                    }
                    temp.FilePath = musicFile;
                    temp.idPlaylist = newPlaylist.idPlaylist;
                    temp.Created = DateTime.Now;
                    temp.TotalTime = (TimeSpan)GetTotalTime(musicFile);
                    musicappentities.SONGs.Add(temp);
                    musicappentities.SaveChanges();
                }
                LoadAllPlaylist();
                LoadAllSong(0);
            }
            catch { }
        }

        private PLAYLIST CreatePlaylist(string path)
        {
            PLAYLIST playlist = new PLAYLIST();
            playlist.PlaylistName = System.IO.Path.GetFileNameWithoutExtension(path);
            playlist.TotalSong = Directory.GetFiles(path, "*.mp3").Length;
            playlist.Created = DateTime.Now;
            return playlist;
        }

        private int GetLastIdPlaylist()
        {

            var query = from playlist in musicappentities.PLAYLISTs
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

        private void ShuffleButton_Click(object sender, RoutedEventArgs e)
        {
            IsShuffle = !IsShuffle;

            if (listPlaylist.SelectedIndex == -1)
            {
                var querAllSong = from song in musicappentities.SONGs
                                  orderby song.idSong
                                  select song;
                var list = querAllSong.ToList();
                if (IsShuffle)
                {
                    var mappingList = list;
                    List<SONG> shuffledList = new List<SONG>();
                    Random r = new Random();

                    while (mappingList.Count > 0)
                    {
                        int index = r.Next(mappingList.Count);
                        shuffledList.Add(mappingList[index]);
                        mappingList.RemoveAt(index);
                    }

                    listSong.ItemsSource = shuffledList;
                }
                else
                {
                    listSong.ItemsSource = list;
                }
            }
            else
            {
                PLAYLIST temp = (PLAYLIST)listPlaylist.SelectedItem;
                var querAllSong = from song in musicappentities.SONGs
                                  where song.idPlaylist == temp.idPlaylist
                                  orderby song.idSong
                                  select song;
                var list = querAllSong.ToList();
                if (IsShuffle)
                {
                    var mappingList = list;
                    List<SONG> shuffledList = new List<SONG>();
                    Random r = new Random();

                    while (mappingList.Count > 0)
                    {
                        int index = r.Next(mappingList.Count);
                        shuffledList.Add(mappingList[index]);
                        mappingList.RemoveAt(index);
                    }

                    listSong.ItemsSource = shuffledList;
                }
                else
                {
                    listSong.ItemsSource = list;
                }
            }
        }
        private void BackwardButton_Clicked(object sender, RoutedEventArgs e)
        {
            indexPlaylistPre = -1;
            listPlaylist.SelectedIndex = -1;
            IsShuffle = false;
            IsRepeat = false;
            IsRepeatOnce = false;
            LoadAllSong(0);
        }

        private void RepeatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!(IsRepeat || IsRepeatOnce))
            {
                IsRepeat = !IsRepeat;
            }
            else if (IsRepeat)
            {
                IsRepeatOnce = !IsRepeatOnce;
                IsRepeat = !IsRepeat;
            }
            else if (IsRepeatOnce)
            {
                IsRepeatOnce = !IsRepeatOnce;
            }
        }

        private void listSong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSelectedSong)
            {
                try
                {
                    if (listSong.SelectedItem != null)
                    {
                        SONG song = (SONG)listSong.SelectedItem;
                        if (song != null)
                        {
                            MediaPlayerManager.filePath = song.FilePath;
                            MediaPlayerManager.PlayMusic(MediaPlayerManager.filePath);
                            tblNameSongPlaying.Text = song.SongName.ToString();
                            tblNameArtistPlaying.Text = song.Artist.ToString();
                            tblTotalTime.Text = FomatTimeSpan(song.TotalTime).ToString();
                            indexPre = listSong.SelectedIndex;
                        }

                    }

                }
                catch { }
            }
        }
        private void listSong_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            isSelectedSong = false;
        }
        private void listSong_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isSelectedSong = true;
        }
        private void ContextMenuListSong_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                if (indexPre == -1)
                {
                    listSong.SelectedItem = null;   
                }
                else
                {
                    listSong.SelectedIndex = indexPre;
                }
            }
        }

        private void DeleteFromPlaylist_Click(object sender, RoutedEventArgs e)
        {
            int indexPlaylist = 0;
            try 
            {
                SONG song = (SONG)listSong.SelectedItem;
                if (song != null)
                {
                    SONG s = DataProvider.Ins.DB.SONGs.Find(song.idSong);
                    if(s != null)
                    {
                        PLAYLIST playlist = DataProvider.Ins.DB.PLAYLISTs.Find(song.idPlaylist);
                        if(playlist != null)
                        {
                            indexPlaylist = playlist.idPlaylist;
                            playlist.TotalSong--;
                            s.idPlaylist = null;
                            DataProvider.Ins.DB.SaveChanges();
                            LoadAllSong(indexPlaylist);
                            listPlaylist.SelectedIndex = indexPlaylist - 1;
                            lblPlaylistName.Text = playlist.PlaylistName;
                            lblTotalSong.Text = string.Format("Total song: {0}", playlist.TotalSong.ToString());
                            IsShuffle = false;
                            IsRepeat = false;
                            IsRepeatOnce = false;
                            LoadAllPlaylist();
                        }
                    }
                }
            }
            catch { }   
        }

        private void DeleteFromApp_Click(object sender, RoutedEventArgs e)
        {
            int indexPlaylist = 0;
            try 
            {
                SONG song = (SONG)listSong.SelectedItem;
                if (song != null)
                {
                    SONG s = DataProvider.Ins.DB.SONGs.Find(song.idSong);
                    if(s.idPlaylist != null)
                    {

                        PLAYLIST playlist = DataProvider.Ins.DB.PLAYLISTs.Find(song.idPlaylist);
                        if (playlist != null)
                        {
                            indexPlaylist = playlist.idPlaylist;
                            playlist.TotalSong--;
                        }
                        
                    }
                    DataProvider.Ins.DB.SONGs.Remove(s);
                    DataProvider.Ins.DB.SaveChanges();
                    if (listPlaylist.SelectedItem == null)
                    {
                        LoadAllSong(0);
                    }
                    else
                    {
                        LoadAllSong(indexPlaylist);
                    }
                    LoadAllPlaylist();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void ContextMenuListPlaylist_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                if(indexPlaylistPre == -1)
                {
                    listPlaylist.SelectedItem = null;
                }
                else
                {
                    listPlaylist.SelectedIndex = indexPlaylistPre;
                }
            }
        }

        private void listPlaylist_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (listPlaylist.SelectedItem != null)
            {
                IsShuffle = false;
                IsRepeat = false;
                IsRepeatOnce = false;
                PLAYLIST selectedPlaylist = (PLAYLIST)listPlaylist.SelectedItem;
                if (selectedPlaylist != null)
                {
                    indexPlaylistPre = listPlaylist.SelectedIndex;
                    LoadAllSong(selectedPlaylist.idPlaylist);
                    lblPlaylistName.Text = selectedPlaylist.PlaylistName;
                    lblTotalSong.Text = string.Format("Total song: {0}", selectedPlaylist.TotalSong.ToString());
                }
                
            }
            else
            {
                IsShuffle = false;
                IsRepeat = false;
                IsRepeatOnce = false;
            }
        }

        private void DeletePlaylist_Click(object sender, RoutedEventArgs e)
        {
            int indexItemMouseClick = 0;
            try 
            {
                PLAYLIST pLAYLIST = (PLAYLIST)listPlaylist.SelectedItem;
                if (pLAYLIST != null)
                {
                    indexItemMouseClick = listPlaylist.SelectedIndex;
                    PLAYLIST playlist = DataProvider.Ins.DB.PLAYLISTs.Find(pLAYLIST.idPlaylist);
                    if(playlist != null)
                    {
                        DataProvider.Ins.DB.PLAYLISTs.Remove(playlist);
                        DataProvider.Ins.DB.SaveChanges();
                        IsShuffle = false;
                        IsRepeat = false;
                        IsRepeatOnce = false;
                        LoadAllPlaylist();
                        if(indexPlaylistPre == -1)
                        {
                            LoadAllSong(0);
                            indexPlaylistPre = -1;
                        }
                        else if(indexItemMouseClick == indexPlaylistPre)
                        {
                            indexPlaylistPre = -1;
                            LoadAllSong(0);
                        }
                    }
                }
            } 
            catch { }
        }
        

        private void btnAddShufflePlaylist_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void btnAddSong_Click(object sender, RoutedEventArgs e)
        {
            try 
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
                    IsShuffle = false;
                    IsRepeat = false;
                    IsRepeatOnce = false;
                    LoadAllSong(0);
                }
            } 
            catch { }
        }
        private void addSong_DataReturned(object sender, SONG e)
        {
            SONG newSong = e;
            musicappentities.SONGs.Add(newSong);
            musicappentities.SaveChanges();
        }

        private void AddSongToPlayList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int indexPlaylistAddSong = listPlaylist.SelectedIndex;
                if(indexPlaylistAddSong != -1)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Title = "Chọn một tệp tin";
                    openFileDialog.Filter = "File MP3|*.mp3";

                    bool? result = openFileDialog.ShowDialog();
                    if (result == true)
                    {
                        var selectedFilePath = (dynamic)openFileDialog.FileName;
                        string filepath = selectedFilePath.ToString();
                        AddSong addSong = new AddSong(selectedFilePath);
                        addSong.DataReturned += addSong_DataReturned;
                        addSong.ShowDialog();
                        PLAYLIST playlist = (PLAYLIST)listPlaylist.Items[indexPlaylistAddSong];
                        SONG song = DataProvider.Ins.DB.SONGs.OrderByDescending(s => s.idSong).FirstOrDefault(s => s.FilePath == filepath && s.idPlaylist == null);
                        if (song != null)
                        {
                            song.idPlaylist = playlist.idPlaylist;
                            PLAYLIST playlistTrigger = DataProvider.Ins.DB.PLAYLISTs.Find(playlist.idPlaylist);
                            playlistTrigger.TotalSong++;
                            DataProvider.Ins.DB.SaveChanges();
                            if (indexPlaylistPre == indexPlaylistAddSong)
                            {
                                LoadAllSong(playlist.idPlaylist);
                            }
                            LoadAllPlaylist();
                        }
                        
                    }
                }
            }
            catch { }
        }

        private void btnAddRandomSong_Click(object sender, RoutedEventArgs e)
        {
            int countSong = DataProvider.Ins.DB.SONGs.Count();
            if(countSong > 0)
            {
                Random rand = new Random();
                var querAllSong = from song in musicappentities.SONGs
                                  orderby song.idSong
                                  select song;
                var slist = querAllSong.ToList();
                int listCount = slist.Count;
                int randCount;
                if (listCount == 1)
                {
                    randCount = 1;
                }
                else
                {
                    randCount = rand.Next(1, listCount);
                }

                List<SONG> mappingList = new List<SONG>(slist);
                List<SONG> randomList = new List<SONG>();

                for (int i = 0; i < randCount; i++)
                {
                    int index = rand.Next(mappingList.Count);
                    randomList.Add(mappingList[index]);
                    mappingList.RemoveAt(index);
                }
                AddRandomPlaylist addRandomPlaylist = new AddRandomPlaylist(randomList, musicappentities);
                addRandomPlaylist.ShowDialog();
                IsShuffle = false;
                IsRepeat = false;
                IsRepeatOnce = false;
                LoadAllPlaylist();
                LoadAllSong(0);
            }
        }

        private void btnEditSongName_Click(object sender, RoutedEventArgs e)
        {
            if(listSong.SelectedItem != null)
            {
                SONG song = (SONG)listSong.SelectedItem;
                if(song != null)
                {
                    EditNameSong editNameSong = new EditNameSong();
                    editNameSong.txtSongName.Text = song.SongName.ToString();
                    if(song.Artist != null)
                    {
                        editNameSong.txtArtistName.Text = song.Artist.ToString();
                    }
                    else
                    {
                        editNameSong.txtArtistName.Text = "";
                    }
                    editNameSong.song = song;
                    editNameSong.ShowDialog();
                    if (song.idPlaylist != null && indexPlaylistPre != -1)
                    {
                        int temp = (int)song.idPlaylist;
                        LoadAllSong(temp);
                    }
                    else if (indexPlaylistPre == -1)
                    {
                        LoadAllSong(0);
                    }
                }
            }
        }

        private void btnEditPlaylistName_Click(object sender, RoutedEventArgs e)
        {
            if(listPlaylist.SelectedItem != null)
            {
                PLAYLIST playlist = (PLAYLIST)listPlaylist.SelectedItem;
                if(playlist != null)
                {
                    EditPlaylistName editpPlaylistName = new EditPlaylistName();
                    editpPlaylistName.txtPlaylistName.Text = playlist.PlaylistName.ToString();
                    editpPlaylistName.playlist = playlist;
                    editpPlaylistName.ShowDialog();
                    LoadAllPlaylist();
                    lblPlaylistName.Text = playlist.PlaylistName;
                }
            }
        }

        private void btnAddToPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if(listSong.SelectedItem != null)
            {
                SONG song = (SONG)listSong.SelectedItem;
                if(song != null)
                {
                    AllPlaylist allPlaylist = new AllPlaylist();
                    allPlaylist.song = song;
                    allPlaylist.ShowDialog();
                    LoadAllPlaylist();
                }
            }
        }
    }
}
