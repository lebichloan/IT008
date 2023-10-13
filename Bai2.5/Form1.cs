using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bai2._5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeUI();
        }
        private void InitializeUI() // Show 2 panel để phù hợp với kích thước của form
        {
           
            // PanelDisk (TreeView)
            PanelDisk.Width = 250; 
            PanelDisk.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Bottom;


            // PanelFile (ListView)
            PanelFile.Anchor = AnchorStyles.Top  | AnchorStyles.Left | AnchorStyles.Bottom;
            PanelFile.Location = new Point(255, PanelDisk.Location.Y);

            // Gắn sự kiện SizeChanged của Form
            this.SizeChanged += new EventHandler(Form1_SizeChanged);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            // Cập nhật kích thước của PanelFile để lấp đầy phần còn lại của Form
            PanelFile.Width = this.ClientSize.Width;
            PanelFile.Height = this.ClientSize.Height - 75;

            // Đảm bảo PanelDisk vẫn có kích thước Width = 250 và không thay đổi chiều cao
            PanelDisk.Height = this.ClientSize.Height - 75;
        }


        // Câu C
        private void largeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.LargeIcon;
        }

        private void smallIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.SmallIcon;
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.List;
        }

        private void detailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.Details;
        }

        private void listIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.Tile;
        }
        // Hết câu C
    }
}
