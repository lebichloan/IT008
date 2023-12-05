using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMusic
{
    public class DataProvider
    {
        public static DataProvider _ins;
        public static DataProvider Ins
        {
            get
            {
                if (_ins == null)
                    _ins = new DataProvider();
                return _ins;
            }
            set
            {
                _ins = value;
            }
        }
        public MUSICAPPEntities DB { get; set; }
        private DataProvider()
        {
            DB = new MUSICAPPEntities();
        }
    }
}
