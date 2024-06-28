using pr_3.models;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Data.Entity;


namespace pr_3.pages
{
    /// <summary>
    /// Логика взаимодействия для Invite.xaml
    /// </summary>
    public partial class Invite : Page
    {
        private User currentUser;
        public Invite()
        {
            InitializeComponent();
            currentUser = new User(); 

        }
        private void InitializeUI()
        {
            tbFam.Text = currentUser.surname;
            tbName.Text = currentUser.name;
            tbOtch.Text = currentUser.otchestvo;
            tbPhone.Text = currentUser.phone_number;

            tbRole.SelectedIndex = currentUser.role_id - 1; 

            tbLogin.Text = currentUser.userlogin;
            tbPass.Text = currentUser.user_password;

            
        }
        private BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            BitmapImage image = new BitmapImage();

            try
            {
                using (MemoryStream stream = new MemoryStream(imageData))
                {
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = stream;
                    image.EndInit();
                    image.Freeze(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }

            return image;
        }
        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg, *.jpeg, *.png, *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            
        }


        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
           
            User newUser = new User
            {
                name = tbName.Text,
                surname = tbFam.Text,
                otchestvo = tbOtch.Text,
                phone_number = tbPhone.Text,
                email = "", 
                userlogin = tbLogin.Text,
                user_password = tbPass.Text,
                role_id = GetRoleIdFromComboBox(), 
                GenderId = GetGenderIdFromComboBox(), 
            };

            // Save the user to the database
            using (var dbContext = new Entity())
            {
                dbContext.User.Add(newUser);
                dbContext.SaveChanges();
            }

            MessageBox.Show("User saved to the database.");
        }

        private int GetRoleIdFromComboBox()
        {
           
            switch (tbRole.SelectedItem.ToString())
            {
                case "Client":
                    return 1; 
                case "Employee":
                    return 2; 
                case "Admin":
                    return 3;
                default:
                    return 1;
            }
        }

        private int GetGenderIdFromComboBox()
        {
            
            switch (tbGender.SelectedItem.ToString())
            {
                case "Мужской":
                    return 1; 
                case "Женский":
                    return 2; 
                default:
                    return 1; 
            }
        }

        private void BtnSaveImage_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
