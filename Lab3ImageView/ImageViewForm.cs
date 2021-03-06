using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Lab3ImageView
{
    public partial class ImageViewer : Form
    {
        List<FileInfo> images;
        public ImageViewer()
        {
            InitializeComponent();

            settingsPanel.Visible = false;
        }

        private void imageReload(Image image = null)
        {
            if (pictureBox1.Image != null)
                pictureBox1.Image.Dispose();

            pictureBox1.Image = image;
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (settingsPanel.Visible)
                settingsPanel.Visible = false;
            else
                settingsPanel.Visible = true;
        }

        private void folderButton_Click(object sender, System.EventArgs e)
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if(folderBrowser.ShowDialog() == DialogResult.OK)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(folderBrowser.SelectedPath);

                images = dirInfo.GetFiles()
                    .Where(f => f.Extension == ".jpg" || f.Extension == ".png" || f.Extension == ".gif")
                    .ToList();

                imageReload();

                listBox1.Items.Clear();
                foreach(FileInfo file in images)
                    listBox1.Items.Add(file.Name);

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            imageReload();

            Image loaded = Image.FromFile(images[listBox1.SelectedIndex].FullName);

            ImageController.setOriginal(loaded);
            ImageController.adjust(hScrollBar1.Value * 0.01f, hScrollBar2.Value * 0.01f, checkBox1.Checked);
            pictureBox1.Image = ImageController.getModified();

        }

        private void sliderChange(object sender, System.EventArgs e)
        {
            ImageController.adjust(hScrollBar1.Value * 0.01f, hScrollBar2.Value * 0.01f, checkBox1.Checked);

            imageReload(ImageController.getModified());
        }

        private void checkBox1_CheckStateChanged(object sender, System.EventArgs e)
        {
            sliderChange(sender, e);
        }
    }
}
