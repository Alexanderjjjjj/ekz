using iText.IO.Image;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.Win32;
using pr_3.models;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pr_3.pages
{
    /// <summary>
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Page
    {
        private User currentUser;
        private Entity context = new Entity();

        public Admin(User currentUser)
        {
            InitializeComponent();
            this.currentUser = currentUser;
            var ppl = context.User.ToList();
            LViewPpl.ItemsSource = ppl;
        }

        // Остальной код...

        private void SaveToPDF_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF files (*.pdf)|*.pdf";
            saveFileDialog.DefaultExt = "pdf";
            saveFileDialog.AddExtension = true;
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    using (var context = new Entity())
                    {
                        var users = context.User.ToList();

                        using (var writer = new PdfWriter(saveFileDialog.FileName))
                        {
                            using (var pdf = new PdfDocument(writer))
                            {
                                var document = new Document(pdf);

                                var table = new iText.Layout.Element.Table(new float[] { 1, 1, 1, 1, 1 });
                                table.AddHeaderCell("Photo");
                                table.AddHeaderCell("Name");
                                table.AddHeaderCell("Surname");
                                table.AddHeaderCell("Patronymic");
                                table.AddHeaderCell("Role");
                                foreach (var user in users)
                                {
                                    

                                    if (user.name != null)
                                    {
                                        var nameElement = new iText.Layout.Element.Text(user.name);
                                        table.AddCell(nameElement.GetText());
                                    }
                                    else
                                    {
                                        table.AddCell(""); // Empty cell if no name
                                    }

                                    table.AddCell(user.surname ?? ""); // Добавляем фамилию в следующий столбец
                                    table.AddCell(user.otchestvo ?? ""); // Добавляем отчество в следующий столбец
                                    table.AddCell(user.Role?.name_role ?? ""); // Добавляем роль в следующий столбец
                                }
                                document.Add(table);
                            }
                        }

                        MessageBox.Show("PDF file has been saved successfully!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }










        private void Selector_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            var selectedUser = (User)LViewPpl.SelectedItem;

            if (selectedUser != null)
            {
                NavigationService.Navigate(new Redact(selectedUser));
            }
            else
            {
                MessageBox.Show("Please select a user to edit.");
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearch.Text;

            if (searchText.Length == 0)
            {
                var ppl = context.User.ToList();
                LViewPpl.ItemsSource = ppl;
            }
            else
            {
                if (cmbSorting.SelectedIndex == 0) //по возр
                {
                    switch (cmbFilter.SelectedIndex)
                    {
                        case 0: // Должность
                            var filteredAndSortedPpl = context.User
                                .Where(u => u.Role.name_role.Contains(searchText))
                                .OrderBy(u => u.Role.name_role)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl;
                            break;

                        case 1: // Фамилия
                            var filteredAndSortedPpl1 = context.User
                                .Where(u => u.surname.Contains(searchText))
                                .OrderBy(u => u.surname)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl1;
                            break;

                        case 2: // Имя
                            var filteredAndSortedPpl2 = context.User
                                .Where(u => u.name.Contains(searchText))
                                .OrderBy(u => u.name)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl2;
                            break;

                        case 3: // Отчество
                            var filteredAndSortedPpl3 = context.User
                                .Where(u => u.otchestvo.Contains(searchText))
                                .OrderBy(u => u.otchestvo)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl3;
                            break;
                    }
                }

                if (cmbSorting.SelectedIndex == 1) //по убыв
                {
                    switch (cmbFilter.SelectedIndex)
                    {
                        case 0: // Должность
                            var filteredAndSortedPpl = context.User
                                .Where(u => u.Role.name_role.Contains(searchText))
                                .OrderByDescending(u => u.Role.name_role)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl;
                            break;

                        case 1: // Фамилия
                            var filteredAndSortedPpl1 = context.User
                                .Where(u => u.surname.Contains(searchText))
                                .OrderByDescending(u => u.surname)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl1;
                            break;

                        case 2: // Имя
                            var filteredAndSortedPpl2 = context.User
                                .Where(u => u.name.Contains(searchText))
                                .OrderByDescending(u => u.name)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl2;
                            break;

                        case 3: // Отчество
                            var filteredAndSortedPpl3 = context.User
                                .Where(u => u.otchestvo.Contains(searchText))
                                .OrderByDescending(u => u.otchestvo)
                                .ToList();
                            LViewPpl.ItemsSource = filteredAndSortedPpl3;
                            break;
                    }
                }
            }
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            Invite invite = new Invite();
            NavigationService.Navigate(invite);
        }

        
        private void cmbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void LViewPpl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
