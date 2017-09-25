using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDetectorClient
{
    interface DashViewer
    {
        void update(Image image, string message);
        void updateException(Exception ex);
    }
}
