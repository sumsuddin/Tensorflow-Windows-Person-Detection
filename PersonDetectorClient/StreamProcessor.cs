using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebEye;

namespace PersonDetectorClient
{
    class StreamProcessor : Processor
    {
        private StreamPlayerControl streamPlayerControler;
        private DashViewer viewer;
        private int timeout;
        private float threshold;
        Timer timer;

        bool busy;

        public StreamProcessor(int timeout, DashViewer viewer, float threshold)
        {
            setBusy(false);
            this.timeout = timeout;
            this.viewer = viewer;
            this.threshold = threshold;
            streamPlayerControler = new StreamPlayerControl();
            streamPlayerControler.StreamStarted += new System.EventHandler(HandleStreamStartedEvent);
            streamPlayerControler.StreamStopped += new System.EventHandler(HandleStreamStoppedEvent);
            streamPlayerControler.StreamFailed += new System.EventHandler<StreamFailedEventArgs>(HandleStreamFailedEvent);

            timer = new Timer();
            timer.Interval = 100;
            timer.Tick += new EventHandler(timer_tick);
        }

        private void HandleStreamStartedEvent(object sender, EventArgs e)
        {
        }

        private void HandleStreamFailedEvent(object sender, StreamFailedEventArgs e)
        {
        }

        private void HandleStreamStoppedEvent(object sender, EventArgs e)
        {
        }

        public void play(string url)
        {
            try
            {
                streamPlayerControler.StartPlay(new Uri(url), TimeSpan.FromSeconds(timeout));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Person Detector Stream Player",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            timer.Start();
        }

        public void pause()
        {
            stop();
        }

        public void stop()
        {
            try
            {
                timer.Stop();
                streamPlayerControler.Stop();
            }
            catch (Exception ex)
            {
                //remain silent. :|
            }
        }

        void timer_tick(object sender, EventArgs eventArgs)
        {
            if (!isBusy())
            {
                try
                {
                    Image image = Util.DeepCopy(streamPlayerControler.GetCurrentFrame());
                    ActionExecutor ae = new ActionExecutor(Util.DeepCopy(image), threshold, viewer, this);
                    ae.ExecuteLazy();
                }
                catch (Exception ex)
                {
                    //pass frame not ready yet.
                }
            }
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
