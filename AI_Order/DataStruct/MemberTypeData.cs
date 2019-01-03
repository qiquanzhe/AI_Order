using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Order.DataStruct
{
    class MemberTypeData
    {
        public int MtId;
        public String MTitle;
        public Double MDiscount;

        public MemberTypeData()
        {
        }

        public MemberTypeData(int mtId, string mTitle, double mDiscount)
        {
            MtId = mtId;
            MTitle = mTitle;
            MDiscount = mDiscount;
        }
    }
}
