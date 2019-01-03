using AI_Order.Connector;
using AI_Order.DataStruct;
using AI_Order.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AI_Order
{
    /// <summary>
    /// MemberInfo.xaml 的交互逻辑
    /// </summary>
    public partial class MemberInfo : Window
    {
        private int Type;
        private List<MemberInfoData> memberInfoDatas;
        private List<MemberTypeData> memberTypeDatas;
        private MemberInfoData MemberInfoData;

        public MemberInfo(int type)
        {
            //记录当前登录的账户的类型
            this.Type = type;

            //窗口在屏幕居中显示
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            //初始化组件
            InitializeComponent();

            //加载所有会员
            LoadAllMember();

            //管理员有管理会员类型的资格
            if (Type == 1) typeWindow.IsEnabled = true;

            LoadMemberTypes();
            MemberTypes.SelectedIndex = 0;
        }

        /*
         * 鼠标拖动窗口的方法
         * */
        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /**
         * 加载所有会员的方法
         * */
        private void LoadAllMember()
        {
            memberInfoDatas = MemberInfoConnector.GetMembers();
            foreach (MemberInfoData infoData in memberInfoDatas)
            {
                /*
                 * 没有给它写专门的控件，把买单的拿来用了
                 * 对应关系：
                 * DName    --会员类型
                 * DPrice   --姓名
                 * DNumber  --余额
                 * DSum     --手机号
                 * */
                MemberTypeData memberTypeData = MemberInfoConnector.GetMemberType(infoData.MtId);
                ListBoxItem listBoxItem = new ListBoxItem();
                PayInfo info = new PayInfo();
                info.DName.Text = memberTypeData.MTitle;
                info.DPrice.Text = infoData.MbName;
                info.DNumber.Text = infoData.MbMoney.ToString();
                info.DSum.Text = infoData.MbPhone;
                listBoxItem.Content = info;
                MemberList.Items.Add(listBoxItem);
            }
        }

        /**
         * 实现模糊查询，通过手机号和姓名
         * */
        private void LoadMember()
        {
            if(SearchName.Text == ""&&SearchPhone.Text == "")
                return;
            for (int i = 0; i < memberInfoDatas.Count; i++)
                MemberList.Items.RemoveAt(0);
            String MbPhone = SearchPhone.Text;
            String MbName = SearchName.Text;
            memberInfoDatas = MemberInfoConnector.GetMembers(MbPhone, MbName);
            foreach (MemberInfoData infoData in memberInfoDatas)
            {
                ListBoxItem listBoxItem = new ListBoxItem();
                PayInfo info = new PayInfo();
                MemberTypeData memberTypeData = MemberInfoConnector.GetMemberType(infoData.MtId);
                info.DName.Text = memberTypeData.MTitle;
                info.DPrice.Text = infoData.MbName;
                info.DNumber.Text = infoData.MbMoney.ToString();
                info.DSum.Text = infoData.MbPhone;
                listBoxItem.Content = info;
                MemberList.Items.Add(listBoxItem);
            }
        }

        private void SearchPhone_TextChanged(object sender, TextChangedEventArgs e) => LoadMember();

        private void SearchName_TextChanged(object sender, TextChangedEventArgs e) => LoadMember();

        private void DisplayButton_Click(object sender, RoutedEventArgs e) => LoadAllMember();

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(Type);
            mainMenuWindow.Show();
            Close();
        }

        /*
         * 加载会员类型到ComboBox中
         * */
        private void LoadMemberTypes()
        {
            memberTypeDatas = MemberInfoConnector.GetMemberTypeDatas();
            foreach (MemberTypeData item in memberTypeDatas)
            {
                ComboBoxItem comboBoxItem = new ComboBoxItem();
                comboBoxItem.Content = item.MTitle;
                MemberTypes.Items.Add(comboBoxItem);
            }
        }

        private void MemberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)MemberList.SelectedItem;
            PayInfo payInfo = (PayInfo)listBoxItem.Content;
            MemberTypes.SelectedIndex = MemberInfoConnector.GetMemberType(payInfo.DName.Text).MtId - 1;
            MemberName.Text = payInfo.DPrice.Text;
            MemberMoney.Text = payInfo.DNumber.Text;
            MemberPhone.Text = payInfo.DSum.Text;
        }
    }
}
