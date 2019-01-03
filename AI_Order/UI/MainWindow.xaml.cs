using AI_Order.Connector;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AI_Order
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ValidCode validCode;
        public MainWindow()
        {
            /*
             * 窗口初始化位置居中
             * 加载验证码
             */
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            LoadValidCode();
        }
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            /*
             * 判断输入框是否为空
             */
            if (textBox1.Text == "")
            {
                MessageBox.Show("用户名不能为空!");
                return;
            }
            else if (passwordBox1.Password == "")
            {
                MessageBox.Show("密码不能为空!");
                return;
            }
            else if (textBox2.Text == "")
            {
                MessageBox.Show("验证码不能为空!");
                return;
            }

            /*
             * 判断验证码是否正确
             */
            else if (!textBox2.Text.ToString().ToUpper().Equals(validCode.CheckCode))
            {
                MessageBox.Show("验证码错误!");
                validCode = new ValidCode(5, ValidCode.CodeType.Alphas);
                this.image1.Source = BitmapFrame.Create(validCode.CreateCheckCodeImage());
                return;
            }

            int result = ManagerConnector.Login(textBox1.Text, passwordBox1.Password);

            if(result == -2)
            {
                MessageBox.Show("用户名或密码错误！");
                textBox1.Text = "";
                passwordBox1.Password = "";
                textBox2.Text = "";
                LoadValidCode();
            }
            else
            {
                MainMenuWindow mainMenuWindow = new MainMenuWindow(result);
                mainMenuWindow.Show();
                this.Close();
            }
        }

        private void Button1_Clicktemp(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(1);
            mainMenuWindow.Show();
            this.Close();
        }

        /*
         * 拖动窗体的事件
         */
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        /*
         * 退出
         */
        private void Button2_Click(object sender, RoutedEventArgs e) => this.Close();


        /*
         * 鼠标点击验证码时刷新 
         */
        private void Image_MouseDown(object sender, MouseButtonEventArgs e) => LoadValidCode();

        /*
         * 刷新验证码的函数
         */
        private void LoadValidCode()
        {
            validCode = new ValidCode(5, ValidCode.CodeType.Alphas);
            this.image1.Source = BitmapFrame.Create(validCode.CreateCheckCodeImage());
            this.image1.Stretch = Stretch.Uniform;
        }
    }
}
