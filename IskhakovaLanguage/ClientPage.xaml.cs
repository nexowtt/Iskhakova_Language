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
using System.Xml.Linq;

namespace IskhakovaLanguage
{
    /// <summary>
    /// Логика взаимодействия для ClientPage.xaml
    /// </summary>
    public partial class ClientPage : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Client> CurrentPageList = new List<Client>();
        List<Client> TableList;
        public ClientPage()
        {
            InitializeComponent();
            var Client = ИсхаковаLanguageEntities.GetContext().Client.ToList();
            ClientlistView.ItemsSource = Client;
            PagesComboBox.SelectedIndex = 0;
            Pol.SelectedIndex = 0;
            Sort.SelectedIndex = 0;
            UpdateClient();
        }

        private void UpdateClient()
        {
            var currentClient = ИсхаковаLanguageEntities.GetContext().Client.ToList();
            TableList = currentClient;
            if (Pol.SelectedIndex == 1)
            {
                currentClient = currentClient.Where(p => p.GenderCode == "м").ToList();
            }
            if (Pol.SelectedIndex == 2)
            {
                currentClient = currentClient.Where(p => p.GenderCode == "ж").ToList();
            }
            if (Sort.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderBy(p => p.FirstName).ToList();
            }

            if (Sort.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderBy(p => p.FirstName).ToList();
            }
            if (Sort.SelectedIndex == 2)
            {
                currentClient = currentClient.OrderByDescending(p => ИсхаковаLanguageEntities.GetContext().ClientService
                    .Where(cs => cs.ClientID == p.ID)
                    .OrderByDescending(cs => cs.StartTime)
                    .FirstOrDefault()?.StartTime)
                .ToList();
            }
            if (Sort.SelectedIndex == 3)
            {
                currentClient = currentClient.OrderByDescending(c =>
                ИсхаковаLanguageEntities.GetContext().ClientService
                    .Count(cs => cs.ClientID == c.ID))
                .ToList();
            }


            currentClient = currentClient.Where(p => p.FIO.ToLower().Contains(search.Text.ToLower())
                          //|| p.LastName.ToLower().Contains(search.Text.ToLower())
                          //|| p.Patronymic.ToLower().Contains(search.Text.ToLower())
                          || p.Email.ToLower().Contains(search.Text.ToLower())
                          || p.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(search.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))).ToList();

            ClientlistView.ItemsSource = currentClient;
            TableList = currentClient;

