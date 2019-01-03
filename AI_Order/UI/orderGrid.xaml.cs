using AI_Order.DataStruct;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_Order
{
    /// <summary>
    /// orderGrid.xaml 的交互逻辑
    /// </summary>
    public partial class orderGrid : UserControl
    {
        public orderGrid()
        {
            InitializeComponent();
        }
        public orderGrid(Dish dish)
        {
            InitializeComponent();
            this.nameLabel.Content = dish.DTitle;
            this.priceLabel.Content ="当前菜价：￥"+ dish.DPrice;
            MemoryStream ms = new MemoryStream(dish.DPic);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            BitmapImage newBitmapImage = new BitmapImage();
            newBitmapImage.BeginInit();
            newBitmapImage.StreamSource = ms;
            newBitmapImage.EndInit();
            this.image1.Source = newBitmapImage;
        }
        private void addNumBtn_Click(object sender, RoutedEventArgs e)
        {
            orderNum.Content = int.Parse(orderNum.Content.ToString())+1;
            if (redOrderBtn.IsEnabled == false)
                redOrderBtn.IsEnabled = true;
        }

        private void redOrderBtn_Click(object sender, RoutedEventArgs e)
        {
            orderNum.Content = int.Parse(orderNum.Content.ToString()) - 1;
            if (int.Parse(orderNum.Content.ToString()) == 1)
                redOrderBtn.IsEnabled = false;
        }
    }
}
