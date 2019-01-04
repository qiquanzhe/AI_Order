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
    /// DishType.xaml 的交互逻辑
    /// </summary>
    public partial class DishType : Window
    {
        List<DishTypeData> dishTypeDatas;
        public DishType()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadAllDishType();
        }

        /*
         * 加载所有菜系
         * */
         private void LoadAllDishType()
        {
            int count = DishTypeList.Items.Count;
            for (int i = 0; i < count; i++)
                DishTypeList.Items.RemoveAt(0);
            dishTypeDatas = DishInfoConnector.GetDishTypeDatas();
            foreach (DishTypeData typeData in dishTypeDatas)
            {
                ListBoxItem listBoxItem = new ListBoxItem()
                {
                    Content = typeData.DtTitle,
                    FontSize = 14,
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                };
                DishTypeList.Items.Add(listBoxItem);
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
         * 添加菜系
         * 查空，查重，插入
         * */
        private void AddTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if(AddTypeName.Text == "")
            {
                MessageBox.Show("菜系名为空");
                return;
            }

            int InsertResult = DishInfoConnector.InsertDishType(AddTypeName.Text);
            if(InsertResult == 1)
            {
                MessageBox.Show("插入成功");
                AddTypeName.Text = "";
                LoadAllDishType();
            }
            else
            {
                MessageBox.Show("插入失败");
                return;
            }
        }

        /**
         * 删除选中
         * 未分类菜系不能删除
         * 不必查存在，直接删除
         * */
        private void DeleteTypeButton_Click(object sender, RoutedEventArgs e)
        {
            if(DishTypeList.SelectedIndex == -1)
            {
                MessageBox.Show("未选择任何项");
                return;
            }
            if(DishTypeList.SelectedIndex == 0)
            {
                MessageBox.Show("未分类菜系不能删除");
                return;
            }
            ListBoxItem listBoxItem = (ListBoxItem)DishTypeList.SelectedItem;
            String DTitle = (String)listBoxItem.Content;
            int DeleteResult = DishInfoConnector.DeleteDishType(DTitle);
            if(DeleteResult == 1)
            {
                MessageBox.Show("删除成功");
                LoadAllDishType();
            }
            else
            {
                MessageBox.Show("删除失败");
            }
        }

        /**
         * 返回菜品信息列表界面
         * */
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DishInfo dishInfo = new DishInfo();
            dishInfo.Show();
            Close();
        }
    }
}
