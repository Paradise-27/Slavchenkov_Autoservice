using Slavchenkov_Autoservice;
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

namespace Familiya_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для SignUpPage.xaml
    /// </summary>
    public partial class SignUpPage : Page
    {
        public SignUpPage()
        {
            InitializeComponent();
        }
        private Service _currentService = new Service();
        public SignUpPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null) 
            this._currentService = SelectedService;
            DataContext = _currentService;
            var _currentClient = Славченков_автосервисEntities1.GetContext().Client.ToList();
            ComboClient.ItemsSource = _currentClient;
        }
        private ClientService _currentClientService = new ClientService();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboClient.SelectedItem == null)
            errors.AppendLine("Укажите ФИО клиента");
            if (StartDate.Text == "")
                errors.AppendLine("Укажите дату услуги");
            if (TBStart.Text == "")
                errors.AppendLine("Укажите время начала услуги");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            _currentClientService.ClientID = ComboClient.SelectedIndex + 1;
            _currentClientService.ServiceID = _currentService.ID;
            _currentClientService.StartTime = Convert.ToDateTime(StartDate.Text + " " + TBStart.Text);
            if (_currentClientService.ID == 0)
                Славченков_автосервисEntities1.GetContext().ClientService.Add(_currentClientService);
            try
            {
                Славченков_автосервисEntities1.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void TBStart_TextChanged(object sender, TextChangedEventArgs e)
        {
            string s = TBStart.Text;
            if (s.Length < 4 || !s.Contains(":"))
            {
                TBEnd.Text = " ";
            }
            else
            {
                string[] start = s.Split(new char[] { ':' });
                int startHour = Convert.ToInt32(start[0].ToString()) * 60;
                int startMin = Convert.ToInt32(start[1].ToString());
                int sum = startHour + startMin + _currentService.Duration;
                if (sum >= 1440)
                {
                    TBEnd.Text = "Недопустимое время";
                }
                else
                {
                    int EndHour = sum / 60;
                    int EndMin = sum % 60;
                    s = EndHour.ToString() + ":" + EndMin.ToString();
                    TBEnd.Text = s;
                }
            }
        }

        private void TBStart_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, 0) && e.Text != ":")
                e.Handled = true;
        }
    }
}
