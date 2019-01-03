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
    static class MemberInfoConnector
    {
        /**
         * 通过手机号和姓名获取会员信息
         * */
        public static List<MemberInfoData> GetMembers(String MbPhone,String MbName)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            MbPhone += "%";
            MbName += "%";
            List<MemberInfoData> memberInfoDatas = new List<MemberInfoData>();
            String sql = "select * from memberinfo where MbPhone like '" + MbPhone + "' and MbName like '"+MbName+"'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while (mySqlDataReader.Read())
            {
                MemberInfoData memberInfoData = new MemberInfoData(
                    mySqlDataReader.GetInt32("MbId"),
                    mySqlDataReader.GetInt32("MtId"),
                    mySqlDataReader.GetString("MbName"),
                    mySqlDataReader.GetString("MbPhone"),
                    mySqlDataReader.GetDouble("MbMoney"));
                memberInfoDatas.Add(memberInfoData);
            }
            return memberInfoDatas;
        }

        /**
         * 通过MtId获取会员类型
         * */
        public static MemberTypeData GetMemberType(int MtId)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "select * from membertypeinfo where MtId=" + MtId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            MemberTypeData memberTypeData = null;
            if (mySqlDataReader.Read())
            {
                //int mtId, string mTitle, double mDiscount
                memberTypeData = new MemberTypeData(
                    mySqlDataReader.GetInt32("MtId"),
                    mySqlDataReader.GetString("MTitle"),
                    mySqlDataReader.GetDouble("MDiscount")
                    );
            }
            return memberTypeData;
        }

        /**
         * 更改会员的余额
         * 参数：mbid会员id，money改变之后的余额
         * */
        public static int ModifyMoney(int MbId,double money)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "update memberinfo set MbMoney=" + money+" where MbId="+MbId;
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }

        /**
         * 查找数据库中所有会员
         * */
        public static List<MemberInfoData> GetMembers()
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<MemberInfoData> memberInfoDatas = new List<MemberInfoData>();

            String sql = "select * from memberinfo";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();

            while (mySqlDataReader.Read())
            {
                //int mbId, int mtId, string mbName, string mbPhone, double mbMoney
                MemberInfoData memberInfoData = new MemberInfoData(
                    mySqlDataReader.GetInt32("MbId"),
                    mySqlDataReader.GetInt32("MtId"),
                    mySqlDataReader.GetString("MbName"),
                    mySqlDataReader.GetString("MbPhone"),
                    mySqlDataReader.GetDouble("MbMoney")
                    );
                memberInfoDatas.Add(memberInfoData);
            }

            return memberInfoDatas;
        }

        /**
         * 查询数据库中所有的会员类型
         * */
        public static List<MemberTypeData> GetMemberTypeDatas()
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            List<MemberTypeData> memberTypeDatas = new List<MemberTypeData>();

            String sql = "select * from membertypeinfo";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            while(mySqlDataReader.Read())
            {
                //int mtId, string mTitle, double mDiscount
                MemberTypeData memberTypeData = new MemberTypeData(
                    mySqlDataReader.GetInt32("MtId"),
                    mySqlDataReader.GetString("MTitle"),
                    mySqlDataReader.GetDouble("MDiscount")
                    );
                memberTypeDatas.Add(memberTypeData);
            }
            return memberTypeDatas;
        }

        /*
         * 根据会员名返回会员类型
         * */
        public static MemberTypeData GetMemberType(String MTitle)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            MemberTypeData memberTypeData = null;

            String sql = "select * from membertypeinfo where MTitle='" + MTitle + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            if (mySqlDataReader.Read())
            {
                memberTypeData = new MemberTypeData(
                    mySqlDataReader.GetInt32("MtId"),
                    mySqlDataReader.GetString("MTitle"),
                    mySqlDataReader.GetDouble("MDiscount")
                    );
            }
            return memberTypeData;
        }

        /**
         * 向数据库中添加会员，先检查是否存在该手机号和姓名的组合
         * */
        public static int InsertMember(int MtId,String MbName,String MbPhone,Double MbMoney)
        {
            List<MemberInfoData> memberInfoDatas = GetMembers(MbPhone, MbName);
            if (memberInfoDatas.Count > 0)
                return -1;

            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "insert into memberinfo(MtId,MbName,MbPhone,MbMoney) values(" +
                MtId + ",'" + MbName + "','" + MbPhone + "'," + MbMoney+")";
            //MessageBox.Show(sql);
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
            //return 1;
        }

        /**
         * 修改会员，只能修改会员余额和会员类型
         * */
        public static int ModifyMember(int MtId, String MbName, String MbPhone, Double MbMoney)
        {
            List<MemberInfoData> memberInfoDatas = GetMembers(MbPhone, MbName);
            if (memberInfoDatas.Count == 0)
                return -1;

            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "update memberinfo set MtId=" + MtId + ",MbMoney=" + MbMoney +
                " where MbPhone='" + MbPhone + "' and MbName='" + MbName + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);

            return mySqlCommand.ExecuteNonQuery();
        }

        /**
         * 删除会员
         * */
        public static int DeleteMember(String MbName, String MbPhone)
        {
            String connectStr = ConnectorInfo.connectStr;
            MySqlConnection conn = new MySqlConnection(connectStr);
            conn.Open();

            String sql = "delete from memberinfo where MbName='" + MbName + "' and MbPhone='" + MbPhone + "'";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, conn);
            return mySqlCommand.ExecuteNonQuery();
        }
    }
}
