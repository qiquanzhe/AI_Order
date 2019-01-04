using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI_Order.DataStruct;
using MySql.Data.MySqlClient;

namespace AI_Order.Connector
{
    static class HallInfoConnector
    {
        //public HallInfoData HallInfoData;

        /*
         * 返回所有的房间信息
         * */
        public static List<HallInfoData> HallInfoDatas()
        {
            List<HallInfoData> hallInfoDatas = new List<HallInfoData>();
            HallInfoData hallInfoData;
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            String sql = "select * from hallinfo";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader reader = mySqlCommand.ExecuteReader();
            while (reader.Read())
            {
                hallInfoData = new HallInfoData(reader.GetInt16("HId"), reader.GetString("HTitle"));
                hallInfoDatas.Add(hallInfoData);
            }
            reader.Close();
            return hallInfoDatas;
        }

        /*
         * 通过HId查询餐厅信息的方法
         * 在前端中并没有使用HId进行操作，所以可认为所有数据都是经过验证的数据
         * 不需要再验证是否存在传入的HId
         * */
        public static HallInfoData GetHallInfoDataByHId(int HId)
        {
            HallInfoData hallInfoData = null;
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            String sql = "select * from hallinfo where HId = '" + HId + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                hallInfoData = new HallInfoData(mySqlDataReader.GetInt16("HId"), mySqlDataReader.GetString("HTitle"));
            }
            mySqlDataReader.Close();
            return hallInfoData;
        }

        /**
         * 通过餐厅名查询餐厅信息
         * */
        public static HallInfoData GetHallInfoData(String HTitle)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            HallInfoData hallInfoData = null;
            String sql = "select * from hallinfo where HTitle='" + HTitle + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            if(mySqlDataReader.Read())
                hallInfoData = new HallInfoData(mySqlDataReader.GetInt16("HId"), mySqlDataReader.GetString("HTitle"));
            return hallInfoData;
        }
    }
}
