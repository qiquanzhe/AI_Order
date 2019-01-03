using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using AI_Order.Connector;
using AI_Order.DataStruct;

namespace AI_Order
{
    /// <summary>
    /// MainMenuWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        private int LoginType;
        private List<HallInfoData> hallInfoDatas;

        public MainMenuWindow(int type)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//显示在屏幕中间
            LoginType = type;
            InitializeComponent();
            AddNewRooms();
            RoomList.SelectedIndex = 0;
            bool isAdmin = (type == 1) ? true : false;
            DishInfoButton.IsEnabled = isAdmin;
            TableInfoButton.IsEnabled = isAdmin;
            ManagerButton.IsEnabled = isAdmin;
        }

        //动态添加房间名的方法
        private void AddNewRooms()
        {
            List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
            hallInfoDatas = HallInfoConnector.HallInfoDatas();
            foreach(HallInfoData hall in hallInfoDatas)
            {
                ListBoxItem list = new ListBoxItem();
                list.Content = hall.HName;
                listBoxItems.Add(list);
            }
            RoomList.ItemsSource = listBoxItems;
        }

        //鼠标拖动窗口的方法
        private void WindowDrag(object sender, MouseButtonEventArgs e) => this.DragMove();

        /*
         * 点击房间名的方法
         * */
        private void RoomList_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadListBox(RoomList.SelectedIndex);

        /*
         * 显示ListBox，将自定义的控件对象放进ListBox中
         * 传进来的参数是上一个ListBox的selectedIndex，是从零开始的，所以在使用的时候先＋1
         * */
        private void LoadListBox(int HId)
        {
            HId = HId + 1;
            List<TableInfoData> tableInfoDatas = TableInfoConnector.GetTableInfoDatas(HId);
            List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
            TableStatus tableStatus;
            foreach (var item in tableInfoDatas)
            {
                tableStatus = new TableStatus();
                tableStatus.TableName.Content = item.TTitle;
                tableStatus.tableStatus = item.TIsFree;
                tableStatus.tableNumber = item.TId;
                tableStatus.LockStatus.Source = tableStatus.tableStatus == 1 ? tableStatus.unlockImage : tableStatus.lockImage;
                ListBoxItem listBoxItem = new ListBoxItem();
                listBoxItem.Content = tableStatus;
                listBoxItem.MouseDoubleClick += ListBox_MouseDoubleClick;
                listBoxItems.Add(listBoxItem);
            }
            tableList.ItemsSource = listBoxItems;
        }

        /*
         * 双击餐桌也可以点餐
         * */
        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (tableList.SelectedIndex == -1)
            {
                MessageBox.Show("请先选中餐桌或者双击餐桌点餐！");
                return;
            }
            ListBoxItem listBoxItem = (ListBoxItem)tableList.SelectedItem;
            TableStatus tableStatus = (TableStatus)listBoxItem.Content;
            OrderWindow(tableStatus.tableNumber);
        }

        /*
         * 点餐按钮，先选中，如果没有选中要提示选中
         * */
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if(tableList.SelectedIndex == -1)
            {
                MessageBox.Show("请先选中餐桌或者双击餐桌点餐！");
                return;
            }
            ListBoxItem listBoxItem = (ListBoxItem)tableList.SelectedItem;
            TableStatus tableStatus = (TableStatus)listBoxItem.Content;
            
            OrderWindow(tableStatus.tableNumber);
        }

        /*
         * 点餐，需要的参数是餐桌的ID和账户类型
         * */
         private void OrderWindow(int TId)
        {
            OrderDishWindow orderDishWindow = new OrderDishWindow(TId,LoginType);
            orderDishWindow.Show();
            Close();
        }

        /*
         * 退出函数
         * */
        private void ExitButton_Click(object sender, RoutedEventArgs e) => Close();

        //买单按钮函数体
        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            if (tableList.SelectedIndex == -1)
            {
                MessageBox.Show("请先选中餐桌进行买单操作！");
                return;
            }
            ListBoxItem listBoxItem = (ListBoxItem)tableList.SelectedItem;
            TableStatus tableStatus = (TableStatus)listBoxItem.Content;
            if (tableStatus.tableStatus == 1)
            {
                MessageBox.Show("该餐桌空闲，无需买单！");
                return;
            }
            OrderPay orderPay = new OrderPay(tableStatus.tableNumber,LoginType);
            orderPay.Show();
            Close();
        }

        private void MemberButton_Click(object sender, RoutedEventArgs e)
        {
            MemberInfo memberInfo = new MemberInfo(LoginType);
            memberInfo.Show();
            Close();
        }

        private void DishInfoButton_Click(object sender, RoutedEventArgs e)
        {
            DishInfo dishInfo = new DishInfo();
            dishInfo.Show();
            Close();
        }
    }
}
