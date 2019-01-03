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

        //返回所有菜品信息
        public static List<Dish> GetDishes()
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<Dish> dishes = new List<Dish>();

            String sql = "select * from dishinfo";
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

        //通过菜系ID返回菜系
        public static DishTypeData GetTypeData(int DtId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "select * from dishtypeinfo where DtId="+DtId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            mySqlDataReader.Read();
            DishTypeData dish = new DishTypeData(DtId, mySqlDataReader.GetString("DtTitle"));
            return dish;
        }

        /*
         * 模糊查询菜品信息数据
         * 需要两个参数，菜系id和菜品名字（模糊）
         * 菜系的id传过来的值可能是0，因为是从combobox中传过来的
         * 传值为0时表示全部菜系内查找
         * */
        public static List<Dish> GetDishes(int DtId,String DTitle)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<Dish> dishes = new List<Dish>();

            String sql = "select * from dishinfo where DTitle like '" + DTitle + "%'" +
                (DtId == 0 ? "" : ("and DTypeId="+DtId));
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
         * 修改价格
         * 菜名，菜系，价格
         * */
        public static int ModifyDishPrice(String DTitle,int DTypeId,double DPrice)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "update dishinfo set DPrice=" + DPrice + " where DTitle='" + DTitle + "' and DTypeId=" + DTypeId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }

        /**
         * 删除菜品信息
         * */
        public static int DeleteDish(String DTitle,int DTypeId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "delete from dishinfo where DTitle='" + DTitle + "' and DTypeId=" + DTypeId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            return mySqlCommand.ExecuteNonQuery();
        }
    }
}
