using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Order.DataStruct
{
    public class MemberInfoData
    {
        public int MbId;
        public int MtId;
        public String MbName;
        public String MbPhone;
        public double MbMoney;

        public MemberInfoData()
        {
        }

        public MemberInfoData(int mbId, int mtId, string mbName, string mbPhone, double mbMoney)
        {
            MbId = mbId;
            MtId = mtId;
            MbName = mbName;
            MbPhone = mbPhone;
            MbMoney = mbMoney;
        }
    }
}
