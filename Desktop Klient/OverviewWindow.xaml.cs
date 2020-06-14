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
using RestSharp;
using Newtonsoft.Json;

namespace Desktop_Klient
{
    /// <summary>
    /// Interaction logic for OverviewWindow.xaml
    /// </summary>
    public partial class OverviewWindow : Window
    {
        PropFunctions propFunc = new PropFunctions();
        public static InspectData inspectedTicketData;
        public OverviewWindow()
        {
            InitializeComponent();
            UsernameLabel.Content = MainWindow.LoggedinUser.Firstname +" "+ MainWindow.LoggedinUser.Lastname;
            UserRoleLabel.Content = MainWindow.LoggedinUser.RolleNavn;

            body_datagrid.ItemsSource = LoadCollectionData();

            if(MainWindow.LoggedinUser.Rolle == 1)
            {
                AssignEmployeeToTicketBtn.Visibility = Visibility.Visible;
                EditUsersBtn.Visibility = Visibility.Visible;
            }
        }
        private List<Ticket> LoadCollectionData()
        {
            string URL = "endpoints/klient/getAssignedTickets.php";
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
                foreach (TicketData ticket in data.Records)
                {
                    tickets.Add(new Ticket()
                    {
                        ID = ticket.ID,
                        CreationDate = ticket.CreationDate,
                        Kategori = ticket.Kategori,
                        Titel = ticket.Titel,
                        Status = ticket.Status
                    });
                }
            }
            return tickets;
        }

        private void inspectTicket(object sender, RoutedEventArgs e)
        {
            inspectedTicketData = new InspectData();
            Ticket rowObj = body_datagrid.SelectedItem as Ticket;
            if (rowObj == null) return;
            int ticketID = rowObj.ID;


            string URL = "endpoints/klient/getInspectedTicketData.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "ticketID", Value = ticketID},
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
                inspectedTicketData.ID = ticketID;
                inspectedTicketData.Fornavn = data.Firstname;
                inspectedTicketData.Efternavn = data.Lastname;
                inspectedTicketData.Titel = data.Titel;
                inspectedTicketData.Body = data.Body;
                inspectedTicketData.CreationDate = data.CreationDate;
                inspectedTicketData.Klok = data.Klok;
                inspectedTicketData.Rolle = data.RolleNavn;
                inspectedTicketData.Status = data.Status;
                inspectedTicketData.StatusID = data.StatusID;
                InspectWindow iWin = new InspectWindow();
                iWin.ShowDialog();
            }



            
        }

        private void assignEmployeeToTicket(object sender, RoutedEventArgs e)
        {
            inspectedTicketData = new InspectData();
            Ticket rowObj = body_datagrid.SelectedItem as Ticket;
            if (rowObj == null) return;
            int ticketID = rowObj.ID;

            string URL = "endpoints/klient/getInspectedTicketData.php";
            Method RestType = Method.GET;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "ticketID", Value = ticketID},
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
                inspectedTicketData.ID = ticketID;
                inspectedTicketData.Titel = data.Titel;
                inspectedTicketData.Kategori = data.Kategori;
                AssignWindow aWin = new AssignWindow();
                aWin.ShowDialog();
            }
        }

        private void editUsers(object sender, RoutedEventArgs e)
        {
            EditUserWindow euWin = new EditUserWindow();
            euWin.ShowDialog();
        }
    }
}
