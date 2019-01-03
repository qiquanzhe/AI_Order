using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using AI_Order.DataStruct;

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
    }
}
