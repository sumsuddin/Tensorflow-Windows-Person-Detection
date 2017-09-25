using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace PersonDetectorClient
{
    public partial class Main : Form, DashViewer
    {

        private const int PB_HEIGHT = 640;
        private const int PB_WIDTH = 640;
        private WebCamProcessor webcamProcessor;
        private StreamProcessor streamProcessor;
        //private VideoProcessor videoProcessor;

        bool pause;

        public Main()
        {
            InitializeComponent();
            Text = "Person Detector";
            float threshold = float.Parse(tbThreshold.Text);
            webcamProcessor = new WebCamProcessor(this, threshold);
            int timeout = 15;
            streamProcessor = new StreamProcessor(timeout, this, threshold);

            foreach (string device in webcamProcessor.getCamList())
            {
                cbCamList.Items.Add(device);
            }

            btAutoPlay.Text = "Pause";
            pause = false;

            if (chkbUseStream.Checked)
            {
                streamProcessor.play(tbRtmp.Text);
            }
            else
            {
                webcamProcessor.play();
            }
        }

        private void updateUI(string message, Image image)
        {
            var oldImage = pbMain.Image;

            pbMain.Image = image;
            pbMain.Size = getRenderSize(pbMain.Image);
            pbMain.Invalidate();

            lblMessage.Text = message;

            if (oldImage != null)
            {
                oldImage.Dispose();
            }
        }

        private void exceptionAction(Exception ex)
        {
            //TODO complete this method
        }

        private Size getRenderSize(Image image)
        {
            Size renderSize = getSizeFor(image);
            return renderSize;
        }

        private Size getSizeFor(Image image)
        {
            double ratio = getRatio(image);
            int width = (int)((double)image.Width / ratio);
            int height = (int)((double)image.Height / ratio);
            return new Size(width, height);
        }

        private double getRatio(Image image)
        {
            double ratioX = (double)image.Width / (double)PB_WIDTH;
            double ratioY = (double)image.Height / (double)PB_HEIGHT;
            return image.Height < image.Width ? ratioX : ratioY;
        }

        private void btAutoPlay_Click(object sender, EventArgs e)
        {
            if (pause)
            {
                btAutoPlay.Text = "Play";
                if (chkbUseStream.Checked)
                {
                    streamProcessor.pause();
                }
                else
                {
                    webcamProcessor.pause();
                }
            }
            else
            {
                btAutoPlay.Text = "Pause";
                if (chkbUseStream.Checked)
                {
                    streamProcessor.play(tbRtmp.Text);
                }
                else
                {
                    webcamProcessor.play();
                }
            }
            pause = !pause;
        }

        public void update(Image image, string message)
        {
            Invoke((Action)(() => updateUI(message, image)));
        }

        public void updateException(Exception ex)
        {
            btAutoPlay.PerformClick();
            File.AppendAllText("log.txt", ex.Message + Environment.NewLine);
            Invoke((Action)(() => lblMessage.Text = ex.Message));
        }

        private void cbCamList_SelectedIndexChanged(object sender, EventArgs e)
        {
            webcamProcessor.setDevice(cbCamList.SelectedIndex);
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            webcamProcessor.stop();
            streamProcessor.stop();
            Environment.Exit(Environment.ExitCode);
        }

        private void chkbUseRtmp_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbUseStream.Checked)
            {
                webcamProcessor.stop();
                tbRtmp.Enabled = true;
                cbCamList.Enabled = false;
                streamProcessor.play(tbRtmp.Text);
            }
            else
            {
                tbRtmp.Enabled = false;
                cbCamList.Enabled = true;
                streamProcessor.stop();
                webcamProcessor.play();
            }
        }

        private void tbRtmp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                streamProcessor.stop();
                streamProcessor.play(tbRtmp.Text);
            }
        }

        private void tbThreshold_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                float threshold = float.Parse(tbThreshold.Text);
                webcamProcessor.setThreshold(threshold);
                streamProcessor.setThreshold(threshold);
            }
        }
    }
}
