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
    /// InfomationTable.xaml 的交互逻辑
    /// </summary>
    public partial class InfomationTable : UserControl
    {
        public InfomationTable()
        {
            InitializeComponent();
        }

        public InfomationTable(MemberTypeData memberTypeData)
        {
            InitializeComponent();
            Left_TB.Text = memberTypeData.MTitle;
            Right_TB.Text = memberTypeData.MDiscount.ToString();
        }
    }
}
