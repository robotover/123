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
    /// <summary>
    /// Логика взаимодействия для Avtorizaciya.xaml
    /// </summary>
    public partial class Avtorizaciya : Window
    {
        public static int CurrentUserId = 0;
        public Avtorizaciya()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            using (internetshopDBEntities db = new internetshopDBEntities())
            {
                List<user> users = (from us in db.user where us.login == TxtLogin.Text && us.password == TxtPassword.Password select us).ToList();

                if (users.Count != 0)
                {
                    CurrentUserId = users[0].user_id;

                    if (users[0].role == "admin")
                    {
                        Administrator admin = new Administrator();
                        admin.Show();
                        this.Close();
                        MessageBox.Show("Вы вошли как администратор");
                    }
                    if (users[0].role == "client")
                    {
                        Klient user = new Klient();
                        user.Show();
                        this.Close();
                        MessageBox.Show("Вы вошли как клиент");
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка ввода данных");
                }
            }
        }
    }
}
