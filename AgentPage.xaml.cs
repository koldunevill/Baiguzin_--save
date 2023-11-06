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

namespace Baiguzin_Глазки_save
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();

            var currentAgent = Baiguzin_glazkiEntities.GetContext().Agent.ToList();

            AgentListView.ItemsSource = currentAgent;

            ComboType.SelectedIndex = 0;
            ComboType2.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboType2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();  
        }
        private void UpdateAgent()
        {
            var currentAgent = Baiguzin_glazkiEntities.GetContext().Agent.ToList();


            if (ComboType.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }

            if (ComboType2.SelectedIndex == 0)
            {
                currentAgent = currentAgent;
            }
            if (ComboType2.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeString == "МФО").ToList();
            }
            if (ComboType2.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeString == "ЗАО").ToList();
            }
            if (ComboType2.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeString == "МКК").ToList();
            }
            if (ComboType2.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeString == "ОАО").ToList();
            }
            if (ComboType2.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeString == "ПАО").ToList();
            }
            if (ComboType2.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeString == "ООО").ToList();
            }

            currentAgent = currentAgent.Where(p => PhoneFormat(p.Phone.ToLower()).Trim().Contains(TBoxSearch.Text.ToLower()) || p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) 
            ||  p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            AgentListView.ItemsSource = currentAgent;

        }
        private string PhoneFormat(string phone)
        {
            return phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
        }
    }
}
