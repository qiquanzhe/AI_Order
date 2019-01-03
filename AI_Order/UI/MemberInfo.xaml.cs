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
            if(memberInfoDatas != null)
            for (int i = 0; i < memberInfoDatas.Count; i++)
                MemberList.Items.RemoveAt(0);
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

        //选中会员，加载到界面中
        private void MemberList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBoxItem listBoxItem = (ListBoxItem)MemberList.SelectedItem;
            if (listBoxItem == null)
            {
                return;
            }
            PayInfo payInfo = (PayInfo)listBoxItem.Content;
            MemberTypes.SelectedIndex = MemberInfoConnector.GetMemberType(payInfo.DName.Text).MtId - 1;
            MemberName.Text = payInfo.DPrice.Text;
            MemberMoney.Text = payInfo.DNumber.Text;
            MemberPhone.Text = payInfo.DSum.Text;
        }

        //正则验证手机号
        private bool IsMobilePhone(string input)
        {
            if (input.Length == 0)
            {
                return false;
            }
            else if (input.Length != 11)
            {
                return false;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[1]+[3,4,5,7,8]+\d{9}");
        }

        /**
         * 添加会员事件：
         * 验证不为空、验证手机号和余额、后台验证重复
         * 返回-1表示重复、返回1表示成功、返回其他值插入失败
         * */
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if(MemberName.Text == ""||MemberPhone.Text == ""||MemberMoney.Text == "")
            {
                MessageBox.Show("信息未填写完整");
                return;
            }

            if(!IsMobilePhone(MemberPhone.Text))
            {
                MessageBox.Show("手机号码填写不正确");
                return;
            }

            if (!Double.TryParse(MemberMoney.Text, out double MbMoney))
            {
                MessageBox.Show("余额格式不正确");
                return;
            }

            int IsInserted =
                MemberInfoConnector.InsertMember(MemberTypes.SelectedIndex+1,MemberName.Text, MemberPhone.Text, MbMoney);

            if(IsInserted == -1)
            {
                MessageBox.Show("已存在的会员");
                return;
            }
            else if(IsInserted != 1)
            {
                MessageBox.Show("插入失败");
                return;
            }
            else
            {
                MessageBox.Show("添加成功");
                MemberList.SelectedIndex = -1;
                MemberName.Text = "";
                MemberPhone.Text = "";
                MemberMoney.Text = "";
                MemberTypes.SelectedIndex = 0;
                LoadAllMember();
            }
        }

        /**
         * 修改会员事件：
         * 验证不为空、验证手机号和余额、后台验证会员是否存在
         * 返回-1表示不存在、返回1表示成功、返回其他值修改失败
         * 仅可修改余额和会员类型，手机号和姓名做会员的唯一标志
         * */
        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberName.Text == "" || MemberPhone.Text == "" || MemberMoney.Text == "")
            {
                MessageBox.Show("信息未填写完整");
                return;
            }

            if (!IsMobilePhone(MemberPhone.Text))
            {
                MessageBox.Show("手机号码填写不正确");
                return;
            }

            if (!Double.TryParse(MemberMoney.Text, out double MbMoney))
            {
                MessageBox.Show("余额格式不正确");
                return;
            }

            int ModifiedMember = 
                MemberInfoConnector.ModifyMember(MemberTypes.SelectedIndex + 1, MemberName.Text, MemberPhone.Text, MbMoney);
            if(ModifiedMember == -1)
            {
                MessageBox.Show("该会员不存在(仅可更改会员类型和余额)");
                return;
            }
            if (ModifiedMember != 1)
            {
                MessageBox.Show("修改失败");
                return;
            }

            else
            {
                MessageBox.Show("修改成功");
                MemberList.SelectedIndex = -1;
                MemberName.Text = "";
                MemberPhone.Text = "";
                MemberMoney.Text = "";
                MemberTypes.SelectedIndex = 0;
                LoadAllMember();
            }
        }

        /**
         * 删除会员：
         * 判断是否选中，没有选中直接返回
         * 对选中的值进行提取，删除数据库中的数据
         * */
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if(MemberList.SelectedIndex == -1)
            {
                MessageBox.Show("请先选中会员信息！");
                return;
            }
            ListBoxItem listBoxItem = (ListBoxItem)MemberList.SelectedItem;
            PayInfo payInfo = (PayInfo)listBoxItem.Content;
            /*MemberTypes.SelectedIndex = MemberInfoConnector.GetMemberType(payInfo.DName.Text).MtId - 1;
            MemberName.Text = payInfo.DPrice.Text;
            MemberMoney.Text = payInfo.DNumber.Text;
            MemberPhone.Text = payInfo.DSum.Text;*/

            int IsDeleted = 
                MemberInfoConnector.DeleteMember(payInfo.DPrice.Text, payInfo.DSum.Text);

            if(IsDeleted != 1)
            {
                MessageBox.Show("删除失败");
                return;
            }
            else
            {
                MessageBox.Show("删除成功");
                MemberList.SelectedIndex = -1;
                MemberName.Text = "";
                MemberPhone.Text = "";
                MemberMoney.Text = "";
                MemberTypes.SelectedIndex = 0;
                LoadAllMember();
            }

        }


        private void TypeWindow_Click(object sender, RoutedEventArgs e)
        {
            MemberType memberType = new MemberType(Type);
            memberType.Show();
            Close();
        }
    }
}
