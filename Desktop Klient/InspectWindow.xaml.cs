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
    /// Interaction logic for InspectWindow.xaml
    /// </summary>
    public partial class InspectWindow : Window
    {
        PropFunctions propFunc = new PropFunctions();
        public InspectWindow()
        {
            InitializeComponent();
            FillComboStatus();
            InsertTicket();
            InsertReplies();
        }


        private void InsertTicket()
        {
            TicketTitle.Content = OverviewWindow.inspectedTicketData.Titel;
            TicketBody.Text = "[" + OverviewWindow.inspectedTicketData.Fornavn+" "+OverviewWindow.inspectedTicketData.Efternavn+ "] " +
                "["+ OverviewWindow.inspectedTicketData.Rolle +"] " +
                OverviewWindow.inspectedTicketData.CreationDate +" - "+ OverviewWindow.inspectedTicketData.Klok +
                "\n"+ OverviewWindow.inspectedTicketData.Body;
            TicketBody.Margin = new Thickness(0, 0, 0, 6);
            Rectangle divider = new Rectangle();
            divider.Fill = Brushes.Black;
            divider.Width = 800;
            divider.Height = 1;
            divider.Margin = new Thickness(0, 0, 0, 6);
            StackPanel.Children.Add(divider);
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        private void InsertReplies()
        {
            string URL = "endpoints/klient/getTicketReplies.php";
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
                return;
            }
            Response data = JsonConvert.DeserializeObject<Response>(content);
            if (data.Result == 1)
            {
                foreach (TicketData reply in data.Records)
                {
                    TextBlock replyBlock = new TextBlock();
                    replyBlock.TextWrapping = TextWrapping.Wrap;
                    replyBlock.Margin = new Thickness(0, 0, 0, 6);
                    replyBlock.Text = "[" + reply.Navn + "] " +
                        "[" + reply.Rolle + "] " +
                        reply.CreationDate + " - " + reply.Klok +
                        "\n" + reply.Body;

                    Rectangle divider = new Rectangle();
                    divider.Fill = Brushes.Black;
                    divider.Width = 800;
                    divider.Height = 1;
                    divider.Margin = new Thickness(0, 0, 0, 6);


                    StackPanel.Children.Add(replyBlock);
                    StackPanel.Children.Add(divider);
                }
                scrollViewer.Content = StackPanel;
            }
            return;
        }

        private void SendReply(object sender, RoutedEventArgs e)
        {
            string givenReply = ReplyTextBox.Text;

            string URL = "endpoints/klient/postTicketReply.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[]
            {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "body", Value = givenReply},
                new RestParam { Name = "ticketID", Value = OverviewWindow.inspectedTicketData.ID},
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
                this.Close();
            }
        }

        private void SetStatus(object sender, RoutedEventArgs e)
        {
            ComboBoxItem selectedStatus = (ComboBoxItem)StatusCombo.SelectedItem;
            string URL = "endpoints/klient/postStatus.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[] {
                new RestParam { Name = "token", Value = MainWindow.LoggedinUser.Token},
                new RestParam { Name = "ticketID", Value = OverviewWindow.inspectedTicketData.ID},
                new RestParam { Name = "status", Value = selectedStatus.Tag},
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
                MessageBox.Show(data.Message);
            }
        }

        private void FillComboStatus()
        {
            string URL = "endpoints/klient/getStatus.php";
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
                foreach (TicketData status in data.Records)
                {
                    ComboBoxItem comboBoxItem = new ComboBoxItem();
                    comboBoxItem.Content = status.Titel;
                    comboBoxItem.Tag = status.ID;
                    StatusCombo.Items.Add(comboBoxItem);
                }
                StatusCombo.SelectedIndex = OverviewWindow.inspectedTicketData.StatusID -1;
            }
        }
    }
}
