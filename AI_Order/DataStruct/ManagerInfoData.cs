using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Order.DataStruct
{
    public class ManagerInfoData
    {
        public String MName;
        public int MType;

        public ManagerInfoData() { }

        public ManagerInfoData(string mName, int mType)
        {
            MName = mName;
            MType = mType;
        }
    }
}
