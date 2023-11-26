using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AppMusic
{
    public class MediaPlayerManager
    {
        public static string filePath = string.Empty;
        private static MediaPlayer _mediaPlayer = new MediaPlayer();
        private static bool _isPlaying = false;

        public static MediaPlayer MediaPlayer
        {
            get { return _mediaPlayer; }
            set { _mediaPlayer = value; }
        }

        public static bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (_isPlaying != value)
                {
                    _isPlaying = value;
                    OnIsPlayingChanged();
                }
            }
        }

        public static event EventHandler IsPlayingChanged;

        protected static void OnIsPlayingChanged()
        {
            IsPlayingChanged?.Invoke(null, EventArgs.Empty);
        }
        public static void PlayMusic(string filePath)
        {
            if(MediaPlayer != null) { 
                IsPlaying = true;
                MediaPlayer.Open(new Uri(filePath, UriKind.Absolute));
                MediaPlayer.Volume = 1.0;
                MediaPlayer.Play();
            }
        }
    }
}
