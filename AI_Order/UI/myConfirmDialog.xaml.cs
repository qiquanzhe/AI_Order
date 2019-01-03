using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// myConfirmDialog.xaml 的交互逻辑
    /// </summary>
    public partial class myConfirmDialog : Window
    {
        public string vipno;
        public myConfirmDialog()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            vipno = vipnumber.Text;
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
