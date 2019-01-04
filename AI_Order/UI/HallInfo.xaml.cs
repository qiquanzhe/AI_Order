using AI_Order.Connector;
using AI_Order.DataStruct;
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
    /// HallInfo.xaml 的交互逻辑
    /// </summary>
    public partial class HallInfo : Window
    {
        public HallInfo()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//显示在屏幕中间
            InitializeComponent();
            LoadAllHalls();
        }

        /*
         * 加载全部的大厅
         * */
        private void LoadAllHalls()
        {
            int count = HallList.Items.Count;
            for (int i = 0; i < count; i++)
                HallList.Items.RemoveAt(0);

            List<HallInfoData> hallInfoDatas = HallInfoConnector.HallInfoDatas();
            foreach (HallInfoData infoData in hallInfoDatas)
            {
                ListBoxItem listBoxItem = new ListBoxItem()
                {
                    Content = infoData.HName,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                };
                HallList.Items.Add(listBoxItem);
            }
        }

        /*
        * 鼠标拖动窗口的方法
        * */
        private void WindowDrag(object sender, MouseButtonEventArgs e) => this.DragMove();

        /*
         * 添加餐厅的函数
         * 1. 判断是否为空
         * 2. 判断是否重复
         * */
        private void AddHallButton_Click(object sender, RoutedEventArgs e)
        {
            if(AddHallName.Text == "")
            {
                MessageBox.Show("未输入餐厅名");
                return;
            }

            int InsertResult = HallInfoConnector.InsertHall(AddHallName.Text);
            if(InsertResult == 1)
            {
                MessageBox.Show("插入成功");
                AddHallName.Text = "";
                LoadAllHalls();
            }
            else
            {
                MessageBox.Show("插入失败");
                return;
            }
        }

        /*
         * 删除，判断选中
         * 判断餐厅中餐桌的状态
         * */
        private void DeleteHallButton_Click(object sender, RoutedEventArgs e)
        {
            if(HallList.SelectedIndex == -1)
            {
                MessageBox.Show("未找到选中项");
                return;
            }
            ListBoxItem listBoxItem = (ListBoxItem)HallList.SelectedItem;
            String HTitle = (String)listBoxItem.Content;
            int DeleteResult = HallInfoConnector.DeleteHall(HTitle);
            if(DeleteResult == 1)
            {
                MessageBox.Show("删除成功");
                //AddHallName.Text = "";
                LoadAllHalls();
            }
            else if(DeleteResult == -1)
            {
                MessageBox.Show("餐厅未空闲");
                return;
            }
            else
            {
                MessageBox.Show("删除失败");
                return;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            TableInfo tableInfo = new TableInfo();
            tableInfo.Show();
            Close();
        }
    }
}
