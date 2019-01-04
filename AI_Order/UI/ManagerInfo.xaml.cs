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
    /// ManagerInfo.xaml 的交互逻辑
    /// </summary>
    public partial class ManagerInfo : Window
    {
        public ManagerInfo()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//显示在屏幕中间
            InitializeComponent();
            LoadAllManagers();
        }

        /*
         *  加载所有的店员
         * */
        private void LoadAllManagers()
        {
            int count = ManagerList.Items.Count;
            for (int i = 0; i < count; i++)
                ManagerList.Items.RemoveAt(0);

            List<ManagerInfoData> managerInfoDatas = ManagerConnector.GetManagerInfoDatas();
            foreach (ManagerInfoData manager in managerInfoDatas)
            {
                InfomationTable infomationTable = new InfomationTable(manager);
                ManagerList.Items.Add(infomationTable);
            }
        }
        /*
         * 鼠标拖动窗口的方法
         * */
        private void WindowDrag(object sender, MouseButtonEventArgs e) => this.DragMove();

        /*
         * 添加店员或经理
         * 默认密码123456不可更改
         * 判空，查重
         * */
        private void AddMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if(MemberName.Text == "")
            {
                MessageBox.Show("员工名不能为空");
                return;
            }
            if(Assistants.IsChecked == false&&Manager.IsChecked == false)
            {
                MessageBox.Show("请选择店员类型");
                return;
            }

            int result = ManagerConnector.InsertManager(MemberName.Text, (Assistants.IsChecked == true) ? 0 : 1);
            if(result == 1)
            {
                MessageBox.Show("插入成功");
                MemberName.Text = "";
                LoadAllManagers();
            }
            else if(result == -1)
            {
                MessageBox.Show("已存在的店员");
                return;
            }
            else
            {
                MessageBox.Show("插入失败");
                return;
            }
        }

        /**
         * 仅支持修改类型
         * 判空，判存在
         * */
        private void ModifyMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if (MemberName.Text == "")
            {
                MessageBox.Show("员工名不能为空");
                return;
            }
            if (Assistants.IsChecked == false && Manager.IsChecked == false)
            {
                MessageBox.Show("请选择店员类型");
                return;
            }

            int ModifyResult = ManagerConnector.ModifyManagerType(MemberName.Text, (Assistants.IsChecked == true) ? 0 : 1);
            if (ModifyResult == 1)
            {
                MessageBox.Show("修改成功");
                MemberName.Text = "";
                LoadAllManagers();
            }
            else if (ModifyResult == -1)
            {
                MessageBox.Show("不存在的店员");
                return;
            }
            else
            {
                MessageBox.Show("修改失败");
                return;
            }
        }

        /*
         * 删除，判选
         * */
        private void DeleteMemberButton_Click(object sender, RoutedEventArgs e)
        {
            if(ManagerList.SelectedIndex == -1)
            {
                MessageBox.Show("没有选中员工");
                return;
            }
            int DeleteResult = ManagerConnector.DeleteManager(((InfomationTable)ManagerList.SelectedItem).Left_TB.Text);
            if (DeleteResult == 1)
            {
                MessageBox.Show("删除成功");
                MemberName.Text = "";
                LoadAllManagers();
            }
            else
            {
                MessageBox.Show("修改失败");
                return;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(1);
            mainMenuWindow.Show();
            Close();
        }
    }
}
