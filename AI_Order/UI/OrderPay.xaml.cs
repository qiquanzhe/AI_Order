using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using AI_Order.DataStruct;
using AI_Order.Connector;
using System.Windows.Controls;
using AI_Order.UI;

namespace AI_Order
{
    /// <summary>
    /// OrderPay.xaml 的交互逻辑
    /// </summary>
    public partial class OrderPay : Window
    {
        private int TId;
        private MemberInfoData MemberInfo = null;
        private MemberTypeData MemberType = null;
        private double summaryMoney = 0;
        private double shouldPay = 0;
        private double leftMoney = 0;
        private int loginType;
        public OrderPay(int TId,int LoginType)
        {            
            //显示在屏幕中间
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.TId = TId;
            InitializeComponent();
            LoadOrderList();
            TableNumber.Content = TableInfoConnector.GetTable(TId).hall.HName + TableInfoConnector.GetTable(TId).TTitle + "桌";
            this.loginType = LoginType;
        }

        private void LoadOrderList()
        {
            List<AddOrderTmp> addOrders = OrderInfoConnector.GetOrdersByTId(TId);
            foreach (AddOrderTmp order in addOrders)
            {
                PayInfo orderContainer = new PayInfo(order);
                ListBoxItem item = new ListBoxItem();
                item.Content = orderContainer;
                ListToPay.Items.Add(item);
                summaryMoney += double.Parse(order.DPrice) * int.Parse(order.DNumber);
            }
            summaryLabel.Content = summaryMoney;
            shouldPayLabel.Content = summaryMoney;
            shouldPay = summaryMoney;
        }

        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (IsVIP.IsChecked == true)
            {
                VIPGroup.IsEnabled = true;
            }
            else
            {
                VIPGroup.IsEnabled = false;
                shouldPayLabel.Content = summaryMoney;
                shouldPay = summaryMoney;
                MemberInfo = null;
                MemberType = null;
            }
        }


        private void MemberPhone_LostFocus(object sender, RoutedEventArgs e) => LoadVIP();

        private void MemberName_LostFocus(object sender, RoutedEventArgs e) => LoadVIP();

        /**
         * 对VIP账号信息进行模糊查询
         * */
        private void LoadVIP()
        {
            if (MemberPhone.Text == "" && MemberName.Text == "")
                return;
            String MbPhone = MemberPhone.Text;
            String MbName = MemberName.Text;
            List<MemberInfoData> memberInfoDatas = MemberInfoConnector.GetMembers(MbPhone, MbName);
            if(memberInfoDatas.Count > 1)
            {
                MessageBox.Show("相似项过多，请详细输入！");
                return;
            }
            else if(memberInfoDatas.Count == 0)
            {
                MessageBox.Show("未查找到相关会员信息！");
                return;
            }
            else
            {
                MemberInfo = memberInfoDatas[0];
                MemberMoney.Content = "￥" + MemberInfo.MbMoney;     //加载账户余额
                MemberType = MemberInfoConnector.GetMemberType(MemberInfo.MbId);    //获得账户会员类型
                MemberTypeLabel.Content = MemberType.MTitle;
                MemberDiscountLabel.Content = MemberType.MDiscount;//折扣
                shouldPayLabel.Content = summaryMoney * MemberType.MDiscount;
                shouldPay = summaryMoney * MemberType.MDiscount;
                leftMoney = MemberInfo.MbMoney;
            }
        }

        //使用余额时改变显示和变量
        private void UseMoney_Click(object sender, RoutedEventArgs e)
        {
            if(useMoney.IsChecked == true)
            {
                if (shouldPay < MemberInfo.MbMoney)
                {
                    leftMoney = MemberInfo.MbMoney - shouldPay;
                    shouldPay = 0;
                    shouldPayLabel.Content = shouldPay;
                    MemberMoney.Content = "￥"+leftMoney;
                }
                else
                {
                    shouldPay -= MemberInfo.MbMoney;
                    leftMoney = 0;
                    shouldPayLabel.Content = shouldPay;
                    MemberMoney.Content = leftMoney;
                }
            }
            else
            {
                shouldPay = summaryMoney * MemberType.MDiscount;
                leftMoney = MemberInfo.MbMoney;
                shouldPayLabel.Content = shouldPay;
                MemberMoney.Content = leftMoney;
            }
        }

        /**
         * 结账按钮的实现，提交到数据库
         * 1. 改变账号余额
         * 2. 改变桌子状态为空闲
         * 3. 删除该桌子的订单
         * */
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberInfo != null)
                MemberInfoConnector.ModifyMoney(MemberInfo.MbId, leftMoney);
            TableInfoConnector.ModifyStatus(TId, 1);
            OrderInfoConnector.DeleteOrderByTId(TId);
            MessageBox.Show("结账完成！");
            MainMenuWindow mainMenuWindow = new MainMenuWindow(loginType);
            mainMenuWindow.Show();
            Close();
        }

        /*
         * 关闭此窗口，开启上一个窗口，把type传回去
         * */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(loginType);
            mainMenuWindow.Show();
            Close();
        }
    }
}
