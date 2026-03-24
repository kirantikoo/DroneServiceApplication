using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DroneServiceLib.Interfaces;
using DroneServiceLib.Models;
using DroneServiceLib.Services;



namespace DroneServiceApplication
{
    public partial class MainWindow : Window
    {
        private readonly ServiceManager serviceManager;
        public MainWindow()
        {
            InitializeComponent();

            serviceManager = new ServiceManager();


            txtServiceTag.Text = "100";
            rbRegular.IsChecked = true;
            DisplayRegularService();
            DisplayExpressService();
            DisplayFinishedList();
            UpdateStatus("Ready.");
        }

        public class ProductDisplayItem
        {
            public string ClientName { get; set; } = string.Empty;
            public string Model { get; set; } = string.Empty;
            public string ServiceProblem { get; set; } = string.Empty;
            public string ServiceCostFormatted { get; set; } = string.Empty;
            public int ServiceTag { get; set; }
        }

        private void AddNewItem(Object sender, RoutedEventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                {
                    return;
                }

                double serviceCost = ValidateServiceCost();
                if (serviceCost < 0)
                {
                    return;
                }

                string clientName = txtClientName.Text;
                string model = txtDroneModel.Text;
                string problem = txtServiceProblem.Text;
                double ServiceCost = serviceCost;
                int tag = int.Parse(txtServiceTag.Text);
                string priority = serviceManager.GetServicePriority(rbExpress.IsChecked == true);

                serviceManager.AddNewItem(new Drone(clientName, model, problem, serviceCost, tag), priority);

                DisplayRegularService();
                DisplayExpressService();
                DisplayFinishedList();

                txtServiceTag.Text = serviceManager.GetNextServiceTag(int.Parse(txtServiceTag.Text)).ToString();
                ClearInputFields();

                UpdateStatus($"{priority} service item added successfully.");
            }
            catch (Exception ex)
            {
                UpdateStatus("Error adding item: " + ex.Message);
            }
        }

        private void DisplayRegularService()
        {
            lvRegularService.ItemsSource = null;

            var items = serviceManager.GetRegularService()
                .Select(item => new ProductDisplayItem
                {
                    ClientName = item.GetClientName(),
                    Model = item.GetModel(),
                    ServiceProblem = item.GetServiceProblem(),
                    ServiceCostFormatted = "$" + item.GetServiceCost().ToString("F2"),
                    ServiceTag = item.GetServiceTag()
                })
                .ToList();

            lvRegularService.ItemsSource = items;
        }

        private void DisplayExpressService()
        {
            lvExpressService.ItemsSource = null;
            var items = serviceManager.GetExpressService()
                .Select(item => new ProductDisplayItem
                {
                    ClientName = item.GetClientName(),
                    Model = item.GetModel(),
                    ServiceProblem = item.GetServiceProblem(),
                    ServiceCostFormatted = "$" + item.GetServiceCost().ToString("F2"),
                    ServiceTag = item.GetServiceTag()
                })
                .ToList();
            lvExpressService.ItemsSource = items;
        }

        private void DisplayFinishedList()
        {
            lstFinishedServices.ItemsSource = null;

            var items = serviceManager.GetFinishedList()
                .Select(item => item.Display())
                .ToList();

            lstFinishedServices.ItemsSource = items;
        }

        private double ValidateServiceCost()
        {
            if (string.IsNullOrWhiteSpace(txtServiceCost.Text))
            {
                UpdateStatus("Service Cost is required.");
                return -1;
            }

            if (!double.TryParse(
                txtServiceCost.Text,
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out double cost))
            {
                UpdateStatus("Service Cost must be a valid number.");
                return -1;
            }

            string[] parts = txtServiceCost.Text.Split('.');
            if (parts.Length == 2 && parts[1].Length > 2)
            {
                UpdateStatus("Service Cost can only have 2 decimal places.");
                return -1;
            }
            if (cost < 0)
            {
                UpdateStatus("Service Cost cannot be negative.");
                return -1;
            }
            return cost;
        }

