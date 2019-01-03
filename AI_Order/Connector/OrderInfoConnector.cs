using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AI_Order.DataStruct;
using System.Windows;

namespace AI_Order.Connector
{
    static class OrderInfoConnector
    {
        /*
         * 向数据库中提交用的代码
         * 桌子的ID和需要提交的菜单
         * */
        public static int SubmitOrder(int Tid,List<AddOrderTmp> addOrders)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            //先将数据库已有的此桌子的菜单删除
            String sql = "delete from orderinfo where OTableId='"+Tid+"'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            mySqlCommand.ExecuteNonQuery();

            //遍历菜单，逐个插入
            foreach (AddOrderTmp addOrder in addOrders)
            {
                Dish dish = DishInfoConnector.GetDish(addOrder.DTitle);
                int count = int.Parse(addOrder.DNumber);
                double money = count * Double.Parse(addOrder.DPrice);
                sql = "insert into orderinfo(OTableId,ODid,OCount,OMoney) " +
                    "values(" + Tid + "," + dish.DId + "," + count + "," + money + ")";
                mySqlCommand = new MySqlCommand(sql, conn);
                try
                {
                    mySqlCommand.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    return -1;
                }
            }
            return 0;
        }

        /*
         * 返回数据库中该餐桌id的已有订单
         * 参数为餐桌id
         * */
        public static List<AddOrderTmp> GetOrdersByTId(int TId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<AddOrderTmp> addOrderTmps = new List<AddOrderTmp>();
            String sql = "select * from orderinfo where OTableId=" + TId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                Dish dish = DishInfoConnector.GetDish(mySqlDataReader.GetInt32("ODId"));
                AddOrderTmp order = new AddOrderTmp(dish.DTitle, dish.DPrice.ToString(), mySqlDataReader.GetInt32("OCount").ToString());
                addOrderTmps.Add(order);
            }
            return addOrderTmps;
        }

        /**
         * 删除数据库中该ID餐桌的所有订单
         * */
        public static int DeleteOrderByTId(int TId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "delete from orderinfo where OTableId='" + TId + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            
            return mySqlCommand.ExecuteNonQuery();
        }
    }
}
