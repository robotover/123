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

namespace WpfApp1.win
{
    public partial class Administrator : Window
    {
        internetshopDBEntities db = new internetshopDBEntities();

        public Administrator()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            DgProducts.ItemsSource = db.product.ToList();
            DgUsers.ItemsSource = db.user.ToList();
            DgOrders.ItemsSource = db.order.ToList();
        }

        // Логика товаров
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            product newProd = new product()
            {
                title = TxtTitle.Text,
                price = Convert.ToInt32(TxtPrice.Text),
                stock_quantity = Convert.ToInt32(TxtStock.Text),
                category_name = TxtCategory.Text
            };
            db.product.Add(newProd);
            db.SaveChanges();
            LoadData();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (DgProducts.SelectedItem is product selected)
            {
                selected.title = TxtTitle.Text;
                selected.price = Convert.ToInt32(TxtPrice.Text);
                selected.stock_quantity = Convert.ToInt32(TxtStock.Text);
                selected.category_name = TxtCategory.Text;
                db.SaveChanges();
                LoadData();
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DgProducts.SelectedItem is product selected)
            {
                try
                {
                    db.product.Remove(selected);
                    db.SaveChanges();
                    LoadData();
                }
                catch
                {
                    MessageBox.Show("Нельзя удалить товар, так как он есть в заказах!");
                }
            }
        }

        // Заполнение полей при клике по таблице
        private void DgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DgProducts.SelectedItem is product selected)
            {
                TxtTitle.Text = selected.title;
                TxtPrice.Text = selected.price.ToString();
                TxtStock.Text = selected.stock_quantity.ToString();
                TxtCategory.Text = selected.category_name;
            }
        }

        // Отчёт по продажам
        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            if (DpFrom.SelectedDate != null && DpTo.SelectedDate != null)
            {
                var report = db.order
                    .Where(o => o.order_date >= DpFrom.SelectedDate && o.order_date <= DpTo.SelectedDate)
                    .ToList();
                DgReport.ItemsSource = report;
            }
        }

        // Выход
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Avtorizaciya login = new Avtorizaciya();
            login.Show();
            this.Close();
        }
    }
}
