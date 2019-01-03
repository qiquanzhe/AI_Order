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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_Order.UI
{
    /// <summary>
    /// OrderContainer.xaml 的交互逻辑
    /// </summary>
    public partial class OrderContainer : UserControl
    {
        public OrderContainer()
        {
            InitializeComponent();
        }

        public OrderContainer(String Title,String Price,String number)
        {
            InitializeComponent();
            this.DishTitle.Text = Title;
            this.DishPrice.Text = Price;
            this.DishNumber.Text = number;
        }

        public OrderContainer(AddOrderTmp addOrder)
        {
            InitializeComponent();
            this.DishTitle.Text = addOrder.DTitle;
            this.DishPrice.Text = addOrder.DPrice;
            this.DishNumber.Text = addOrder.DNumber;
        }
    }
}
