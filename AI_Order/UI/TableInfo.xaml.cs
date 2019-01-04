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
    /// TableInfo.xaml 的交互逻辑
    /// </summary>
    public partial class TableInfo : Window
    {
        private List<TableInfoData> tableInfoDatas;
        public TableInfo()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//显示在屏幕中间
            InitializeComponent();
            LoadAllTables();
            LoadAllHalls();
        }

        /*
         * 加载餐桌信息
         * */
        private void LoadAllTables()
        {
            int count =  TableList.Items.Count;
            for (int i = 0; i < count; i++)
                TableList.Items.RemoveAt(0);
            tableInfoDatas = TableInfoConnector.GetTableInfoDatas();
            foreach (TableInfoData table in tableInfoDatas)
            {
                InformationTable3 informationTable3 = new InformationTable3();
                informationTable3.Left_TB.Text = table.TTitle;
                informationTable3.Mid_TB.Text = table.hall.HName;
                informationTable3.Right_TB.Text = table.TIsFree == 1 ? "是" : "否";
                TableList.Items.Add(informationTable3);
            }
        }

        /**
         * 加载所有的大厅
         * */
        private void LoadAllHalls()
        {
            List<HallInfoData> hallInfoDatas = HallInfoConnector.HallInfoDatas();
            foreach (HallInfoData hall in hallInfoDatas)
            {
                SearchHallCombo.Items.Add(hall.HName);
                ModifyHallCombo.Items.Add(hall.HName);
            }
            SearchHallCombo.SelectedIndex = 0;
            ModifyHallCombo.SelectedIndex = 0;
        }

        /*
         * 鼠标拖动窗口的方法
         * */
        private void WindowDrag(object sender, MouseButtonEventArgs e) => this.DragMove();

        /**
         * 模糊查询餐桌信息
         * */
        private void FindSimTable()
        {
            List<TableInfoData> tableInfoDatas =
                TableInfoConnector.GetTableInfoDatas(SearchTableName.Text, SearchHallCombo.SelectedIndex);
            int count = TableList.Items.Count;
            for (int i = 0; i < count; i++)
                TableList.Items.RemoveAt(0);
            foreach (TableInfoData table in tableInfoDatas)
            {
                InformationTable3 informationTable3 = new InformationTable3();
                informationTable3.Left_TB.Text = table.TTitle;
                informationTable3.Mid_TB.Text = table.hall.HName;
                informationTable3.Right_TB.Text = table.TIsFree == 1 ? "是" : "否";
                TableList.Items.Add(informationTable3);
            }
        }

        /*
         * 显示全部餐桌信息
         * */
        private void ShowAllDishesButton_Click(object sender, RoutedEventArgs e) => LoadAllTables();

        /*
         * 选中餐桌显示在右侧
         * */
        private void TableList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableList.SelectedIndex == -1)
                return;
            InformationTable3 informationTable3 = (InformationTable3)(TableList.SelectedItem);
            ModifyTableName.Text = informationTable3.Left_TB.Text;
            ModifyHallCombo.SelectedIndex = HallInfoConnector.GetHallInfoData(informationTable3.Mid_TB.Text).HId - 1;
            if (informationTable3.Right_TB.Text == "是")
                FreeRadio.IsChecked = true;
            else
                NonFreeRadio.IsChecked = false;
        }

        //搜索输入框文字发生改变时进行模糊查询
        private void SearchTableName_TextChanged(object sender, TextChangedEventArgs e) => FindSimTable();

        //搜索的餐厅发生改变时进行模糊查询
        private void SearchHallCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => FindSimTable();

        /*
         * 添加餐桌
         * 1. 餐桌名不能是空
         * 2. 同餐厅下餐桌名不能重复
         * 3. 未选择状态，默认空闲
         * */
        private void AddTableButton_Click(object sender, RoutedEventArgs e)
        {
            if(ModifyTableName.Text=="")
            {
                MessageBox.Show("未填写餐桌名");
                return;
            }
            int TIsFree;
            if (FreeRadio.IsChecked == true || (FreeRadio.IsChecked == false && NonFreeRadio.IsChecked == false))
                TIsFree = 1;
            else
                TIsFree = 0;
            int InsertResult = TableInfoConnector.InsertTable(ModifyTableName.Text, ModifyHallCombo.SelectedIndex + 1, TIsFree);
            if(InsertResult == -1)
            {
                MessageBox.Show("已存在的餐桌");
                return;
            }
            if(InsertResult == 1)
            {
                MessageBox.Show("插入成功");
                ModifyTableName.Text = "";
                ModifyHallCombo.SelectedIndex = 0;
                LoadAllTables();
            }
            else
            {
                MessageBox.Show("插入失败");
            }
        }

        /*
         * 修改事件，只可以对餐桌状态进行改变
         * */
        private void ModifyTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (ModifyTableName.Text == "")
            {
                MessageBox.Show("未填写餐桌名");
                return;
            }

            if(FreeRadio.IsChecked==false&&NonFreeRadio.IsChecked == false)
            {
                MessageBox.Show("未选择状态");
                return;
            }

            int TIsFree = FreeRadio.IsChecked==true ? 1 : 0;
            int ModifyResult = 
                TableInfoConnector.ModifyTableStatus(ModifyTableName.Text, ModifyHallCombo.SelectedIndex + 1, TIsFree);
            if(ModifyResult == -1)
            {
                MessageBox.Show("不存在的餐桌");
            }
            if(ModifyResult == 1)
            {
                MessageBox.Show("修改成功");
                ModifyTableName.Text = "";
                ModifyHallCombo.SelectedIndex = 0;
                LoadAllTables();
            }
            else
            {
                MessageBox.Show("修改失败");
            }
        }

        /*
         * 删除餐桌
         * 1. 查看选中状态
         * 2. 查看餐桌空闲状态，空闲则可删
         * 3. 删除
         * */
        private void DeleteTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (TableList.SelectedIndex == -1)
            {
                MessageBox.Show("未选中餐桌");
                return;
            }

            if(((InformationTable3)TableList.SelectedItem).Right_TB.Text == "是")
            {
                int DeleteResult =
                    TableInfoConnector.DeleteTable(((InformationTable3)TableList.SelectedItem).Left_TB.Text,
                        HallInfoConnector.GetHallInfoData(((InformationTable3)TableList.SelectedItem).Mid_TB.Text).HId);
                if (DeleteResult == 1)
                {
                    MessageBox.Show("删除成功");
                    ModifyTableName.Text = "";
                    ModifyHallCombo.SelectedIndex = 0;
                    LoadAllTables();
                }
            }
            else
            {
                MessageBox.Show("非空闲的餐桌无法删除");
                return;
            }
        }

        /*
         * 返回按钮
         * */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(1);
            mainMenuWindow.Show();
            Close();
        }

        //打开餐厅管理界面
        private void HallInfoButton_Click(object sender, RoutedEventArgs e)
        {
            HallInfo hallInfo = new HallInfo();
            hallInfo.Show();
            Close();
        }
    }
}
