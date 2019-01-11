using AI_Order.Connector;
using AI_Order.DataStruct;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// AddDish.xaml 的交互逻辑
    /// </summary>
    public partial class AddDish : Window
    {
        private OpenFileDialog openFileDialog;
        public AddDish()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;//窗口初始化位置居中
            InitializeComponent();
            LoadAllDishTypes();
        }

        /*
         * 鼠标拖动窗口的方法
         * */
        private void WindowDrag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /**
         * 加载所有菜系
         * */
        private void LoadAllDishTypes()
        {
            List<DishTypeData> dishTypeDatas = DishInfoConnector.GetDishTypeDatas();
            foreach (DishTypeData typeData in dishTypeDatas)
                AddDishType.Items.Add(typeData.DtTitle);
            AddDishType.SelectedIndex = 0;
        }

        /*
         * 选择图片的方法
         * 将图片显示在DishImage中
         * 路径显示在ImagePathLabel中
         * */
        private void AddDishImageButton_Click(object sender, RoutedEventArgs e)
        {
            openFileDialog = new OpenFileDialog
            {
                Filter = "pictures (*.jpg;*.bmp;*png)|*.jpeg;*.jpg;*.bmp;*.png|AllFiles(*.*)|*.*"
            };
            if (openFileDialog.ShowDialog()== true)
            {
                ImagePathLabel.Content = openFileDialog.FileName;
                DishImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        /*
         * 添加菜单的事件
         * 1. 判断输入框是否为空
         * 2. 判断图片是否选择
         * 3. 判断价格的输入是否正确
         * 4. 判断是否数据已存在（后台）
         * */
        private void AddDishButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddDishName.Text == ""||AddDishPrice.Text == "")
            {
                MessageBox.Show("信息填写不完整");
                return;
            }
            if (openFileDialog == null)
            {
                MessageBox.Show("图片未选择");
                return;
            }

            if(!double.TryParse(AddDishPrice.Text,out double DPrice))
            {
                MessageBox.Show("价格格式不正确");
                return;
            }

            byte[] bytesOfPic = File.ReadAllBytes(openFileDialog.FileName);
            int AddResult = DishInfoConnector.InsertDish(AddDishName.Text, AddDishType.SelectedIndex , DPrice, ref bytesOfPic);
            if(AddResult == -1)
            {
                MessageBox.Show("已存在的菜品");
                return;
            }
            if(AddResult == 1)
            {
                MessageBox.Show("添加成功");
                AddDishName.Text = "";
                AddDishPrice.Text = "";
                openFileDialog = null;
                DishImage.Source = null;
                ImagePathLabel.Content = null;
                AddDishType.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("添加失败");
                return;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            DishInfo dishInfo = new DishInfo();
            dishInfo.Show();
            Close();
        }
    }
}
