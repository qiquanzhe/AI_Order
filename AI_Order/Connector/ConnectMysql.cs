using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Windows;

namespace AI_Order
{
    class ConnectMysql
    {
        private string connectStr;
        private MySqlConnection conn;
        private MySqlCommand cmd;
        private MySqlDataReader reader;
        private string sql;

        public ConnectMysql()
        {
            connectStr = "server=127.0.0.1;user=root;password=123456;port=3306;database=Cater;";
            conn = new MySqlConnection(connectStr);
            conn.Open();
        }

        public int Login(string MName,string MPwd)
        {
            int type = -2;
            sql = "select MType from managerinfo where MName='" + MName + "' and MPwd='" + MPwd + "'";
            cmd = new MySqlCommand(sql, conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
                type = reader.GetInt16("MType");
            return type;
        }

        //用于添加菜谱
        public int AddMenu(string num,string name,float price, ref byte[] pic)
        {
            int score = (int)(price / 10);
            sql = "select no from userorder where no='" + num + "';";
            cmd = new MySqlCommand(sql, conn);
            if (Convert.ToInt32(cmd.ExecuteScalar()) > 0)
            {
                MessageBox.Show("The num is already exists!");
                return 1;
            }
            sql = "select no from userorder where name='" + name + "';";
            cmd = new MySqlCommand(sql, conn);
            if (Convert.ToInt16(cmd.ExecuteScalar()) > 0)
            {
                MessageBox.Show("The name is already exists!");
                return 2;
            }
            int i = 0;
            try
            {
                cmd = new MySqlCommand("insert into userorder(no,name,price,score,pic) values(@number,@name,@price,@score,@image);", conn);

                cmd.Parameters.AddWithValue("@number", num);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@score", score);
                cmd.Parameters.AddWithValue("@image", pic);
                i = cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return 3;
            }
            if (i > 0)
                return 0;
            else
                return 3;
        }

        //用于查找菜谱
        //str是传输的值，flag是信号量，0代表传值是Number，1代表传值是Name，默认是0
        public string findMenu(string str,int flag = 0)
        {
            string condition , res = "";
            if (flag == 0) condition = "no";
            else condition = "name";
            sql = "select * from userorder where " + condition + "='" + str + "';";
            cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                res += reader.GetString("no") + ";" + reader.GetString("name") + ";" + reader.GetFloat("price") + ";" + reader.GetFloat("score");
            }
            return res;
        }

        //用于删除菜谱

        public void deleteMenu(string number)
        {
            sql = "delete from userorder where no='" + number + "';";
            cmd = new MySqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            
            MessageBox.Show("Delete Record Successfully!");
        }

        //修改菜单的方法
        public void changeMenu(string number,string condition,string cvalue,byte[] bytes = null)
        {
            if (condition.Equals("name") )
            {
                sql = "update userorder set " + condition + "='" + cvalue + "' where no='" + number + "';";
                cmd = new MySqlCommand(sql, conn);
            }
            else if (condition.Equals("price"))
            {
                int score = (int)((float.Parse(cvalue)) / 10);
                sql = "update userorder set " + condition + "='" + cvalue + "',score='" + score + "'  where no='" + number + "';";
                cmd = new MySqlCommand(sql, conn);
            }
            else
            {
                cmd = new MySqlCommand("update userorder set pic=@image where no=@number;", conn);
                cmd.Parameters.AddWithValue("@image", bytes);
                cmd.Parameters.AddWithValue("@number", number);
            }

            cmd.ExecuteNonQuery();
            MessageBox.Show("Change Menu Successfully!");

        }

        //注册会员的方法
        public void registerVip(string number,int flag)
        {
            sql = "select currentscore from vip where vno='"+number+"';";
            cmd = new MySqlCommand(sql, conn);
            if (Convert.ToInt16(cmd.ExecuteScalar()) > 0)
            {
                MessageBox.Show("The vip is already exists!");
                return;
            }

            if (flag == 0)
            {
                sql = "insert into vip(vno, currentscore) values('" + number + "',0);";
            }
            else {
                sql = "insert into vip(vno, currentscore) values('" + number + "',30);";
            }


            cmd = new MySqlCommand(sql, conn);
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                MessageBox.Show("Register Successfully!");
            }
            else
                MessageBox.Show("Register Failed!");
        }

        //查询数据库中数据的条数
        public int countOrder()
        {
            sql = "select count(*) from userorder;";
            cmd = new MySqlCommand(sql, conn);
            int count = Convert.ToInt32(cmd.ExecuteScalar());
            return count;
        }

        //返回数据库数据的信息
        public orderInfo[] getOrderInfo(int number)
        {
            orderInfo[] info = new orderInfo[number];
            orderInfo oi;
            sql = "select * from userorder;";
            cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                oi = new orderInfo();
                oi.no = reader.GetString("no");
                oi.name = reader.GetString("name");
                oi.price = reader.GetFloat("price");
                oi.score = reader.GetFloat("score");
                oi.pic = (byte[])reader["pic"];
                info[i++] = oi;
                //MessageBox.Show(""+oi.name+" "+oi.no+" "+oi.price);
            }
            return info;
        }

        //修改积分的值
        //flag=0表示增加，flag=1表示减少，value变化的数量，no变化的账号
        public string modifyScore(int flag,int value,string no)
        {
            int currentscore = 0;
            sql = "select currentscore from vip where vno='" + no + "';";
            cmd = new MySqlCommand(sql, conn);
            MySqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read()){
                currentscore = reader.GetInt16("currentscore");
            }
            reader.Close();

            MessageBox.Show("" + currentscore+value+flag);
            if (flag == 0)
                sql = "update vip set currentscore=" + (currentscore + value) + " where vno='" + no + "';";
            else if (currentscore >= value)
                sql = "update vip set currentscore=" + (currentscore - value) + " where vno='" + no + "';";
            else return "no enough score!";

            cmd = new MySqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return "update score successfully";
        }
    }
}
