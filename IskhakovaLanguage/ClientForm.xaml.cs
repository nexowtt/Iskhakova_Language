﻿using Microsoft.Win32;
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

namespace IskhakovaLanguage
{
    /// <summary>
    /// Логика взаимодействия для ClientForm.xaml
    /// </summary>
    public partial class ClientForm : Page
    {
        private Client _currentClient = new Client();
        public ClientForm(Client SelectedClient)
        { 
            InitializeComponent();
            if (SelectedClient != null)
            {
                _currentClient = SelectedClient;
            }
            DataContext = _currentClient;
            if (_currentClient.GenderCode == "м")
            {
                Man.IsChecked = true;
            }
            if (_currentClient.GenderCode == "ж")
            {
                Woman.IsChecked = true;
            }
            if (_currentClient.ID == 0)
            {
                WrapID.Visibility = Visibility.Hidden;
                IDBlock.Visibility = Visibility.Hidden;
                IDBox.Visibility = Visibility.Hidden;

            }
        }
        private void ChangePicture_Click(object sender, RoutedEventArgs e)
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

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentClient.FirstName))
                errors.AppendLine("Укажите фамилию клиента");
            else if (!Regex.IsMatch(_currentClient.FirstName, @"^[A-Za-zА-Яа-яЁё\- ]+$") || _currentClient.FirstName.Length > 50)
                errors.AppendLine("Фамилия может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов");
            if (string.IsNullOrWhiteSpace(_currentClient.LastName))
                errors.AppendLine("Укажите имя клиента");
            else if (!Regex.IsMatch(_currentClient.LastName, @"^[A-Za-zА-Яа-яЁё\- ]+$") || _currentClient.LastName.Length > 50)
                errors.AppendLine("Имя может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов");
            if (string.IsNullOrWhiteSpace(_currentClient.Patronymic))
                errors.AppendLine("Укажите отчество клиента");
            else if (!Regex.IsMatch(_currentClient.Patronymic, @"^[A-Za-zА-Яа-яЁё\- ]+$") || _currentClient.Patronymic.Length > 50)
                errors.AppendLine("Отество может содержать только буквы, пробелы и дефисы, и не может быть длиннее 50 символов");


            if (string.IsNullOrWhiteSpace(BirthDP.Text))
            { errors.AppendLine("Укажите дату рождения клиента"); }
            else
            {
                _currentClient.Birthday = Convert.ToDateTime(BirthDP.Text);
            }


            if (string.IsNullOrWhiteSpace(_currentClient.Phone))
                errors.AppendLine("Укажите номер телефона!");
            else
            {
                string phonePattern = @"^\+?\d[\d\-\(\)\s]+$";
                string clearPhone = _currentClient.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Replace("+", "");
                if (!Regex.IsMatch(_currentClient.Phone, phonePattern) || !clearPhone.All(char.IsDigit))
                    errors.AppendLine("Телефон может содержать только цифры, плюс, минус, открывающая и закрывающая круглые скобки, знак пробела!");
                else
                {
                    if (_currentClient.Phone.Length > 20)
                    {
                        errors.AppendLine("Укажите в телефоне менее 20 символов у клиента");

                    }
                }
            }

            if (string.IsNullOrWhiteSpace(_currentClient.Email))
            {
                errors.AppendLine("Укажите почту клиента");
            }
            else
            {
                string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(_currentClient.Email, emailPattern))
                {
                    errors.AppendLine("Укажите корректный email клиента");
                }
            }

            // _currentClient.Birthday = Convert.ToDateTime(BirthDP.Text);

            if (Woman.IsChecked == true)
            {
                _currentClient.GenderCode = "ж";
            }
            else
            {
                if (Man.IsChecked == true)
                {
                    _currentClient.GenderCode = "м";
                }
                else
                {
                    errors.AppendLine("Укажите пол клиента");
                }
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentClient.ID == 0)
            {
                ИсхаковаLanguageEntities.GetContext().Client.Add(_currentClient);
            }
            try
            {
                _currentClient.RegistrationDate = DateTime.Today;

                ИсхаковаLanguageEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
