using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonDetectorClient
{
    class ActionExecutor
    {
        private const string ENDPOINT = "http://127.0.0.1:5000/api/predict";
        private Image image;
        private DashViewer viewer;
        private Processor processor;
        private float threshold;

        public class DetectionList
        {
            [JsonProperty("detections")]
            public List<Detection> detections { get; set; }
            public DetectionList()
            {
                detections = new List<Detection>();
            }
        }

        public ActionExecutor(Image image, float threshold, DashViewer viewer, Processor processor)
        {
            this.image = image;
            this.threshold = threshold;
            this.viewer = viewer;
            this.processor = processor;
        }

        public void ExecuteLazy()
        {
            processor.setBusy(true);
            Thread t = new Thread(resolveAnnotationsFromServer);
            t.Start();
        }

        private void resolveAnnotationsFromServer()
        {
            byte[] imageBytes;
            using (MemoryStream mem = new MemoryStream())
            {
                image.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                imageBytes = mem.ToArray();
            }
            
            string imageEncoded = Convert.ToBase64String(imageBytes);
            List<Detection> serverDetections = new List<Detection>();
            string url = ENDPOINT;
            var request = (HttpWebRequest)WebRequest.Create(url);

            request.Method = "POST";
            request.ContentType = "application/json";
            List<Detection> detections = new List<Detection>();
            int serverTime = -1;
            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = "{" + 
                        "\"inputs\":\"" + imageEncoded + "\" , " + 
                        "\"threshold\":\"" + threshold.ToString() + "\"" +
                        "}";

                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();
                }
                DateTime preTime = DateTime.Now;
                var webResponse = (HttpWebResponse)request.GetResponse();
                serverTime = DateTime.Now.Subtract(preTime).Milliseconds;
                string result;
                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
                detections = JsonConvert.DeserializeObject<DetectionList>(result).detections;
            }
            catch (Exception ex)
            {
                viewer.updateException(ex);
            }
            completedAction(image, detections, serverTime);
        }

        private void completedAction(Image image, List<Detection> detections, int processTime)
        {
            Pen bluePen = new Pen(Color.Blue, (int)(image.Width / 100) + 1);
            using (Graphics g = Graphics.FromImage(image))
            {
                foreach (var detection in detections)
                {
                    if (detection.classId != 1)
                        continue;
                    int x = (int)((float)image.Width * detection.box.xmin);
                    int y = (int)((float)image.Height * detection.box.ymin);
                    int width = (int)((float)image.Width * (detection.box.xmax - detection.box.xmin));
                    int height = (int)((float)image.Height * (detection.box.ymax - detection.box.ymin));

                    Rectangle rectangle = new Rectangle(x, y, width, height);
                    g.DrawRectangle(bluePen, rectangle);
                }
            }
            string message = "Response Time : " + processTime +
                " Image resolution : " + image.Width + " X " + image.Height;
            viewer.update(image, message);
            processor.setBusy(false);
        }
    }
}
