using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AI_Order.DataStruct;
using MySql.Data.MySqlClient;


namespace AI_Order.Connector
{
    static class ManagerConnector
    {
        /*
         * 登录用的函数，返回值是用户的类型
         * 返回0表示店员，返回1表示经理
         * 返回-2表示用户名或者密码错误
         * */
        public static int Login(string MName, string MPwd)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            int type = -2;
            String sql = "select MType from managerinfo where MName='" + MName + "' and MPwd='" + MPwd + "'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                type = reader.GetInt16("MType");
            return type;
        }

        /*
         * 返回所有的店员
         * */
        public static List<ManagerInfoData> GetManagerInfoDatas()
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<ManagerInfoData> managerInfoDatas = new List<ManagerInfoData>();

            String sql = "select MName,MType from managerinfo";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                ManagerInfoData managerInfoData =
                    new ManagerInfoData(
                        mySqlDataReader.GetString("MName"),
                        mySqlDataReader.GetInt32("MType")
                        );
                managerInfoDatas.Add(managerInfoData);
            }
            return managerInfoDatas;
        }

        /*
         * 新建店员的方法，默认密码123456
         * 查重
         * */
        public static int InsertManager(String MName,int MType)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "select * from managerinfo where MName='" + MName +"'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            if (mySqlDataReader.Read())
                return -1;
            mySqlDataReader.Close();

            sql = "insert into managerinfo(MName,MType,MPwd) values('" + MName + "'," + MType + ",'123456')";
            mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }

        /**
         * 修改店员类型
         * 店员名 新类型
         * */
        public static int ModifyManagerType(String MName,int newMType)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "select * from managerinfo where MName='" + MName + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            if (!mySqlDataReader.Read())
                return -1;
            mySqlDataReader.Close();

            sql = "update managerinfo set MType=" + newMType + " where MName='" + MName + "'";
            mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }

        /**
         * 删除店员
         * */
        public static int DeleteManager(String MName)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "delete from managerinfo where MName='" + MName + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }
    }
}
