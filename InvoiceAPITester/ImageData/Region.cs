﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAPITester
{
    public class Region
    {
        public string boundingBox { get; set; }
        public List<Line> lines { get; set; }
    }
}
