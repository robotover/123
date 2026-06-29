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
    public partial class Klient : Window
    {
        internetshopDBEntities db = new internetshopDBEntities();

        public Klient()
        {
            InitializeComponent();
            LoadCatalog();
            LoadMyOrders();
        }

        private void LoadCatalog()
        {
            DgCatalog.ItemsSource = db.product.ToList();
        }

        private void LoadMyOrders()
        {
            DgMyOrders.ItemsSource = db.order.Where(o => o.user_id == Avtorizaciya.CurrentUserId).ToList();
        }

        // Фильтрация
        private void BtnFilter_Click(object sender, RoutedEventArgs e)
        {
            var query = db.product.AsQueryable();

            if (!string.IsNullOrEmpty(TxtFilterCat.Text))
                query = query.Where(p => p.category_name.Contains(TxtFilterCat.Text));

            if (!string.IsNullOrEmpty(TxtPriceFrom.Text))
            {
                int priceFrom = Convert.ToInt32(TxtPriceFrom.Text);
                query = query.Where(p => p.price >= priceFrom);
            }

            if (!string.IsNullOrEmpty(TxtPriceTo.Text))
            {
                int priceTo = Convert.ToInt32(TxtPriceTo.Text);
                query = query.Where(p => p.price <= priceTo);
            }

            DgCatalog.ItemsSource = query.ToList();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            TxtFilterCat.Clear(); TxtPriceFrom.Clear(); TxtPriceTo.Clear();
            LoadCatalog();
        }

        // Создание заказа
        private void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            if (DgCatalog.SelectedItem is product selectedProd)
            {
                order newOrder = new order()
                {
                    user_id = Avtorizaciya.CurrentUserId,
                    order_date = DateTime.Now,
                    status = "В обработке",
                    total_amount = selectedProd.price
                };
                db.order.Add(newOrder);
                db.SaveChanges();

                order_item item = new order_item()
                {
                    order_id = newOrder.order_id,
                    product_id = selectedProd.product_id,
                    quantity = 1,
                    price_at_purchase = selectedProd.price
                };
                db.order_item.Add(item);
                db.SaveChanges();

                MessageBox.Show("Успешно оформлено!");
                LoadMyOrders();
            }
            else
            {
                MessageBox.Show("Сначала выберите товар из таблицы!");
            }
        }

        // Написать отзыв
        private void BtnReview_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtRevProductId.Text) && !string.IsNullOrEmpty(TxtRevText.Text))
            {
                review newRev = new review()
                {
                    user_id = Avtorizaciya.CurrentUserId,
                    product_id = Convert.ToInt32(TxtRevProductId.Text),
                    review_text = TxtRevText.Text,
                    created_at = DateTime.Now
                };
                db.review.Add(newRev);
                db.SaveChanges();

                MessageBox.Show("Отзыв сохранен!");
                TxtRevProductId.Clear();
                TxtRevText.Clear();
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Avtorizaciya login = new Avtorizaciya();
            login.Show();
            this.Close();
        }
    }
}
