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
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        PropFunctions propFunc = new PropFunctions();
        public static InspectData inspectedEmployee;
        public EditUserWindow()
        {
            InitializeComponent();
            body_datagrid.ItemsSource = LoadCollectionData();
            FillComboSpec();
            FillComboRolle();
            inspectedEmployee = new InspectData();
            inspectedEmployee.ID = -1;
        }

        private void EditEmployee(object sender, RoutedEventArgs e)
        {
            Ticket rowObj = body_datagrid.SelectedItem as Ticket;
            if (rowObj == null) return;
            int employeeID = rowObj.ID;
            string URL = "endpoints/klient/getEmployeesPerID.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
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
                inspectedEmployee.ID = data.ID;
                inspectedEmployee.Fornavn = data.Firstname;
                inspectedEmployee.Efternavn = data.Lastname;
                inspectedEmployee.SpecialitetID = data.Specialitet;
                inspectedEmployee.RolleID = data.Rolle;

                EmployeeNameLabel.Content = inspectedEmployee.Fornavn + " " + inspectedEmployee.Efternavn;
                RolleCombo.SelectedIndex = inspectedEmployee.RolleID - 1;
                SpecCombo.SelectedIndex = inspectedEmployee.SpecialitetID - 1;

            }
        }

        private void SaveEmployee(object sender, RoutedEventArgs e)
        {
            if (inspectedEmployee.ID == -1) return;
            ComboBoxItem selectedRolle = (ComboBoxItem)RolleCombo.SelectedItem;
            ComboBoxItem selectedSpec = (ComboBoxItem)SpecCombo.SelectedItem;
            string URL = "endpoints/klient/postUpdateEmployee.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "userID", Value = inspectedEmployee.ID},
                new RestParam { Name = "rolleID", Value = selectedRolle.Tag},
                new RestParam { Name = "specID", Value = selectedSpec.Tag},
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
                EmployeeNameLabel.Content = "INGEN PERSON VALGT";
                inspectedEmployee.ID = -1;
                body_datagrid.ItemsSource = LoadCollectionData();
            }
        }

        private List<Ticket> LoadCollectionData()
        {
            string URL = "endpoints/klient/getAllEmployees.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[]
            {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
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
                        Navn = employee.Navn,
                        Rolle = employee.Rolle,
                        Specialitet = employee.Specialitet,
                    });
                }
            }
            return tickets;

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

        private void FillComboRolle()
        {
            string URL = "endpoints/klient/getRoller.php";
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
                    RolleCombo.Items.Add(comboBoxItem);
                }
                RolleCombo.SelectedIndex = 0;
            }
        }
    }
}
