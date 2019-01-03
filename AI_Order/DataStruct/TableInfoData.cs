using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_Order.DataStruct
{
    class TableInfoData
    {
        public int TId;
        public String TTitle;
        public HallInfoData hall;
        public int TIsFree;

        public TableInfoData(int id, String title, HallInfoData hall, int isFree)
        {
            TId = id;
            TTitle = title;
            this.hall = hall;
            TIsFree = isFree;
        }
    }
}
