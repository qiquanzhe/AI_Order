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
    /// MemberType.xaml 的交互逻辑
    /// </summary>
    public partial class MemberType : Window
    {
        private int LoginType;
        private List<MemberTypeData> memberTypeDatas;
        public MemberType(int type)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//显示在屏幕中间
            LoginType = type;
            InitializeComponent();
            LoadAllTypes();
        }

        /**
         * 加载所有的会员类型（除了普通会员，也就是id为5的会员类型）
         * */
        private void LoadAllTypes()
        {
            if (memberTypeDatas != null)
                for (int i = 0; i < memberTypeDatas.Count-1; i++)
                    MemberTypeList.Items.RemoveAt(0);
            memberTypeDatas = MemberInfoConnector.GetMemberTypeDatas();
            foreach (MemberTypeData infoData in memberTypeDatas)
            {
                if (infoData.MtId!=5)
                {
                    InfomationTable infomationTable = new InfomationTable(infoData);
                    MemberTypeList.Items.Add(infomationTable);
                }
            }
        }

        /*
         * 鼠标拖动窗口的方法
         * */
        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /**
         * 选中的时候显示在右侧的输入框中
         * */
        private void MemberTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MemberTypeList.SelectedIndex == -1)
                return;
            MemberTypeBox.Text = ((InfomationTable)MemberTypeList.SelectedItem).Left_TB.Text;
            MemberDiscountBox.Text = ((InfomationTable)MemberTypeList.SelectedItem).Right_TB.Text;
        }

        /**
         * 添加会员类型
         * 1. 检查两个输入框是否为空
         * 2. 检查折扣是否为数字
         * 3. 后台查重
         * */
        private void AddTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberTypeBox.Text == ""||MemberDiscountBox.Text == "")
            {
                MessageBox.Show("信息填写不完整");
                return;
            }

            if(!double.TryParse(MemberDiscountBox.Text,out double MDiscount)||MDiscount > 1)
            {
                MessageBox.Show("请填写正确的折扣");
                return;
            }

            int InsertTypeResult = MemberInfoConnector.InsertMemberType(MemberTypeBox.Text, MDiscount);
            if(InsertTypeResult == -1)
            {
                MessageBox.Show("已存在的类型");
                return;
            }
            else if(InsertTypeResult != 1)
            {
                MessageBox.Show("添加失败");
                return;
            }
            else
            {
                MessageBox.Show("添加成功");
                MemberTypeBox.Text = "";
                MemberDiscountBox.Text = "";
                MemberTypeList.SelectedIndex = -1;
                LoadAllTypes();
            }
        }

        /**
         * 修改会员类型的函数
         * 仅可修改折扣
         * */
        private void ModifyTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberTypeBox.Text == "" || MemberDiscountBox.Text == "")
            {
                MessageBox.Show("信息填写不完整");
                return;
            }

            if (!double.TryParse(MemberDiscountBox.Text, out double MDiscount) || MDiscount > 1)
            {
                MessageBox.Show("请填写正确的折扣");
                return;
            }

            int ModifyTypeResult = MemberInfoConnector.ModifyMemberType(MemberTypeBox.Text, MDiscount);
            if (ModifyTypeResult == -1)
            {
                MessageBox.Show("不存在的类型(仅支持修改折扣)");
                return;
            }
            else if(ModifyTypeResult != 1)
            {
                MessageBox.Show("修改失败");
                return;
            }
            else
            {
                MessageBox.Show("修改成功");
                MemberTypeBox.Text = "";
                MemberDiscountBox.Text = "";
                MemberTypeList.SelectedIndex = -1;
                LoadAllTypes();
            }
        }

        /*
         * 删除会员类型的函数（2、3、4均后台实现）
         * 1. 检查选中
         * 2. 检查是否存在
         * 3. 将已有的该类型的会员的类型id改为5（普通会员）
         * 4. 删除会员
         * */
        private void DeleteSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberTypeList.SelectedIndex == -1)
            {
                MessageBox.Show("未选中任何类型");
                return;
            }

            int DeleteTypeResult = 
                MemberInfoConnector.DeleteMemberType(((InfomationTable)MemberTypeList.SelectedItem).Left_TB.Text);

            if(DeleteTypeResult == -1)
            {
                //从界面上选择删除，一般不会不存在，这个提示出现的时候一般是逻辑出了问题
                MessageBox.Show("不存在的类型");
                return;
            }

            else
            {
                MessageBox.Show("删除成功");
                MemberTypeBox.Text = "";
                MemberDiscountBox.Text = "";
                MemberTypeList.SelectedIndex = -1;
                LoadAllTypes();
            }
        }

        /*
         * 返回，将type传回去
         * */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MemberInfo memberType = new MemberInfo(LoginType);
            memberType.Show();
            Close();
        }
    }
}
