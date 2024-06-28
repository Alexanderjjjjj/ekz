using pr_3.models;
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
using pr_3.pages;
using System.Security.Cryptography;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.Net.Mail;

namespace pr_3.pages
{
    public partial class Autho : Page
    {
        Entity dbContext = new Entity();
        User user = new User();
        int countUnsuccessful = 0;
        private string verificationCode;
        private string role;

        public Autho()
        {
            InitializeComponent();
            tboxCaptcha.Visibility = Visibility.Hidden;
            tblockCaptcha.Visibility = Visibility.Hidden;
        }
        private void btnEnterGuests_Click(object sender, RoutedEventArgs e)
        {

        }
        private void btnSign_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(tbxLogin.Text) && !String.IsNullOrEmpty(pasboxPassword.Password))
            {
                LoginUser();
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль", "Предупреждение");
                countUnsuccessful++;
                GenerateCaptcha();
                if (countUnsuccessful % 3 == 0)
                {
                    TimerBLock();
                    return;
                }
            }
        }

        private void LoginUser()
        {
            Entity dbContext = new Entity();
            User user = new User();
            string Login = tbxLogin.Text;
            string pass = pasboxPassword.Password.Trim();


            user = dbContext.User.Where(p => p.userlogin == Login).FirstOrDefault();
            if (countUnsuccessful < 1)
            {
                if (user != null)
                {
                    if (user.user_password == pass)
                    {
                        LoadForm(user.role_id.ToString());
                        tbxLogin.Text = "";
                        tboxCaptcha.Text = "";
                        MessageBox.Show("вы вошли под логином: " + user.userlogin.ToString());
                    }
                    else
                    {
                        MessageBox.Show("неверный пароль");
                        GenerateCaptcha();
                        countUnsuccessful++;
                    }
                }
                else
                {
                    MessageBox.Show("пользователя с логином '" + Login + "' не существует");
                }
            } 
        }


            private async void TimerBLock()
        {
            panel.IsEnabled = false;

            await Task.Factory.StartNew(() =>
            {
                for (int i = 10; i > 0; i--)
                {
                    tblockTimer.Dispatcher.Invoke(() =>
                    {
                        tblockTimer.Text = $"подождите {i} сек";
                    });
                    Task.Delay(1000).Wait();//приостанавливает выполнение задачи на 1 секунду
                }
            });

            tblockTimer.Text = "";
            panel.IsEnabled = true;
        }

        private void LoadForm(string _role)
        {
            switch (_role)
            {
                case "1":
                NavigationService.Navigate(new Client(user));
                break;
                case "2":
                NavigationService.Navigate(new Admin(user));
                break;
                case "3":
                if (TimeWork())
                {
                    NavigationService.Navigate(new Employee(user));
                }
                else
                {
                    MessageBox.Show("рабочий день закончен или еще не начался", "Предупреждение");
                }
                break;
            }
        }

        private bool TimeWork()
        {
            var currentTime = DateTime.Now;
            if (currentTime.Hour < 10 || currentTime.Hour > 19) return false;
            return true;
        }
        private void GenerateCaptcha()
        {
            
            tboxCaptcha.Visibility = Visibility.Visible;
            tblockCaptcha.Visibility = Visibility.Visible;

         
            char[] letters = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            Random rand = new Random();

            string word = "";
            for (int j = 1; j <= 8; j++)
            {
                int letter_num = rand.Next(0, letters.Length - 1);
                word += letters[letter_num];
            }
           

            tblockCaptcha.Text = word;
            tblockCaptcha.TextDecorations = TextDecorations.Strikethrough;
            tboxCaptcha.Text = "";
        }
        private bool CheckingCaptcha() => tblockCaptcha.Text == tboxCaptcha.Text.Trim();

        private void btnForgotPassword_Click(object sender, RoutedEventArgs e)
        {

        }
        private string GenerateCode()
        {
            Random random = new Random();
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, 6).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        
        private void btnForgotPass_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ForgotPass());
        }




    }
}
