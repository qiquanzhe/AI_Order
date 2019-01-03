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

namespace AI_Order
{
    /// <summary>
    /// TableStatus.xaml 的交互逻辑
    /// </summary>
    public partial class TableStatus : UserControl
    {
        public TableStatus()
        {
            lockImage = new BitmapImage(new Uri("pack://application:,,,/Resources/lock.png"));
            unlockImage = new BitmapImage(new Uri("pack://application:,,,/Resources/unlock.png"));
            InitializeComponent();
        }

        public BitmapImage lockImage;
        public BitmapImage unlockImage;
        public int tableNumber;
        public int tableStatus;
    }
}