            ChangePage(0, 0);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            if (PagesComboBox.SelectedIndex == 0)
            {
                CurrentPageList.Clear();
                CountRecords = TableList.Count;
                if (CountRecords % 10 > 0)
                {
                    CountPage = CountRecords / 10 + 1;
                }
                else
                {
                    CountPage = CountRecords / 10;
                }
                Boolean Ifupdate = true;
                int min;
                if (selectedPage.HasValue)
                {
                    if (selectedPage >= 0 && selectedPage <= CountPage)
                    {
                        CurrentPage = (int)selectedPage;
                        min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                        for (int i = CurrentPage * 10; i < min; i++)
                        {
                            CurrentPageList.Add(TableList[i]);
                        }
                    }
                }
                else
                {
                    switch (direction)
                    {
                        case 1:
                            if (CurrentPage > 0)
                            {
                                CurrentPage--;
                                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                                for (int i = CurrentPage * 10; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                        case 2:
                            if (CurrentPage < CountPage - 1)
                            {
                                CurrentPage++;
                                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                                for (int i = CurrentPage * 10; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                    }
                }
                if (Ifupdate)
                {
                    PageListBox.Items.Clear();
                    for (int i = 1; i <= CountPage; i++)
                    {
                        PageListBox.Items.Add(i);
                    }
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    TBCount.Text = CountRecords.ToString();
                    TBAllRecords.Text = " из " + ИсхаковаLanguageEntities.GetContext().Client.ToList().Count().ToString();

                    ClientlistView.ItemsSource = CurrentPageList;
                    ClientlistView.Items.Refresh();
                }
            }
            if (PagesComboBox.SelectedIndex == 1)
            {
                CurrentPageList.Clear();
                CountRecords = TableList.Count;
                if (CountRecords % 50 > 0)
                {
                    CountPage = CountRecords / 50 + 1;
                }
                else
                {
                    CountPage = CountRecords / 50;
                }
                Boolean Ifupdate = true;
                int min;
                if (selectedPage.HasValue)
                {
                    if (selectedPage >= 0 && selectedPage <= CountPage)
                    {
                        CurrentPage = (int)selectedPage;
                        min = CurrentPage * 50 + 50 < CountRecords ? CurrentPage * 50 + 50 : CountRecords;
                        for (int i = CurrentPage * 50; i < min; i++)
                        {
                            CurrentPageList.Add(TableList[i]);
                        }
                    }
                }
                else
                {
                    switch (direction)
                    {
                        case 1:
                            if (CurrentPage > 0)
                            {
                                CurrentPage--;
                                min = CurrentPage * 50 + 50 < CountRecords ? CurrentPage * 50 + 50 : CountRecords;
                                for (int i = CurrentPage * 50; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                        case 2:
                            if (CurrentPage < CountPage - 1)
                            {
                                CurrentPage++;
                                min = CurrentPage * 50 + 50 < CountRecords ? CurrentPage * 50 + 50 : CountRecords;
                                for (int i = CurrentPage * 50; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                    }
                }
                if (Ifupdate)
                {
                    PageListBox.Items.Clear();
                    for (int i = 1; i <= CountPage; i++)
                    {
                        PageListBox.Items.Add(i);
                    }
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * 50 + 50 < CountRecords ? CurrentPage * 50 + 50 : CountRecords;
                    TBCount.Text = CountRecords.ToString();
                    TBAllRecords.Text = " из " + ИсхаковаLanguageEntities.GetContext().Client.ToList().Count().ToString();

                    ClientlistView.ItemsSource = CurrentPageList;
                    ClientlistView.Items.Refresh();
                }
            }

            if (PagesComboBox.SelectedIndex == 2)
            {
                CurrentPageList.Clear();
                CountRecords = TableList.Count;
                if (CountRecords % 200 > 0)
                {
                    CountPage = CountRecords / 200 + 1;
                }
                else
                {
                    CountPage = CountRecords / 200;
                }
                Boolean Ifupdate = true;
                int min;
                if (selectedPage.HasValue)
                {
                    if (selectedPage >= 0 && selectedPage <= CountPage)
                    {
                        CurrentPage = (int)selectedPage;
                        min = CurrentPage * 200 + 200 < CountRecords ? CurrentPage * 200 + 200 : CountRecords;
                        for (int i = CurrentPage * 200; i < min; i++)
                        {
                            CurrentPageList.Add(TableList[i]);
                        }
                    }
                }
                else
                {
                    switch (direction)
                    {
                        case 1:
                            if (CurrentPage > 0)
                            {
                                CurrentPage--;
                                min = CurrentPage * 200 + 200 < CountRecords ? CurrentPage * 200 + 200 : CountRecords;
                                for (int i = CurrentPage * 200; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                        case 2:
                            if (CurrentPage < CountPage - 1)
                            {
                                CurrentPage++;
                                min = CurrentPage * 200 + 200 < CountRecords ? CurrentPage * 200 + 200 : CountRecords;
                                for (int i = CurrentPage * 200; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                    }
                }
                if (Ifupdate)
                {
                    PageListBox.Items.Clear();
                    for (int i = 1; i <= CountPage; i++)
                    {
                        PageListBox.Items.Add(i);
                    }
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * 200 + 200 < CountRecords ? CurrentPage * 200 + 200 : CountRecords;
                    TBCount.Text = CountRecords.ToString();
                    TBAllRecords.Text = " из " + ИсхаковаLanguageEntities.GetContext().Client.ToList().Count().ToString();

                    ClientlistView.ItemsSource = CurrentPageList;
                    ClientlistView.Items.Refresh();
                }
            }

            if (PagesComboBox.SelectedIndex == 3)
            {
                CurrentPageList.Clear();
                CountRecords = TableList.Count;
                if (CountRecords % CountRecords > 0)
                {
                    CountPage = CountRecords / CountRecords + 1;
                }
                else
                {
                    CountPage = CountRecords / CountRecords;
                }
                Boolean Ifupdate = true;
                int min;
                if (selectedPage.HasValue)
                {
                    if (selectedPage >= 0 && selectedPage <= CountPage)
                    {
                        CurrentPage = (int)selectedPage;
                        min = CurrentPage * CountRecords + CountRecords < CountRecords ? CurrentPage * CountRecords + CountRecords : CountRecords;
                        for (int i = CurrentPage * CountRecords; i < min; i++)
                        {
                            CurrentPageList.Add(TableList[i]);
                        }
                    }
                }
                else
                {
                    switch (direction)
                    {
                        case 1:
                            if (CurrentPage > 0)
                            {
                                CurrentPage--;
                                min = CurrentPage * CountRecords + CountRecords < CountRecords ? CurrentPage * CountRecords + CountRecords : CountRecords;
                                for (int i = CurrentPage * CountRecords; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                        case 2:
                            if (CurrentPage < CountPage - 1)
                            {
                                CurrentPage++;
                                min = CurrentPage * CountRecords + CountRecords < CountRecords ? CurrentPage * CountRecords + CountRecords : CountRecords;
                                for (int i = CurrentPage * 10; i < min; i++)
                                {
                                    CurrentPageList.Add(TableList[i]);
                                }
                            }
                            else
                            {
                                Ifupdate = false;
                            }
                            break;
                    }
                }
                if (Ifupdate)
                {
                    PageListBox.Items.Clear();
                    for (int i = 1; i <= CountPage; i++)
                    {
                        PageListBox.Items.Add(i);
                    }
                    PageListBox.SelectedIndex = CurrentPage;

                    min = CurrentPage * CountRecords + CountRecords < CountRecords ? CurrentPage * CountRecords + CountRecords : CountRecords;
                    TBCount.Text = CountRecords.ToString();
                    TBAllRecords.Text = " из " + ИсхаковаLanguageEntities.GetContext().Client.ToList().Count().ToString();

                    ClientlistView.ItemsSource = CurrentPageList;
                    ClientlistView.Items.Refresh();
                }
            }
        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var currentClien = (sender as Button).DataContext as Client;
            var currentClientServices = ИсхаковаLanguageEntities.GetContext().ClientService.ToList();
            currentClientServices = currentClientServices.Where(p => p.ClientID == currentClien.ID).ToList();
            if (currentClientServices.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как Клиент посещал");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ИсхаковаLanguageEntities.GetContext().Client.Remove(currentClien);
                        ИсхаковаLanguageEntities.GetContext().SaveChanges();
                        UpdateClient();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClient();
        }

        private void Pol_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClient();
        }

        private void Sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClient();
        }

        private void PagesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClient();
        }
  
        private void ClientlistView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateClient();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ClientForm(null));
            UpdateClient();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new ClientForm((sender as Button).DataContext as Client));
            UpdateClient();
        }
    }
}
