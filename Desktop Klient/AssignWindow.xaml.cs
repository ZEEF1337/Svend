using Desktop_Klient.Models;
using Desktop_Klient.Functions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using RestSharp;

namespace Desktop_Klient
{
    /// <summary>
    /// Interaction logic for AssignWindow.xaml
    /// </summary>
    public partial class AssignWindow : Window
    {
        PropFunctions propFunc = new PropFunctions();
        public AssignWindow()
        {
            InitializeComponent();
            FillComboSpec();
            TicketTitleLabel.Content = OverviewWindow.inspectedTicketData.Titel;
            TicketCategoriLabel.Content = "[" + OverviewWindow.inspectedTicketData.Kategori + "]";
            body_datagrid.ItemsSource = LoadCollectionData();
        }

        private void FillComboSpec()
        {
            string URL = "endpoints/klient/getSpecialiteter.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            if (content == "")
            {
                MessageBox.Show("Rest fejl");
                return;
            }

            Response data = JsonConvert.DeserializeObject<Response>(content);
            if (data.Result == 1)
            {
                foreach (TicketData spec in data.Records)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = spec.Titel;
                    comboBoxItem.Tag = spec.ID;
                    SpecCombo.Items.Add(comboBoxItem);
                }
                SpecCombo.SelectedIndex = 0;
            }
        }


        private void FillComboEmployees()
        {
            ComboBoxItem selectedSpec = (ComboBoxItem)SpecCombo.SelectedItem;
            string URL = "endpoints/klient/getEmployees.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "spec", Value = selectedSpec.Tag},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            if (content == "")
            {
                MessageBox.Show("Rest fejl");
                return;
            }

            Response data = JsonConvert.DeserializeObject<Response>(content);
            if (data.Result == 1)
            {
                foreach (TicketData employee in data.Records)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = employee.Titel;
                    comboBoxItem.Tag = employee.ID;
                    EmployeeCombo.Items.Add(comboBoxItem);
                }
                EmployeeCombo.SelectedIndex = 0;
            }
        }

        private void Search4Employees(object sender, RoutedEventArgs e)
        {
            EmployeeCombo.Items.Clear();
            FillComboEmployees();
        }



        private List<Ticket> LoadCollectionData()
        {
            string URL = "endpoints/klient/getAssignedEmployees.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[]
            {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "ticketID", Value = OverviewWindow.inspectedTicketData.ID},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            if (content == "")
            {
                MessageBox.Show("Rest fejl");
                List<Ticket> emptyList = new List<Ticket>();
                return emptyList;
            }
            Response data = JsonConvert.DeserializeObject<Response>(content);
            List<Ticket> tickets = new List<Ticket>();
            if (data.Result == 1)
            {
                foreach (TicketData employee in data.Records)
                {
                    tickets.Add(new Ticket()
                    {
                        ID = employee.ID,
                        Titel = employee.Titel,
                        Specialitet = employee.Specialitet,
                    });
                }
            }
            return tickets;

        }

        private void assignEmployee(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedEmployee = (ComboBoxItem)EmployeeCombo.SelectedItem;
            string URL = "endpoints/klient/postAssignToTicket.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[]
            {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "ticketID", Value = OverviewWindow.inspectedTicketData.ID},
                new RestParam { Name = "userID", Value = selectedEmployee.Tag},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            if (content == "")
            {
                MessageBox.Show("Rest fejl");
                return;
            }
            Response data = JsonConvert.DeserializeObject<Response>(content);
            if (data.Result == 1)
            {
                body_datagrid.ItemsSource = LoadCollectionData();
            }
        }

        private void removeEmployeeFromTicket(object sender, RoutedEventArgs e)
        {
            Ticket rowObj = body_datagrid.SelectedItem as Ticket;
            if (rowObj == null) return;
            int employeeID = rowObj.ID;
            string URL = "endpoints/klient/postRemoveFromTicket.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "ticketID", Value = OverviewWindow.inspectedTicketData.ID},
                new RestParam { Name = "userID", Value = employeeID},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            if (content == "")
            {
                MessageBox.Show("Rest fejl");
                return;
            }
            Response data = JsonConvert.DeserializeObject<Response>(content);
            if (data.Result == 1)
            {
                body_datagrid.ItemsSource = LoadCollectionData();
            }
        }
    }
}
