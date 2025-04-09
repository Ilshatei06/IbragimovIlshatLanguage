using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace IbragimovIlshatLanguage
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Client _currentClient = new Client();

        public AddEditPage(Client SelectedClient)
        {
            InitializeComponent();

            IdTb.Visibility = Visibility.Hidden;
            IdText.Visibility = Visibility.Hidden;

            if (SelectedClient != null)
            {
                _currentClient = SelectedClient;

                IdTb.Visibility = Visibility.Visible;
                IdText.Visibility = Visibility.Visible;
            }

            var currentClient = IbragimovIlshatLanguageEntities.GetContext().Client.Where(p => p.ID == _currentClient.ID).Select(p => p.GenderCode).FirstOrDefault();

            RButtonM.IsChecked = currentClient == "1";
            RButtonW.IsChecked = currentClient == "2";

            DataContext = _currentClient;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_currentClient.FirstName))
                errors.AppendLine("Укажите Фамилию!");
            else if (!Regex.IsMatch(_currentClient.FirstName, @"^[A-Za-zА-Яа-яЁё\- ]+$") || _currentClient.FirstName.Length > 50)
                errors.AppendLine("Фамилия может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов");

            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
                errors.AppendLine("Укажите имя!");
            else if(!Regex.IsMatch(_currentClient.LastName, @"^[A-Za-zА-Яа-яЁё\- ]+$") || _currentClient.LastName.Length > 50)
            {
                errors.AppendLine("Имя может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");
            } 

            if (string.IsNullOrWhiteSpace(_currentClient.Patronymic))
                errors.AppendLine("Укажите отчество!");
            else if (!Regex.IsMatch(_currentClient.Patronymic, @"^[A-Za-zА-Яа-яЁё\- ]+$") || _currentClient.Patronymic.Length > 50)
            {
                errors.AppendLine("Отчество может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов.");
            }


            if (string.IsNullOrWhiteSpace(_currentClient.Email))
                errors.AppendLine("Укажите Email!");
            else
            {
                string pattern = @"^[a-zA-Z0-9._%+-]+@([a-zA-Z0-9][a-zA-Z0-9-]*\.)+[a-zA-Z]{2,}$";
                if (!Regex.IsMatch(_currentClient.Email, pattern))
                    errors.AppendLine("Укажите правильный Email!");
                
            }


            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Укажите номер телефона!");
            else
            {
                string phonePattern = @"^\+?\d[\d\-\(\)\s]+$";
                string clearPhone = _currentClient.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
                if (!Regex.IsMatch(_currentClient.Phone, phonePattern) || !clearPhone.All(char.IsDigit))
                    errors.AppendLine("Телефон может содержать только цифры, плюс, минус, открывающая и закрывающая круглые скобки, знак пробела!");
            }

            if (string.IsNullOrWhiteSpace(_currentClient.BirthdayString))
                errors.AppendLine("Укажите дату рождения!");

            if (!RButtonM.IsChecked == true && !RButtonW.IsChecked == true)
            {
                errors.AppendLine("Выберите пол!");
            }


            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            _currentClient.GenderCode = RButtonM.IsChecked == true ? "1" : "2";

            _currentClient.RegistrationDate = DateTime.Now;

            if (_currentClient.ID == 0)
                IbragimovIlshatLanguageEntities.GetContext().Client.Add(_currentClient);
            try
            {
                IbragimovIlshatLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена!");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            // Путь к корню проекта (без имени проекта)
            string projectDirectory = GetProjectRootDirectory();
            string clientsFolderPath = System.IO.Path.Combine(projectDirectory, "Клиенты");

            // Создаем папку, если её нет
            if (!Directory.Exists(clientsFolderPath))
            {
                Directory.CreateDirectory(clientsFolderPath);
            }

            OpenFileDialog myOpenFileDialog = new OpenFileDialog
            {
                InitialDirectory = clientsFolderPath
            };

            if (myOpenFileDialog.ShowDialog() == true)
            {
                string selectedFilePath = myOpenFileDialog.FileName;

                // Сохраняем относительный путь ОТНОСИТЕЛЬНО КОРНЯ ПРОЕКТА
                _currentClient.PhotoPath = System.IO.Path.Combine("Клиенты", System.IO.Path.GetFileName(selectedFilePath));

                // Загружаем изображение по полному пути
                LogoImage.Source = new BitmapImage(new Uri(selectedFilePath));
            }
        }

        // Метод для получения корня проекта
        private string GetProjectRootDirectory()
        {
            // Путь к исполняемому файлу (bin/Debug)
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            // Поднимаемся на 3 уровня: bin/Debug → bin → Корень проекта
            return System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(System.IO.Path.GetDirectoryName(exePath)));
        }


        private void RButtonM_Checked(object sender, RoutedEventArgs e)
        {
            
        }
        private void RButtonW_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