        private void lvRegularService_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IProduct? selectedItem = serviceManager.PeekRegular();
            if (selectedItem != null)
            {
                txtClientName.Text = selectedItem.GetClientName();
                txtDroneModel.Text = selectedItem.GetModel();
                txtServiceProblem.Text = selectedItem.GetServiceProblem();
                txtServiceCost.Text = selectedItem.GetServiceCost().ToString("F2");
                txtServiceTag.Text = selectedItem.GetServiceTag().ToString();
                rbRegular.IsChecked = true;

                UpdateStatus("Regular queue item displayed.");
            }
            else
            {
                UpdateStatus("Regular queue is empty.");
            }
        }

        private void lvExpressService_MouseUp(object sender, MouseButtonEventArgs e)
        {
            IProduct? selectedItem = serviceManager.PeekExpress();

            if (selectedItem != null)
            {
                txtClientName.Text = selectedItem.GetClientName();
                txtDroneModel.Text = selectedItem.GetModel();
                txtServiceProblem.Text = selectedItem.GetServiceProblem();
                txtServiceCost.Text = selectedItem.GetServiceCost().ToString("F2");
                txtServiceTag.Text = selectedItem.GetServiceTag().ToString();
                rbExpress.IsChecked = true;

                UpdateStatus("Express queue item displayed.");
            }
            else
            {
                UpdateStatus("Express queue is empty.");
            }
        }

        private void btnProcessRegular_Click(object sender, RoutedEventArgs e)
        {
            IProduct? finishedItem = serviceManager.ProcessRegular();

            if (finishedItem == null)
            {
                UpdateStatus("Regular queue is empty.");
                return;
            }

            DisplayRegularService();
            DisplayFinishedList();
            UpdateStatus("Regular service item moved to finished list.");
        }

        private void btnProcessExpress_Click(object sender, RoutedEventArgs e)
        {
            IProduct? finishedItem = serviceManager.ProcessExpress();
            if (finishedItem == null)
            {
                UpdateStatus("Express queue is empty.");
                return;
            }

            DisplayExpressService();
            DisplayFinishedList();
            UpdateStatus("Express service item moved to finished list.");
        }

        private void lstFinishedServices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            int selectedIndex = lstFinishedServices.SelectedIndex;

            if (serviceManager.RemoveFinishedItemAt(selectedIndex))
            {
                DisplayFinishedList();
                UpdateStatus("Finished service item removed after payment.");
            }
            else
            {
                UpdateStatus("Please select a finished item to remove.");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtClientName.Text))
            {
                UpdateStatus("Client Name is required.");
                txtClientName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtDroneModel.Text))
            {
                UpdateStatus("Drone Model is required.");
                txtDroneModel.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtServiceProblem.Text))
            {
                UpdateStatus("Service Problem is required.");
                txtServiceProblem.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtServiceTag.Text))
            {
                UpdateStatus("Service Tag is required.");
                return false;
            }
            if (!int.TryParse(txtServiceTag.Text, out int tagValue))
            {
                UpdateStatus("Service Tag must be numeric.");
                return false;
            }

            if (tagValue < 100 || tagValue > 900 || tagValue % 10 != 0)
            {
                UpdateStatus("Service Tag must be between 100 and 900 and increment by 10.");
                return false;
            }

            return true;
        }

        private void txtServiceCost_LostFocus(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(txtServiceCost.Text, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double cost))
            {
                txtServiceCost.Text = cost.ToString("F2");
            }
        }

        private void ClearInputFields()
        {
            txtClientName.Clear();
            txtDroneModel.Clear();
            txtServiceProblem.Clear();
            txtServiceCost.Clear();

            rbRegular.IsChecked = true;
            txtClientName.Focus();
        }

        private void UpdateStatus(string message)
        {
            txtStatus.Text = message;
        }

        private void txtServiceCost_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            if (sender is not TextBox textbox)
            {
                e.Handled = true;
                return;
            }

            string proposedText = textbox.Text.Insert(textbox.SelectionStart, e.Text);
            Regex regex = new Regex(@"^\d*([.]\d{0,2})?$");
            e.Handled = !regex.IsMatch(proposedText);
        }


        private void txtServiceTag_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(txtServiceTag.Text, out int tagValue))
            {
                if (tagValue < 100 || tagValue > 900 || tagValue % 10 != 0)
                {
                    UpdateStatus("Service Tag must be between 100 and 900 and increment by 10.");
                }
                else
                {
                    UpdateStatus("Service Tag is valid.");
                }
            }
            else
            {
                UpdateStatus("Service Tag must be a valid integer.");
            }
        }      
    }
}