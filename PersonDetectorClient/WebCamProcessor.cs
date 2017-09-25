using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace PersonDetectorClient
{
    class WebCamProcessor : Processor
    {
        private Object thisLock = new Object();

        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;
        private List<string> devices;

        private DashViewer viewer;
        private float threshold;

        Timer timer;
        bool canProcess;
        bool busy;

        public WebCamProcessor(DashViewer viewer, float threshold)
        {
            this.threshold = threshold;
            setBusy(false);
            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();
            canProcess = true;
            this.viewer = viewer;
            try
            {
                webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);

                devices = new List<string>();
                foreach (FilterInfo videoCaptureDevice in webcam)
                {
                    devices.Add(videoCaptureDevice.Name);
                }
                cam = new VideoCaptureDevice(webcam[0].MonikerString);
                cam.NewFrame += new NewFrameEventHandler(newFrameHander);
            }
            catch (Exception ex)
            {
                File.AppendAllText("log.txt", ex.Message + Environment.NewLine);
            }
        }

        public void play()
        {
            lock (thisLock)
            {
                cam.Start();
            }
        }

        public void pause()
        {
            lock (thisLock)
            {
                cam.SignalToStop();
            }
        }

        public void stop()
        {
            pause();
            lock (thisLock)
            {
                cam.Stop();
            }
        }

        public void setDevice(int ind)
        {
            pause();
            lock (thisLock)
            {
                cam = new VideoCaptureDevice(webcam[ind].MonikerString);
                cam.NewFrame += new NewFrameEventHandler(newFrameHander);
            }
            play();
        }

        public List<String> getCamList()
        {
            return devices;
        }

        private void newFrameHander(object sender, NewFrameEventArgs eventArgs)
        {
            if (canProcess && !isBusy())
            {
                canProcess = false;
                Image image = eventArgs.Frame;
                //cam.SignalToStop();
                ActionExecutor ae = new ActionExecutor(Util.DeepCopy(image), threshold, viewer, this);
                ae.ExecuteLazy();
            }
        }

        void timer_tick(object sender, EventArgs eventArgs)
        {
            canProcess = true;
        }

        public void setBusy(bool busy)
        {
            this.busy = busy;
        }

        public bool isBusy()
        {
            return busy;
        }

        public void setThreshold(float threshold)
        {
            this.threshold = threshold;
        }
    }
}
