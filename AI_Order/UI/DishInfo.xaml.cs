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
    /// DishInfo.xaml 的交互逻辑
    /// </summary>
    public partial class DishInfo : Window
    {
        private List<Dish> dishes;
        public DishInfo()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//显示在屏幕中间
            InitializeComponent();
            LoadAllDishes();
            LoadAllDishType();
            ModifyDishTypeCombo.SelectedIndex = 0;
        }

        /*
         * 鼠标拖动窗口的方法
         * */
        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /**
         * 加载所有菜品信息
         * */
        private void LoadAllDishes()
        {
            int count = DishList.Items.Count;
            for (int i = 0; i < count; i++)
                DishList.Items.RemoveAt(0);
            dishes = DishInfoConnector.GetDishes();
            foreach (Dish dish in dishes)
            {
                InformationTable3 information = new InformationTable3();
                information.Left_TB.Text = dish.DTitle;
                information.Mid_TB.Text = DishInfoConnector.GetTypeData(dish.DTypeId).DtTitle;
                information.Right_TB.Text = dish.DPrice.ToString();
                DishList.Items.Add(information);
            }
            SearchDishTypeCombo.SelectedIndex = 0;
        }

        /**
         * 加载所有的菜系信息
         * */
        private void LoadAllDishType()
        {
            List<DishTypeData> dishTypeDatas = DishInfoConnector.GetDishTypeDatas();
            ModifyDishTypeCombo.IsEnabled = true;
            foreach (DishTypeData typeData in dishTypeDatas)
            {
                SearchDishTypeCombo.Items.Add(typeData.DtTitle);
                ModifyDishTypeCombo.Items.Add(typeData.DtTitle);
            }
            ModifyDishTypeCombo.IsEnabled = false;
        }


        /**
         * 选中菜品的时候把菜品信息显示在右侧
         * */
        private void DishList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DishList.SelectedIndex == -1)
                return;
            ModifyDishName.Text = ((InformationTable3)DishList.SelectedItem).Left_TB.Text;
            ModifyDishPrice.Text = ((InformationTable3)DishList.SelectedItem).Right_TB.Text;

            String DTitle = ((InformationTable3)DishList.SelectedItem).Mid_TB.Text;
            int DtId = DishInfoConnector.GetDish(((InformationTable3)DishList.SelectedItem).Left_TB.Text).DTypeId;
            ModifyDishTypeCombo.IsEnabled = true;
            ModifyDishTypeCombo.SelectedIndex = DtId - 1;
            ModifyDishTypeCombo.IsEnabled = false;
        }

        /**
         * 模糊查询实现查询菜品数据
         * */
        private void FindSimDish()
        {
            List<Dish> findDishes = DishInfoConnector.GetDishes(SearchDishTypeCombo.SelectedIndex, SearchDishName.Text);
            int count = DishList.Items.Count;
            for (int i = 0; i < count; i++)
                DishList.Items.RemoveAt(0);

            foreach (Dish dish in findDishes)
            {
                InformationTable3 information = new InformationTable3();
                information.Left_TB.Text = dish.DTitle;
                information.Mid_TB.Text = DishInfoConnector.GetTypeData(dish.DTypeId).DtTitle;
                information.Right_TB.Text = dish.DPrice.ToString();
                DishList.Items.Add(information);
            }
        }

        private void SearchDishName_TextChanged(object sender, TextChangedEventArgs e) => FindSimDish();

        private void SearchDishTypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) => FindSimDish();

        private void ShowAllDishButton_Click(object sender, RoutedEventArgs e) => LoadAllDishes();

        private void ModifyButton_Click(object sender, RoutedEventArgs e)
        {
            if(ModifyDishPrice.Text == "")
            {
                MessageBox.Show("价格不能为空");
                return;
            }
            if(!double.TryParse(ModifyDishPrice.Text,out double DPrice))
            {
                MessageBox.Show("价格格式不正确");
                return;
            }

            int ModifyResult =
                DishInfoConnector.ModifyDishPrice(ModifyDishName.Text,
                ModifyDishTypeCombo.SelectedIndex + 1,
                double.Parse(ModifyDishPrice.Text));
            if(ModifyResult == 1)
            {
                MessageBox.Show("修改成功");
                DishList.SelectedIndex = -1;
                ModifyDishName.Text = "";
                ModifyDishPrice.Text = "";
                ModifyDishTypeCombo.IsEnabled = true;
                ModifyDishTypeCombo.SelectedIndex = 0;
                ModifyDishTypeCombo.IsEnabled = false;
                LoadAllDishes();
            }
            else
            {
                MessageBox.Show("修改失败");
                return;
            }
        }

        /*
         * 删除选中
         * 判断选中=》删除
         * */
        private void DeleteDishButton_Click(object sender, RoutedEventArgs e)
        {
            if (DishList.SelectedIndex == -1)
            {
                MessageBox.Show("未选中任何菜品");
                return;
            }


            int DeleteDishResult = DishInfoConnector.DeleteDish(ModifyDishName.Text, ModifyDishTypeCombo.SelectedIndex + 1);
            if(DeleteDishResult == 1)
            {
                MessageBox.Show("删除成功");
                DishList.SelectedIndex = -1;
                ModifyDishName.Text = "";
                ModifyDishPrice.Text = "";
                ModifyDishTypeCombo.IsEnabled = true;
                ModifyDishTypeCombo.SelectedIndex = 0;
                ModifyDishTypeCombo.IsEnabled = false;
                LoadAllDishes();
            }
            else
            {
                MessageBox.Show("删除失败");
                return;
            }
        }

        private void AddDishButton_Click(object sender, RoutedEventArgs e)
        {
            AddDish addDish = new AddDish();
            addDish.Show();
            Close();
        }

        private void DishTypeManager_Click(object sender, RoutedEventArgs e)
        {
            DishType dishType = new DishType();
            dishType.Show();
            Close();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(1);
            mainMenuWindow.Show();
            Close();
        }
    }
}
