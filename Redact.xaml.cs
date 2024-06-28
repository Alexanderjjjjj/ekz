using pr_3.models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace pr_3.pages
{
    public partial class Redact : Page
    {
        private User currentUser;

        public Redact(User selectedUser)
        {
            InitializeComponent();
            this.currentUser = selectedUser;

            // Initialize UI elements with user data
            InitializeUI();
        }

        private void InitializeUI()
        {
            tbFam.Text = currentUser.surname;
            tbName.Text = currentUser.name;
            tbOtch.Text = currentUser.otchestvo;
            tbPhone.Text = currentUser.phone_number;

            tbRole.SelectedIndex = currentUser.role_id - 1;
            if (currentUser.GenderId.HasValue)
            {
                tbGender.SelectedIndex = currentUser.GenderId.Value - 1;
            }

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
                    image.Freeze(); // Freeze the image to prevent memory leaks
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
            try
            {
                using (var context = new Entity())
                {
                    var userToUpdate = context.User.FirstOrDefault(u => u.id == currentUser.id);

                    if (userToUpdate != null)
                    {
                        userToUpdate.surname = tbFam.Text;
                        userToUpdate.name = tbName.Text;
                        userToUpdate.otchestvo = tbOtch.Text;
                        userToUpdate.phone_number = tbPhone.Text;
                        userToUpdate.role_id = tbRole.SelectedIndex + 1;
                        userToUpdate.userlogin = tbLogin.Text;
                        userToUpdate.user_password = tbPass.Text;

                        // Check if a new image is added
                        

                        // Save changes to the database
                        context.SaveChanges();

                        MessageBox.Show("User information updated successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating user: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите удалить пользователя?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DeleteUser();
            }
        }

        private void DeleteUser()
        {
            try
            {
                using (var context = new Entity())
                {
                    var userToDelete = context.User.FirstOrDefault(u => u.id == currentUser.id);

                    if (userToDelete != null)
                    {
                        context.User.Remove(userToDelete);
                        context.SaveChanges();

                        MessageBox.Show("Пользователь удален", "УСПЕХ", MessageBoxButton.OK, MessageBoxImage.Information);

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении пользователя: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
