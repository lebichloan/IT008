using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMusic
{
    public class SongItem
    {
        public int STT { get; set; }
        public string SongName { get; set; }
        public string TotalTime { get; set; }

        public SongItem(int sTT, string songName, string totalTime)
        {
            STT = sTT;
            SongName = songName;
            TotalTime = totalTime;
        }

        public SongItem(SongItem songItem)
        {
            STT = songItem.STT;
            SongName = songItem.SongName;
            TotalTime = songItem.TotalTime;
        }
    }
}
