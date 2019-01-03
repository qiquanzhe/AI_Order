using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
