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
using AI_Order.Connector;
using AI_Order.DataStruct;
using AI_Order.UI;

namespace AI_Order
{
    /// <summary>
    /// OrderDishWindow.xaml 的交互逻辑
    /// </summary>
    public partial class OrderDishWindow : Window
    {
        private int TId;
        private int LoginType;
        private List<DishTypeData> dishTypeDatas;
        private TableInfoData tableInfo;
        private List<AddOrderTmp> addOrders;

        public OrderDishWindow(int TId,int type)
        {
            //显示在屏幕中间
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.TId = TId;
            LoginType = type;
            addOrders = new List<AddOrderTmp>();
            InitializeComponent();
            tableInfo = TableInfoConnector.GetTable(TId);
            LoadExistOrder();
            TableNameTextBlock.Text = tableInfo.hall.HName + tableInfo.TTitle + "桌";
            AddDishTypeList();
            DishTypeList.SelectedIndex = 1;
        }

        // 鼠标拖动窗口的方法
        private void WindowDrag(object sender, MouseButtonEventArgs e) => this.DragMove();

        //把已有的菜单从数据库中加载出来
        private void LoadExistOrder()
        {
            addOrders = OrderInfoConnector.GetOrdersByTId(TId);
            foreach (AddOrderTmp addOrder in addOrders)
            {
                OrderContainer orderContainer = new OrderContainer(addOrder);
                orderContainer.CancelButton.Click += DeleteThisOrderItem;
                ListBoxItem item = new ListBoxItem();
                item.Content = orderContainer;
                OrderList.Items.Add(item);
            }
            CountPriceSummary();
        }
        /*
         * 动态加载菜系的方法
         * */
        private void AddDishTypeList()
        {
            List<ListBoxItem> listBoxItems = new List<ListBoxItem>();
            dishTypeDatas = DishInfoConnector.GetDishTypeDatas();
            foreach (DishTypeData dishType in dishTypeDatas)
            {
                ListBoxItem item = new ListBoxItem
                {
                    Content = dishType.DtTitle.Trim(),
                    MaxWidth = 45,
                    FontSize = 10
                };
                listBoxItems.Add(item);
            }

            DishTypeList.ItemsSource = listBoxItems;
        }

        //选中菜系时候的事件
        private void DishTypeList_SelectionChanged(object sender, SelectionChangedEventArgs e) => LoadDishList(DishTypeList.SelectedIndex );

        //加载菜品
        private void LoadDishList(int DTypeId)
        {
            while (listDishGrid.Children.Count > 0)
            {
                listDishGrid.Children.RemoveRange(0, 3);
                foreach (orderGrid order in listDishGrid.Children)
                    Grid.SetRow(order, (Grid.GetRow(order) - 1));
            }
            List<Dish> dishes = DishInfoConnector.GetDishes(DTypeId);
            int i = dishes.Count;
            int rows;
            if (i / 2 == 0) rows = i / 2;
            else rows = i / 2 + 1;

            RowDefinition[] row = new RowDefinition[i];
            for (int tk = 0; tk < i; tk++)
            {
                row[tk] = new RowDefinition();
                listDishGrid.RowDefinitions.Add(row[tk]);
            }
            int k = 0;
            foreach (Dish dish in dishes)
            {
                orderGrid order = new orderGrid(dish);
                order.button1.Click += Button1_Click;
                listDishGrid.Children.Add(order);
                Grid.SetRow(order, k / 2);
                Grid.SetColumn(order, k % 2 == 0 ? 0 : 1);
                k++;
            }
        }

        /*
         * 按钮点击事件
         * */
        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            //获取兄弟节点中的所有Label节点，返回List<Label>数组
            var a = VisualTreeHelper.GetParent(btn);
            List<Label> listLabel = GetChildObjects<Label>(a, "");

            //数组中的第二个Label是显示菜品名称的Label
            //将菜品名称和订单数量的Label内容作为参数传给AddOrder函数，添加到已点的菜单中
            String DTitle = listLabel[1].Content.ToString();
            int Dnumber = int.Parse(listLabel[2].Content.ToString());
            AddOrder(DTitle,Dnumber);
        }

        /*
         * 寻找子节点
         * */
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

        /*
         * 将订单加入已点列表的方法
         * */
        private void AddOrder(String DTitle, int number)
        {
            int index = 0;
            foreach (AddOrderTmp orderTmp in addOrders)
            {
                if(DTitle == orderTmp.DTitle)
                {
                    //列表中已经存在的项目，把数量加一下然后退出
                    addOrders[index].DNumber = (int.Parse(addOrders[index].DNumber)+number).ToString();
                    OrderContainer container = new OrderContainer(addOrders[index]);
                    //ListBox方法里面没有替换，只能是先删除然后插入
                    OrderList.Items.RemoveAt(index);
                    ListBoxItem boxItem = new ListBoxItem();
                    boxItem.Content = container;
                    OrderList.Items.Insert(index,boxItem);
                    CountPriceSummary();
                    return;
                }
                index++;
            }
            Dish dish = DishInfoConnector.GetDish(DTitle.Trim());
            AddOrderTmp addOrder = new AddOrderTmp(DTitle, dish.DPrice.ToString(), number.ToString());
            OrderContainer orderContainer = new OrderContainer(addOrder);
            orderContainer.CancelButton.Click += DeleteThisOrderItem;
            ListBoxItem item = new ListBoxItem();
            item.Content = orderContainer;
            OrderList.Items.Add(item);
            addOrders.Add(addOrder);    //用addOrders存储已点的菜品
            CountPriceSummary();
        }

        /*
         * 点击按钮，删除按钮所在的行
         * */
        private void DeleteThisOrderItem(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var a = VisualTreeHelper.GetParent(btn);
            List<TextBlock> textBlocks = GetChildObjects<TextBlock>(a, "");
            int index = 0;
            foreach (AddOrderTmp item in addOrders)
            {
                if(item.DTitle == textBlocks[0].Text)
                {
                    addOrders.RemoveAt(index);
                    OrderList.Items.RemoveAt(index);
                    CountPriceSummary();
                    return;
                }
                index++;
            }
        }

        //计算价格显示在下方
        private void CountPriceSummary()
        {
            double summary = 0;
            foreach (AddOrderTmp addOrder in addOrders)
            {
                double price = int.Parse(addOrder.DNumber) * double.Parse(addOrder.DPrice);
                summary += price;
            }
            sumPriceTextBlock.Text = "￥" + summary;
        }

        /*
         * 返回主界面的按钮事件
         * */
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainMenuWindow mainMenuWindow = new MainMenuWindow(LoginType);
            mainMenuWindow.Show();
            Close();
        }

        /**
         * 提交按钮事件
         * 提交数据库，修改桌号的状态
         * */
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int submit =  OrderInfoConnector.SubmitOrder(TId, addOrders);
            int modify = TableInfoConnector.ModifyStatus(TId, 0);
            if (submit == 0&&modify == 1)
                MessageBox.Show("提交成功！");
            else if (submit == 0&&modify != 1) {
                MessageBox.Show("修改餐桌状态失败！");
                return;
            }
            else
            {
                MessageBox.Show("提交失败");
                return;
            }
            MainMenuWindow mainMenuWindow = new MainMenuWindow(LoginType);
            mainMenuWindow.Show();
            Close();
        }
    }
}
