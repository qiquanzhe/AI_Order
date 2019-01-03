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
//using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AI_Order
{
    /// <summary>
    /// list.xaml 的交互逻辑
    /// </summary>
    public partial class list : Window
    {
        private Microsoft.Win32.OpenFileDialog openfile;
        private ConnectMysql cm;
        private float tempPrice;
        private int tempscore;
        List<WhatToOrder> orders;
        List<WhatToOrder> exchanges;
        public list()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            orders = new List<WhatToOrder>();
            exchanges = new List<WhatToOrder>();
            initorderGrid();
            initexchangeGrid();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            label2.Content = "Clicked!";
            openfile = new OpenFileDialog();
            openfile.Filter = "pictures (*.jpg;*.bmp;*png)|*.jpeg;*.jpg;*.bmp;*.png|AllFiles(*.*)|*.*";
            if (openfile.ShowDialog() == true)
            {
                label2.Content = openfile.FileName;
                image2.Source = new BitmapImage(new Uri(openfile.FileName));
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("Give it a number!");
                return;
            }
            else if(!IsNumbers(textBox1.Text))
            {
                MessageBox.Show("Input Correct Numbers!");
                return;
            }
            else if (textBox2.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("Doesn't your food have a name?");
                return;
            }
            else if (textBox3.Text.Equals(""))
            {
                System.Windows.MessageBox.Show("Don't want to make money?");
                return;
            }
            else if (openfile == null)
            {
                System.Windows.MessageBox.Show("No Pictures selected?");
                return;
            }
            else
            {
                float price;
                if (!float.TryParse(textBox3.Text, out price))
                {
                    MessageBox.Show("Price isnot in correct format!");
                    return;
                }
                byte[] bytesOfPic = File.ReadAllBytes(openfile.FileName);
                cm = new ConnectMysql();
                int flag = cm.AddMenu(textBox1.Text, textBox2.Text, price, ref bytesOfPic);
                if (flag == 0)
                {
                    label2.Content = "Add to Menu successfully!";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    openfile = null;
                    refreshOrder();
                    return;
                }
                else {
                    label2.Content = "Failed to add to menu!";
                    return;
                }
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox4.Text != "" || textBox5.Text!= "") button6.IsEnabled = true;
            else button6.IsEnabled = false;
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox4.Text != "" || textBox5.Text != "") button6.IsEnabled = true;
            else button6.IsEnabled = false;
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            int flag;
            string condition, res;
            ConnectMysql cm = new ConnectMysql();
            if (textBox4.Text == "" && textBox5.Text == "")
            {
                MessageBox.Show("No Condition Error!");
                return;
            }
            else if (!IsNumbers(textBox4.Text)&&textBox5.Text == "")
            {
                MessageBox.Show("Input Numbers!");
                return;
            }
            else if (textBox4.Text != "") flag = 0;
            else flag = 1;

            if (flag == 0) condition = textBox4.Text;
            else condition = textBox5.Text;

            res = cm.findMenu(condition, flag);
            if (res == "")
            {
                MessageBox.Show("Cannot Find this record!");
                return;
            }
            else
            {
                string[] resArray = res.Split(';');
                if(resArray[0] != null) numberLabel.Content = resArray[0];
                if (resArray[1] != null) nameLabel.Content = resArray[1];
                if (resArray[2] != null) priceLabel.Content = resArray[2];
                if (resArray[3] != null) scoreLabel.Content = resArray[3];
                button5.IsEnabled = true;
            }

        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult mbr =  MessageBox.Show("Sure to Delete?", "Message", MessageBoxButton.OKCancel,MessageBoxImage.Question);
            if (mbr == MessageBoxResult.OK)
            {
                ConnectMysql cm = new ConnectMysql();
                cm.deleteMenu(numberLabel.Content.ToString());
                refreshOrder();
                button5.IsEnabled = false;
                resetAllDelete();
            }
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            resetAllDelete();
        }

        private void resetAllDelete()
        {
            textBox4.Text = "";
            textBox4.IsEnabled = false;
            textBox5.Text = "";
            textBox5.IsEnabled = false;
            numberLabel.Content = "showNumber";
            nameLabel.Content = "showName";
            priceLabel.Content = "showPrice";
            scoreLabel.Content = "showScore";
            button5.IsEnabled = false;
            button6.IsEnabled = false;
        }

        private void radioButton2_Checked(object sender, RoutedEventArgs e)
        {
            textBox5.IsEnabled = false;
            textBox5.Text = "";
            textBox4.IsEnabled = true;
        }

        private void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            textBox4.IsEnabled = false;
            textBox4.Text = "";
            textBox5.IsEnabled = true;
        }

        private void button7_Click(object sender, RoutedEventArgs e)
        {
            int flag;
            string condition, res;
            ConnectMysql cm = new ConnectMysql();
            if (textBox6.Text == "" && textBox7.Text == "")
            {
                MessageBox.Show("No Condition Error!");
                return;
            }
            else if (!IsNumbers(textBox6.Text) && textBox7.Text == "")
            {
                MessageBox.Show("Input Numbers!");
                return;
            }
            else if (textBox6.Text != "") flag = 0;
            else flag = 1;

            if (flag == 0) condition = textBox6.Text;
            else condition = textBox7.Text;

            res = cm.findMenu(condition, flag);
            if (res == "")
            {
                MessageBox.Show("Cannot Find this record!");
                return;
            }
            else
            {
                string[] resArray = res.Split(';');
                if (resArray[0] != null) numberLabel1.Content = resArray[0];
                if (resArray[1] != null) nameLabel1.Content = resArray[1];
                if (resArray[2] != null) priceLabel1.Content = resArray[2];
                if (resArray[3] != null) scoreLabel1.Content = resArray[3];
                button9.IsEnabled = true;
                comboBox1.IsEnabled = true;
                textBox8.IsEnabled = true;
            }
        }

        private void radioButton3_Checked(object sender, RoutedEventArgs e)
        {
            textBox6.IsEnabled = true;
            textBox7.IsEnabled = false;
            textBox7.Text = "";
        }

        private void radioButton4_Checked(object sender, RoutedEventArgs e)
        {
            textBox7.IsEnabled = true;
            textBox6.IsEnabled = false;
            textBox6.Text = "";
        }

        private void textBox6_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox6.Text != "" || textBox7.Text != "") button8.IsEnabled = true;
            else button8.IsEnabled = false;
        }

        private void textBox7_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox6.Text != "" || textBox7.Text != "") button8.IsEnabled = true;
            else button8.IsEnabled = false;
        }

        private void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.RemovedItems.Count > 0)
            {
                string selected = comboBox1.SelectedItem.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
                MessageBox.Show(selected);
                if (selected.Equals("Picture")) button10.Visibility = Visibility.Visible;
                else if (button10 != null) button10.Visibility = Visibility.Hidden;
            }
        }

        private void button8_Click(object sender, RoutedEventArgs e)
        {
            textBox6.Text = "";
            textBox6.IsEnabled = false;
            textBox7.IsEnabled = false;
            textBox7.Text = "";

            comboBox1.IsEnabled = false;
            textBox8.Text = "";
            textBox8.IsEnabled = false;

            button9.IsEnabled = false;

            numberLabel1.Content = "showNumber";
            nameLabel1.Content = "showName";
            priceLabel1.Content = "showPrice";
            scoreLabel1.Content = "showContent";
            button8.IsEnabled = false;
        }

        private void button9_Click(object sender, RoutedEventArgs e)
        {
            cm = new ConnectMysql();
            string selected = comboBox1.SelectedValue.ToString().Replace("System.Windows.Controls.ComboBoxItem: ", "");
            if (selected.Equals("Price")) 
            {
                if(textBox8.Text == "")
                {
                    MessageBox.Show("No Price?");
                    return;
                }
                cm.changeMenu(numberLabel1.Content.ToString(), "price", textBox8.Text);
            }
            else if (selected.Equals("Name"))
            {
                if (textBox8.Text == "")
                {
                    MessageBox.Show("No Name?");
                    return;
                }
                cm.changeMenu(numberLabel1.Content.ToString(), "name", textBox8.Text);
            }
            else
            {
                if(openfile == null)
                {
                    MessageBox.Show("No Picture?");
                    return;
                }
                byte[] bytes = File.ReadAllBytes(openfile.FileName);
                cm.changeMenu(numberLabel1.Content.ToString(), "pic", "",bytes);
            }

            string res = cm.findMenu(numberLabel1.Content.ToString(),0);
            string[] resArray = res.Split(';');
            if (resArray[0] != null) numberLabel1.Content = resArray[0];
            if (resArray[1] != null) nameLabel1.Content = resArray[1];
            if (resArray[2] != null) priceLabel1.Content = resArray[2];
            if (resArray[3] != null) scoreLabel1.Content = resArray[3];

            textBox6.Text = "";
            textBox6.IsEnabled = false;
            textBox7.IsEnabled = false;
            textBox7.Text = "";

            comboBox1.IsEnabled = false;
            textBox8.Text = "";
            textBox8.IsEnabled = false;

            button9.IsEnabled = false;

            numberLabel1.Content = "showNumber";
            nameLabel1.Content = "showName";
            priceLabel1.Content = "showPrice";
            scoreLabel1.Content = "showContent";
            button8.IsEnabled = false;
            button10.Visibility = Visibility.Hidden;
            refreshOrder();
        }

        private void button10_Click(object sender, RoutedEventArgs e)
        {
            openfile = new OpenFileDialog();
            openfile.Filter = "pictures (*.jpg;*.bmp;*png)|*.jpeg;*.jpg;*.bmp;*.png|AllFiles(*.*)|*.*";
            if (openfile.ShowDialog() == true)
                textBox8.Text = openfile.FileName;
        }

        private void button11_Click(object sender, RoutedEventArgs e)
        {
            int flag;
            if (checkBox1.IsChecked == true) flag = 1;
            else flag = 0;

            if (textBox9.Text == "")
            {
                MessageBox.Show("No PhoneNumber!");
                return;
            }
            else if(!IsMobilePhone(textBox9.Text)) {
                MessageBox.Show("Error PhoneNumber!");
                return;
            }
            cm = new ConnectMysql();
            cm.registerVip(textBox9.Text, flag);
        }

        //正则表达式验证手机号码
        public bool IsMobilePhone(string input)  
        {
            if (input.Length == 0)
            {
                return false;
            }
            else if (input.Length != 11)
            {
                return false;
            }
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^[1]+[3,4,5,7,8]+\d{9}");
        }

        //正则表达式验证纯数字
        public bool IsNumbers(string input)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(input, @"^\d+$");
        }

        //初始化菜单
        void initorderGrid()
        {
            cm = new ConnectMysql();
            int i = cm.countOrder();
            //MessageBox.Show(""+i);
            int  rows;
            if (i / 2 == 0) rows = i / 2;
            else rows = i / 2 + 1;

            RowDefinition[] row = new RowDefinition[i];
            for (int k = 0; k < i; k++)
            {
                row[k] = new RowDefinition();
                grid15.RowDefinitions.Add(row[k]);
            }

            orderGrid[] og = new orderGrid[i];
            orderInfo[] info = cm.getOrderInfo(i);
            
            //og[0] = new orderGrid();
            //og[0].nameLabel.Content = info[0].name;

            //grid15.Children.Add(og[0]);
            //Grid.SetRow(og[0], 0);
            //Grid.SetColumn(og[0], 0);
            MemoryStream ms;
            for (int k = 0; k < i; k++)
            {
                og[k] = new orderGrid();
                og[k].nameLabel.Content = info[k].name;
                og[k].priceLabel.Content = "￥" + info[k].price;
                //og[k].scoreLabel.Content = "#" + info[k].score;
                ms = new MemoryStream(info[k].pic);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                BitmapImage newBitmapImage = new BitmapImage();
                newBitmapImage.BeginInit();
                newBitmapImage.StreamSource = ms;
                newBitmapImage.EndInit();
                //System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                //og[k].image1.Source = BitmapFrame.Create(ms, BitmapCreateOptions.None, BitmapCacheOption.Default);
                og[k].image1.Source = newBitmapImage;
                
                og[k].button1.Click+= new RoutedEventHandler(addSummary);
                grid15.Children.Add(og[k]);
                
                Grid.SetRow(og[k], k / 2);
                Grid.SetColumn(og[k], k % 2 == 0 ? 0 : 1);
            }
        }

        //按钮事件
        private void addSummary(object sender, RoutedEventArgs e)
        {
            WhatToOrder anOrder = new WhatToOrder();

            Button btn = sender as Button;
            var a = VisualTreeHelper.GetParent(btn);
            List<Label> listLabel = GetChildObjects<Label>(a, "");

            //MessageBox.Show(btn.Content.ToString());
            tempPrice = float.Parse(listLabel[0].Content.ToString().Replace("￥", ""));
            tempscore = int.Parse(listLabel[2].Content.ToString().Replace("#", ""));
            int orderNo = int.Parse(listLabel[3].Content.ToString());
            int recordNum = int.Parse(listLabel[4].Content.ToString());
            if (btn.Content.ToString().Equals("Cancel"))
            {
                orderNum.Content = float.Parse(orderNum.Content.ToString()) - tempPrice*recordNum;
                orderScore.Content = int.Parse(orderScore.Content.ToString()) - tempscore*recordNum;
                listLabel[4].Content = 1;
                for (int x = 0; x < orders.Count; x++)
                {
                    if (orders[x].name.Equals(listLabel[1].Content.ToString()))
                        orders.Remove(orders[x]);
                }
                btn.Content = "Order";
                if (orderNum.Content.ToString().Equals("0"))
                    button12.IsEnabled = false;
            }
            else
            {
                orderNum.Content = float.Parse(orderNum.Content.ToString()) + tempPrice*orderNo;
                orderScore.Content = int.Parse(orderScore.Content.ToString()) + tempscore*orderNo;
                listLabel[4].Content =  orderNo;
                anOrder.name = listLabel[1].Content.ToString();
                anOrder.number = orderNo;
                orders.Add(anOrder);
                btn.Content = "Cancel";
                if (button12.IsEnabled == false)
                    button12.IsEnabled = true;
            }
            
        }

        public List<T> GetChildObjects<T>(DependencyObject obj, string name) where T : FrameworkElement
        {
            DependencyObject child = null;
            List<T> childList = new List<T>();
            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);
                if (child is T && (((T)child).Name == name || string.IsNullOrEmpty(name)))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, ""));
            }
            return childList;
        }

        //刷新菜单
        public void refreshOrder()
        {
            cm = new ConnectMysql();
            int i = cm.countOrder();
            for (int k = 1; k <= i/2+1; k++)
            {
                if (grid15.Children.Count > 0)
                {
                    grid15.Children.RemoveRange(0, 3);
                    foreach (orderGrid og in grid15.Children)
                        Grid.SetRow(og, (Grid.GetRow(og) - 1));
                }
            }
            initorderGrid();
        }


        //验证vip，发送至服务器端
        private void button12_Click(object sender, RoutedEventArgs e)
        {
            if (orderNum.Content.ToString().Equals("0"))
            {
                MessageBox.Show("No Orders!");
                return;
            }

            myConfirmDialog mcd = new myConfirmDialog();
            mcd.ShowDialog();
            if (mcd.DialogResult == true)
            {
                try
                {
                    int port = 2000;
                    string host = "127.0.0.1";
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
                    Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
                    //Console.WriteLine("Conneting...");
                    //connectInfo.Content = "Connecting...";
                    c.Connect(ipe);//连接到服务器
                    string sendStr ="";
                    foreach (WhatToOrder w in orders)
                    {
                        sendStr += w.name + ";" + w.number + ";" ;
                    }
                    byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                    c.Send(bs, bs.Length, 0);//发送测试信息
                    c.Close();
                    cm = new ConnectMysql();
                    refreshOrder();
                    orders = new List<WhatToOrder>();
                    button12.IsEnabled = false;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                orderScore.Content = 0;
                orderNum.Content = 0;
            }
        }

        //初始化换购菜单
        void initexchangeGrid()
        {
            cm = new ConnectMysql();
            int i = cm.countOrder();
            int rows;
            if (i / 2 == 0) rows = i / 2;
            else rows = i / 2 + 1;

            RowDefinition[] row = new RowDefinition[i];
            for (int k = 0; k < i; k++)
            {
                row[k] = new RowDefinition();
                exchangeGrid.RowDefinitions.Add(row[k]);
            }

            orderGrid[] og = new orderGrid[i];
            orderInfo[] info = cm.getOrderInfo(i);
            
            MemoryStream ms;
            for (int k = 0; k < i; k++)
            {
                og[k] = new orderGrid();
                og[k].nameLabel.Content = info[k].name;
                og[k].priceLabel.Content = 100 * info[k].score + " Needed";
                //og[k].scoreLabel.Content = "";
                og[k].button1.Content = "Select";
                ms = new MemoryStream(info[k].pic);
                ms.Seek(0, System.IO.SeekOrigin.Begin);
                BitmapImage newBitmapImage = new BitmapImage();
                newBitmapImage.BeginInit();
                newBitmapImage.StreamSource = ms;
                newBitmapImage.EndInit();
                og[k].image1.Source = newBitmapImage;
                og[k].button1.Click += new RoutedEventHandler(addScoreSummary);
                exchangeGrid.Children.Add(og[k]);

                Grid.SetRow(og[k], k / 2);
                Grid.SetColumn(og[k], k % 2 == 0 ? 0 : 1);
            }
        }

        //换购按钮点击事件
        private void addScoreSummary(object sender, RoutedEventArgs e)
        {
            WhatToOrder anOrder = new WhatToOrder();
            Button btn = sender as Button;
            var a = VisualTreeHelper.GetParent(btn);
            List<Label> listLabel = GetChildObjects<Label>(a, "");
            tempscore = int.Parse(listLabel[0].Content.ToString().Replace(" Needed", ""));
            int orderNo = int.Parse(listLabel[3].Content.ToString());
            int recordNum = int.Parse(listLabel[4].Content.ToString());
            if (btn.Content.ToString().Equals("Cancel"))
            {
                scoreNeed.Content = int.Parse(scoreNeed.Content.ToString()) - tempscore * recordNum;
                listLabel[4].Content = 1;
                for (int x = 0; x < exchanges.Count; x++)
                {
                    if (exchanges[x].name.Equals(listLabel[1].Content.ToString()))
                        exchanges.Remove(exchanges[x]);
                }
                btn.Content = "Exchange";
                if (scoreNeed.Content.ToString().Equals("0"))
                    exchangeButton.IsEnabled = false;
            }
            else
            {
                //orderNum.Content = float.Parse(orderNum.Content.ToString()) + tempPrice * orderNo;
                scoreNeed.Content = int.Parse(scoreNeed.Content.ToString()) + tempscore * orderNo;
                listLabel[4].Content = orderNo;
                anOrder.name = listLabel[1].Content.ToString();
                anOrder.number = orderNo;
                exchanges.Add(anOrder);
                btn.Content = "Cancel";
                if (exchangeButton.IsEnabled == false)
                    exchangeButton.IsEnabled = true;
            }
        }

        private void exchangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (scoreNeed.Content.ToString().Equals("0"))
            {
                MessageBox.Show("No Exchanges!");
                return;
            }

            myConfirmDialog mcd = new myConfirmDialog();
            mcd.ShowDialog();

            if (mcd.vipno.Equals(""))
            {
                MessageBox.Show("Are You A VIP?");
                return;
            }

            if (mcd.DialogResult == true)
            {
                try
                {
                    int port = 2000;
                    string host = "127.0.0.1";
                    IPAddress ip = IPAddress.Parse(host);
                    IPEndPoint ipe = new IPEndPoint(ip, port);//把ip和端口转化为IPEndPoint实例
                    Socket c = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//创建一个Socket
                    c.Connect(ipe);//连接到服务器
                    string sendStr ="";
                    foreach (WhatToOrder w in exchanges)
                    {
                        sendStr += w.name + ";" + w.number + ";";
                    }
                    byte[] bs = Encoding.ASCII.GetBytes(sendStr);
                    c.Send(bs, bs.Length, 0);//发送测试信息
                    c.Close();
                    cm = new ConnectMysql();
                    //refreshOrder();
                    refreshExchangeGrid();
                    exchanges = new List<WhatToOrder>();
                    exchangeButton.IsEnabled = false;
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                //orderScore.Content = 0;
                scoreNeed.Content = 0;
            }
        }

        private void refreshExchangeGrid()
        {
            cm = new ConnectMysql();
            int i = cm.countOrder();
            for (int k = 1; k <= i / 2 + 1; k++)
            {
                if (exchangeGrid.Children.Count > 0)
                {
                    exchangeGrid.Children.RemoveRange(0, 3);
                    foreach (orderGrid og in exchangeGrid.Children)
                        Grid.SetRow(og, (Grid.GetRow(og) - 1));
                }
            }
            initexchangeGrid();
        }
    }
}