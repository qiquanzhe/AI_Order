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
using AI_Order.DataStruct;

namespace AI_Order.UI
{
    /// <summary>
    /// PayInfo.xaml 的交互逻辑
    /// </summary>
    public partial class PayInfo : UserControl
    {
        public PayInfo()
        {
            InitializeComponent();
        }
        public PayInfo(AddOrderTmp orderTmp)
        {
            InitializeComponent();
            DName.Text = orderTmp.DTitle;
            DNumber.Text = orderTmp.DNumber;
            DPrice.Text = orderTmp.DPrice;
            DSum.Text = (int.Parse(orderTmp.DNumber) * double.Parse(orderTmp.DPrice)).ToString();
        }
    }
}
