using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonDetectorClient
{
    interface Processor
    {
        void setBusy(bool busy);
        bool isBusy();
        void setThreshold(float threshold);
    }
}
