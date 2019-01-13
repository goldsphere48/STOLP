using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STOLP
{
    public class Data
    {
        public static int MaxAttributes = 2;

        public double[] Attributes = new double[MaxAttributes]; 
        public int ObjClass { get; set; }
        public Data(double[] attributes, int objClass = 0)
        {
            Attributes = attributes;
            ObjClass = objClass;
        }
    }
}
