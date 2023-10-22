using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Bai2._5
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            InitializeUI();
            InitializePanelDisk(); 
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

        private void InitializePanelDisk()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.Name == @"D:\") 
                {
                    //Load các thư mục, file trong ổ dĩa
                    LoadFolderOrFile(drive.RootDirectory, PanelDisk.Nodes);
                    break;
                }
            }
        }

        // Hàm đệ quy để load các thưc mục con của các thư mục con
        private void LoadFolderOrFile(DirectoryInfo directory, TreeNodeCollection nodes)
        {
            try
            {
                TreeNode NodeInCurrentFolder = new TreeNode(directory.Name);
                NodeInCurrentFolder.Tag = directory.FullName; 

                // Thêm node thư mục hiện tại vào danh sách các nodes
                nodes.Add(NodeInCurrentFolder);

                // Lấy danh sách các thư mục con
                DirectoryInfo[] FolderChildren = directory.GetDirectories();

                // Gọi đệ quy để tạo cây thư mục con cho mỗi thư mục con
                foreach (DirectoryInfo folder in FolderChildren)
                {
                    LoadFolderOrFile(folder, NodeInCurrentFolder.Nodes);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
        private string GetCurrentPath()
        {
            if (PanelFile.SelectedItems.Count == 0)
            {
                return currentPathPanelDisk;
            }
            else
            {
                string path = currentPathPanelDisk + "\\" + PanelFile.SelectedItems[0].Text;
                return path;
            }
        }

        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowSuccessMessage(string message)
        {
            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private string currentPathPanelDisk;
        /// <summary>
        /// Xử lý sự kiện khi chọn một node trong PanelDisk
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelDisk_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.Tag != null)
                {
                    currentPathPanelDisk = e.Node.Tag.ToString();
                    UpdatePanelFile(currentPathPanelDisk);
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
        }


        private void AddListViewItem(string itemName, string type)
        {
            ListViewItem item = new ListViewItem(itemName);
            item.SubItems.Add(type);
            PanelFile.Items.Add(item);
        }

        /// <summary>
        /// Cập nhật PanelFile với tệp và thư mục con của node được chọn trong PanelDisk
        /// </summary>
        /// <param name="path"></param>
        private void UpdatePanelFile(string path)
        {
            PanelFile.Items.Clear();
            try
            {
                string[] files = Directory.GetFiles(path);
                string[] directories = Directory.GetDirectories(path);

                foreach (string file in files)
                {
                    AddListViewItem(Path.GetFileName(file), "File");
                }

                foreach (string directory in directories)
                {
                    AddListViewItem(Path.GetFileName(directory), "Folder");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Error: {ex.Message}");
            }
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


        
        /// <summary>
        /// Up: đi lên thư mục cha của thư mục đang được xem hiện tại (dựa vào node đang chọn trong panel disk) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            string parentPath = Directory.GetParent(currentPathPanelDisk)?.FullName;

            if (parentPath != null)
            {
                UpdatePanelFile(parentPath);
                currentPathPanelDisk = parentPath;
            }
            else
            {
                ShowErrorMessage("Không tồn tại thư mục cha");
            }
        }


        /// <summary>
        /// Refresh: load lại thự mục đang xem hiện tại (dựa vào panel disk)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdatePanelFile(currentPathPanelDisk);
        }


        private string _copyItemPath;
        private string _cutItemPath;
        private void HandleCopyCutOperation(bool isCopy)
        {
            if (PanelFile.SelectedItems != null && PanelFile.SelectedItems.Count > 0)
            {
                _copyItemPath = isCopy ? GetCurrentPath() : null;
                _cutItemPath = isCopy ? null : GetCurrentPath();
                if (!isCopy)
                {
                    ShowSuccessMessage($"Cắt nội dung thành công \n {_cutItemPath}");
                }
                else
                {
                    ShowSuccessMessage($"Sao chép nội dung thành công \n {_copyItemPath}");
                }
            }
            else
            {
                ShowErrorMessage("Vui lòng chọn nội dung cần sao chép/cắt");
            }
        }

        /// <summary>
        /// Copy, cut, paste, delete: Copy, cut, paste, delete thư mục hoặc file đang được chọn ở mục listview bên phải.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void copyToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HandleCopyCutOperation(true);
            UpdatePanelFile(currentPathPanelDisk);
        }

        private void cutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HandleCopyCutOperation(false);
            UpdatePanelFile(currentPathPanelDisk);
        }

        private void DirectoryCopy(string sourceDir, string destDir, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            DirectoryInfo[] dirs = dir.GetDirectories();

            Directory.CreateDirectory(destDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destDir, file.Name);
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (copySubDirs)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destDir, subDir.Name);
                    DirectoryCopy(subDir.FullName, newDestinationDir, true);
                }
            }

            //if (!dir.Exists)
            //{
            //    throw new DirectoryNotFoundException($"Source directory does not exist or could not be found: {sourceDir}");
            //}

            //if (!Directory.Exists(destDir))
            //{
            //    Directory.CreateDirectory(destDir);
            //}

            //FileInfo[] files = dir.GetFiles();
            //foreach (FileInfo file in files)
            //{
            //    string tempPath = Path.Combine(destDir, file.Name);
            //    file.CopyTo(tempPath, false);
            //}

            //if (copySubDirs)
            //{
            //    foreach (DirectoryInfo subdir in dirs)
            //    {
            //        string tempPath = Path.Combine(destDir, subdir.Name);
            //        DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
            //    }
            //}
        }

        private void pasteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_copyItemPath) && (Directory.Exists(_copyItemPath) || File.Exists(_copyItemPath)))
            {
                string destinationFolder = GetCurrentPath();
                string destinationPath = Path.Combine(destinationFolder, Path.GetFileName(_copyItemPath));

                try
                {
                    if (Directory.Exists(_copyItemPath))
                    {
                        DirectoryCopy(_copyItemPath, destinationPath, true);
                        ShowSuccessMessage("Dán thư mục thành công");
                    }
                    else if (File.Exists(_copyItemPath))
                    {
                        File.Copy(_copyItemPath, destinationPath);
                        ShowSuccessMessage("Dán file thành công");
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Lỗi sao chép: {ex.Message}");
                }
            }
            else if (!string.IsNullOrEmpty(_cutItemPath) && (Directory.Exists(_cutItemPath) || File.Exists(_cutItemPath)))
            {
                string destinationFolder = GetCurrentPath();

                try
                {
                    if (Directory.Exists(_cutItemPath))
                    {
                        Directory.Move(_cutItemPath, Path.Combine(destinationFolder, Path.GetFileName(_cutItemPath)));
                        ShowSuccessMessage("Dán thư mục thành công");
                    }
                    else if (File.Exists(_cutItemPath))
                    {
                        File.Move(_cutItemPath, Path.Combine(destinationFolder, Path.GetFileName(_cutItemPath)));
                        ShowSuccessMessage("Dán file thành công");
                    }
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"Lỗi cắt: {ex.Message}");
                }
            }
            else
            {
                ShowErrorMessage("Vui lòng chọn nội dung trước khi dán");
            }
            UpdatePanelFile(currentPathPanelDisk);
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (PanelFile.SelectedItems.Count == 0 && PanelFile.SelectedItems != null)
            {
                ShowErrorMessage("Vui lòng chọn nội dung cần xóa");
            }
            else
            {
                string currentItem = GetCurrentPath();

                DialogResult result = MessageBox.Show($"Bạn chắc chắn muốn xóa nội dung đã chọn? \n {currentItem}", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (File.Exists(currentItem))
                    {
                        File.Delete(currentItem);
                        MessageBox.Show("Xóa file đã chọn thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Directory.Exists(currentItem))
                    {
                        Directory.Delete(currentItem, true);
                        MessageBox.Show("Xóa thư mục đã chọn thành công", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Có lỗi xảy ra", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                UpdatePanelFile(currentPathPanelDisk);
            }
        }


        /// <summary>
        /// View: cho phép người dùng chọn cách view của listview bên phải
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void largeIconsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.LargeIcon;
        }

        private void smallIconsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.SmallIcon;
        }

        private void listIconToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.Tile;
        }

        private void listToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.List;
        }

        private void detailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PanelFile.View = View.Details;
        }

    }
}
