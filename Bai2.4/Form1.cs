using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai2._4
{
    public partial class Form1 : Form
    {
        private Random random = new Random();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            int x = random.Next(0,this.ClientSize.Width);
            int y = random.Next(0,this.ClientSize.Height);
            e.Graphics.DrawString("Paint Event", new Font("Arial", 20), Brushes.Black, new Point(x,y));
        }
       
    }
}
