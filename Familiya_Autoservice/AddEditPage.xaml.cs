using Familiya_Autoservice;
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

namespace Slavchenkov_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Service _currentService = new Service();
        public AddEditPage(Service SelectedService)
        {
            InitializeComponent();
            if (SelectedService != null)
                _currentService = SelectedService;
            DataContext = _currentService;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder Errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentService.Title))
                Errors.AppendLine("Укажите название услуги");
            if (_currentService.Cost <= 0)
                Errors.AppendLine("Укажите стоимость услуги");
            if (_currentService.Discount <= 0 || _currentService.Discount> 100)
                Errors.AppendLine("Укажите скидку от 0 до 100");
            if (_currentService.Duration <0||_currentService.Duration>240)
                Errors.AppendLine("Длительность не может быть больше 240 минут или меньше 0");
            if (Errors.Length > 0)
            {
                MessageBox.Show(Errors.ToString());
                return;
            }
            var AllServices = Славченков_автосервисEntities1.GetContext().Service.ToList();
            AllServices = AllServices.Where(p => p.Title == _currentService.Title).ToList();
            if (AllServices.Count == 0)
            {
                if (_currentService.ID == 0)
                {
                    Славченков_автосервисEntities1.GetContext().Service.Add(_currentService);
                }
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
            else
            {
                MessageBox.Show("Уже существует такая услуга");
            }
        }
    }
}
