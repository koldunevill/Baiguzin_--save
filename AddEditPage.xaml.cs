using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        public bool InAgentExist = false;

        private Agent _currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if (SelectedAgent != null)
            {
                InAgentExist = true;
                _currentAgent = SelectedAgent;
            }
            else
            {
                HistoryBtn.Visibility = Visibility.Hidden;
            }
            DataContext = _currentAgent;
        }

        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if (myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора агента");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");
            if (_currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительный приоритет агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            else
            {
                string ph = _currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) ||
                    (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("Укажите корректный номер агента");
            }
            if (string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentAgent.AgentTypeID = ComboType.SelectedIndex + 1;

            var allAgent = Baiguzin_glazkiEntities.GetContext().Agent.ToList();
            allAgent = allAgent.Where(p => p.Title == _currentAgent.Title).ToList();
            if (allAgent.Count == 0 || InAgentExist == true)
            {
                if (_currentAgent.ID == 0)
                    Baiguzin_glazkiEntities.GetContext().Agent.Add(_currentAgent);

                try
                {
                    Baiguzin_glazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    Manager.MainFrame.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            else
            {
                Baiguzin_glazkiEntities.GetContext().SaveChanges();
                MessageBox.Show("Уже существует такая услуга");
            }
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = (sender as Button).DataContext as Agent;

            var currentAgentSale = Baiguzin_glazkiEntities.GetContext().ProductSale.ToList();
            currentAgentSale = currentAgentSale.Where(p => p.AgentID == currentAgent.ID).ToList();

            if (currentAgentSale.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo,
                     MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        Baiguzin_glazkiEntities.GetContext().Agent.Remove(currentAgent);
                        Baiguzin_glazkiEntities.GetContext().SaveChanges();
                        Manager.MainFrame.GoBack();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }

        private void HistoryBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AgentSalePage(_currentAgent));
        }
    }
}
