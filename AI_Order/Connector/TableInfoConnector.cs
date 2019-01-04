using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI_Order.DataStruct;
using MySql.Data.MySqlClient;

namespace AI_Order.Connector
{
    static class TableInfoConnector
    {

        /*
         * 通过HId返回该房间内的所有餐桌信息
         * */
        public static List<TableInfoData> GetTableInfoDatas(int HId)
        {
            List<TableInfoData> tableInfoDatas = new List<TableInfoData>();
            TableInfoData tableInfoData;
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            String sql = "select * from tableinfo where HId='" + HId + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql,conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                tableInfoData = new TableInfoData(mySqlDataReader.GetInt32("TId"), 
                    mySqlDataReader.GetString("TTitle"), 
                    HallInfoConnector.GetHallInfoDataByHId(HId), 
                    mySqlDataReader.GetInt16("TIsFree"));
                tableInfoDatas.Add(tableInfoData);
            }
            return tableInfoDatas;
        }

        /*
         * 通过TId返回餐桌信息
         * */
        public static TableInfoData GetTable(int TId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            String sql = "select * from tableinfo where TId='" + TId + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            mySqlDataReader.Read();
            TableInfoData tableInfo = new TableInfoData(mySqlDataReader.GetInt32("TId"),
                    mySqlDataReader.GetString("TTitle"),
                    HallInfoConnector.GetHallInfoDataByHId(mySqlDataReader.GetInt32("HId")),
                    mySqlDataReader.GetInt16("TIsFree"));
            return tableInfo;
        }

        /**
         * 通过TId修改餐桌状态
         * 传入餐桌ID和状态（1空闲，0不空闲）
         * */
        public static int ModifyStatus(int TId,int Status)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            String sql = "update tableinfo set TIsFree=" + Status+" where TId="+TId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }

        /*
         * 加载所有餐桌信息
         * */
        public static List<TableInfoData> GetTableInfoDatas()
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<TableInfoData> tableInfoDatas = new List<TableInfoData>();
            String sql = "select * from tableinfo";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                TableInfoData tableInfoData = new TableInfoData(mySqlDataReader.GetInt32("TId"),
                    mySqlDataReader.GetString("TTitle"),
                    HallInfoConnector.GetHallInfoDataByHId(mySqlDataReader.GetInt32("HId")),
                    mySqlDataReader.GetInt16("TIsFree"));
                tableInfoDatas.Add(tableInfoData);
            }
            return tableInfoDatas;
        }

        
         //模糊查询餐桌信息
        public static List<TableInfoData> GetTableInfoDatas(String TTitle,int HId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<TableInfoData> tableInfoDatas = new List<TableInfoData>();

            String sql = "select * from tableinfo where TTitle like '" + TTitle + "%'" +
                (HId == 0 ? "" : ("and HId=" + HId));
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                TableInfoData tableInfoData = new TableInfoData(mySqlDataReader.GetInt32("TId"),
                    mySqlDataReader.GetString("TTitle"),
                    HallInfoConnector.GetHallInfoDataByHId(mySqlDataReader.GetInt32("HId")),
                    mySqlDataReader.GetInt16("TIsFree"));
                tableInfoDatas.Add(tableInfoData);
            }

            return tableInfoDatas;
        }

        /*
         * 数据库中添加餐桌信息
         * */
        public static int InsertTable(String TTitle,int HId,int TIsFree)
        {
            List<TableInfoData> tableInfoDatas = GetTableInfoDatas(TTitle,HId);
            if (tableInfoDatas.Count > 0)
                return -1;

            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "insert into tableinfo(TTitle,Hid,TIsFree) values('" + TTitle + "'," + HId + "," + TIsFree + ")";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            return mySqlCommand.ExecuteNonQuery();
        }

        /*
         * 修改餐桌状态
         * */
        public static int ModifyTableStatus(String TTitle,int HId,int TIsFree)
        {
            List<TableInfoData> tableInfoDatas = GetTableInfoDatas(TTitle, HId);
            if (tableInfoDatas.Count == 0)
                return -1;

            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "update tableinfo set TIsFree=" + TIsFree + " where TTitle='" + TTitle + "' and HId=" + HId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            return mySqlCommand.ExecuteNonQuery();
        }

        /*
         * 删除餐桌
         * */
        public static int DeleteTable(String TTitle, int HId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "delete from tableinfo where TTitle='" + TTitle + "' and HId=" + HId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            return mySqlCommand.ExecuteNonQuery();
        }
    }
}
