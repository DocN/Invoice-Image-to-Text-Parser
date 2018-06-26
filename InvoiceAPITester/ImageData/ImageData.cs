using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAPITester
{
    public class ImageData
    {
        public string language { get; set; }
        public string orientation { get; set; }
        public double textAngle { get; set; }
        public List<Region> regions { get; set; }
    }
}
