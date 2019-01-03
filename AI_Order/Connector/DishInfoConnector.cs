using AI_Order.DataStruct;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AI_Order.Connector
{
    static class DishInfoConnector
    {
        /*
         * 返回所有菜系
         * */
        public static List<DishTypeData> GetDishTypeDatas()
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            List<DishTypeData> dishTypeDatas = new List<DishTypeData>();
            DishTypeData dishTypeData;
            String sql = "select * from dishtypeinfo";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                dishTypeData = new DishTypeData(mySqlDataReader.GetInt32("DtId"), mySqlDataReader.GetString("DtTitle"));
                dishTypeDatas.Add(dishTypeData);
            }
            return dishTypeDatas;
        }

        /*
         * 通过菜系（DTypeId）返回菜品
         * */
        public static List<Dish> GetDishes(int DTypeId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            List<Dish> dishes = new List<Dish>();
            String sql = "select * from dishinfo where DTypeId='" + DTypeId + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                Dish dish = new Dish(mySqlDataReader.GetInt32("DId"),
                    mySqlDataReader.GetString("DTitle"),
                    mySqlDataReader.GetInt32("DTypeId"),
                    mySqlDataReader.GetDouble("DPrice"),
                    mySqlDataReader.GetString("DChar"),
                    (byte[])mySqlDataReader["DPic"]);
                dishes.Add(dish);
            }
            return dishes;
        }

        /*
         * 通过菜名返回菜品信息
         * */
        public static Dish GetDish(String DTitle)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            Dish dish = null;
            String sql ="select * from dishinfo where DTitle='" + DTitle + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                dish = new Dish(mySqlDataReader.GetInt32("DId"),
                        mySqlDataReader.GetString("DTitle"),
                        mySqlDataReader.GetInt32("DTypeId"),
                        mySqlDataReader.GetDouble("DPrice"),
                        mySqlDataReader.GetString("DChar"),
                        (byte[])mySqlDataReader["DPic"]);
            }
            return dish;
        }

        //通过DId返回菜品信息
        public static Dish GetDish(int Did)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();
            Dish dish = null;
            String sql = "select * from dishinfo where DId=" + Did;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                dish = new Dish(mySqlDataReader.GetInt32("DId"),
                        mySqlDataReader.GetString("DTitle"),
                        mySqlDataReader.GetInt32("DTypeId"),
                        mySqlDataReader.GetDouble("DPrice"),
                        mySqlDataReader.GetString("DChar"),
                        (byte[])mySqlDataReader["DPic"]);
            }
            return dish;
        }
    }
}
