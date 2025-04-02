using IbragimovIlshatLanguage;
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

namespace IbragimovIlshatLanguage
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
            var currentClient = IbragimovIlshatLanguageEntities.GetContext().Client.ToList();
            ClientListView.ItemsSource = currentClient;

            OutputComboBox.SelectedIndex = 3;
            SortComboBox.SelectedIndex = 0;
            FiltComboBox.SelectedIndex = 0;
            UpdateClients();

        }

        public void UpdateClients()
        {
            var currentClient = IbragimovIlshatLanguageEntities.GetContext().Client.ToList();

            switch (FiltComboBox.SelectedIndex)
            {
                case 1:
                    currentClient = currentClient.Where(p => p.GenderCode == "1").ToList();
                    break;
                case 2:
                    currentClient = currentClient.Where(p => p.GenderCode == "2").ToList();
                    break;
            }

            currentClient = currentClient.Where(p => p.FullFIO.ToLower().Contains(SearchTB.Text.ToLower()) || p.Email.ToLower().Contains(SearchTB.Text.ToLower()) || p.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "").Contains(SearchTB.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))).ToList();


            if (SortComboBox.SelectedIndex == 1)
            {
                currentClient = currentClient.OrderBy(p => p.FirstName).ToList();
            }
            if (SortComboBox.SelectedIndex == 2)
            {
                currentClient = currentClient.OrderByDescending(p => p.LastVisitDate).ToList();
            }

            if (SortComboBox.SelectedIndex == 3)
                currentClient = currentClient.OrderByDescending(p => p.VisitCount).ToList();

            ClientListView.ItemsSource = currentClient;


            FirstPageCountTB.Text = currentClient.Count.ToString();
            SecondPageCountTB.Text = " из " + IbragimovIlshatLanguageEntities.GetContext().Client.ToList().Count().ToString();

            TableList = currentClient;
            ChangePage(0, 0);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            int selectedOutput = OutputComboBox.SelectedIndex;
            int RecordsOutputPages = 0;

            CurrentPageList.Clear();

            CountRecords = TableList.Count;

            switch (selectedOutput)
            {
                case 0:
                    RecordsOutputPages = 10; 
                    break;
                case 1:
                    RecordsOutputPages = 50; 
                    break;
                case 2:
                    RecordsOutputPages = 200; 
                    break;
                case 3:
                    RecordsOutputPages = CountRecords == 0 ? 1 : CountRecords;
                    break;
            }
            
            if (CountRecords % RecordsOutputPages > 0)
                CountPage = CountRecords / RecordsOutputPages + 1;
            else
                CountPage = CountRecords / RecordsOutputPages;

            Boolean ifUpdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * RecordsOutputPages + RecordsOutputPages < CountRecords ? CurrentPage * RecordsOutputPages + RecordsOutputPages : CountRecords;
                    for (int i = CurrentPage * RecordsOutputPages; i < min; i++)
                        CurrentPageList.Add(TableList[i]);
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
                            min = CurrentPage * RecordsOutputPages + RecordsOutputPages < CountRecords ? CurrentPage * RecordsOutputPages + RecordsOutputPages : CountRecords;
                            for (int i = CurrentPage * RecordsOutputPages; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else
                            ifUpdate = false;
                        break;
                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * RecordsOutputPages + RecordsOutputPages < CountRecords ? CurrentPage * RecordsOutputPages + RecordsOutputPages : CountRecords;
                            for (int i = CurrentPage * RecordsOutputPages; i < min; i++)
                                CurrentPageList.Add(TableList[i]);
                        }
                        else
                            ifUpdate = false;
                        break;
                }
            }
            if (ifUpdate)
            {
                PageListBox.Items.Clear();

                for (int i = 1; i <= CountPage; i++)
                    PageListBox.Items.Add(i);
                PageListBox.SelectedIndex = CurrentPage;

                SecondPageCountTB.Text = " из " + IbragimovIlshatLanguageEntities.GetContext().Client.ToList().Count().ToString(); //
                min = CurrentPage * RecordsOutputPages + RecordsOutputPages < CountRecords ? CurrentPage * RecordsOutputPages + RecordsOutputPages : CountRecords;
                //FirstPageCountTB.Text = min.ToString(); 

                ClientListView.ItemsSource = CurrentPageList;
                ClientListView.Items.Refresh();
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentClient = (sender as Button).DataContext as Client;

            var currentClientService = IbragimovIlshatLanguageEntities.GetContext().ClientService.ToList();
            currentClientService = currentClientService.Where(p => p.ClientID == currentClient.ID).ToList();

            if (currentClientService.Count != 0)
                MessageBox.Show("Есть информация о посещениях, удаление невозможно!");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        IbragimovIlshatLanguageEntities.GetContext().Client.Remove(currentClient);
                        IbragimovIlshatLanguageEntities.GetContext().SaveChanges();
                        ClientListView.ItemsSource = IbragimovIlshatLanguageEntities.GetContext().Client.ToList();
                        UpdateClients();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void OutputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void FiltComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void SearchTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateClients();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateClients();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Client));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                IbragimovIlshatLanguageEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ClientListView.ItemsSource = IbragimovIlshatLanguageEntities.GetContext().Client.ToList();

                UpdateClients();
            }
        }
    }
}

 
