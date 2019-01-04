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

        /*
         * 添加餐厅
         * */
        public static int InsertHall(String HTitle)
        {
            if (GetHallInfoData(HTitle) != null)
                return -1;

            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "insert into hallinfo(HTitle) values('" + HTitle + "')";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            return mySqlCommand.ExecuteNonQuery();
        }

        /**
         * 删除餐厅，先检查当前餐厅所有桌子的状态，如果有非空闲桌子则不能删除
         * */
        public static int DeleteHall(String HTitle)
        {
            List<TableInfoData> tableInfoDatas =
                TableInfoConnector.GetTableInfoDatas(GetHallInfoData(HTitle).HId);
            foreach (TableInfoData tableInfoData in tableInfoDatas)
            {
                if (tableInfoData.TIsFree == 0)
                {
                    return -1;
                }
            }

            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            //先删除所有桌子
            String sql = "delete from tableinfo where HId=" + GetHallInfoData(HTitle).HId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            mySqlCommand.ExecuteNonQuery();

            sql = "delete from hallinfo where HTitle='" + HTitle + "'";
            mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }
    }
}
