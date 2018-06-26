using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAPITester.Model
{
    public class Boundary
    {
        //class variables 
        private int xBegin;
        private int yBegin;
        private int xSize;
        private int ySize;

        public int XBegin { get => xBegin; set => xBegin = value; }
        public int YBegin { get => yBegin; set => yBegin = value; }
        public int XSize { get => xSize; set => xSize = value; }
        public int YSize { get => ySize; set => ySize = value; }

        /**
         * Default Constructor 
        */
        public Boundary(string strData)
        {
            convertData(strData);
        }

        /**
         * convertData - Converts boundary string into object data
        */
        private void convertData(string strData)
        {
            int currentVal = 0;
            string currentdata = "";

            for (int i = 0; i < strData.Length; i++)
            {
                if (strData[i] == ',' || i == (strData.Length - 1))
                {
                    //end case 
                    if(currentVal == 3)
                    {
                        currentdata = currentdata + strData[i];
                    }

                    int currentIntVal = Int32.Parse(currentdata);
                    if (currentVal == 0)
                    {
                        xBegin = currentIntVal;
                    }
                    else if(currentVal == 1)
                    {
                        yBegin = currentIntVal;
                    }
                    else if(currentVal == 2)
                    {
                        xSize = currentIntVal;
                    }
                    else if(currentVal == 3)
                    {
                        ySize = currentIntVal;
                    }
                    currentVal++;
                    currentdata = "";
                }
                else
                {
                    currentdata = currentdata + strData[i];
                }
            }
        }
    }
}
