using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Order.DataStruct
{
    public class Dish
    {
        public int DId;
        public String DTitle;
        public int DTypeId;
        public double DPrice;
        public byte[] DPic;

        public Dish(int dId, string dTitle, int dTypeId, double dPrice,  byte[] dPic)
        {
            DId = dId;
            DTitle = dTitle;
            DTypeId = dTypeId;
            DPrice = dPrice;
            DPic = dPic;
        }
    }
}
