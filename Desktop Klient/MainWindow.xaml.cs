using Desktop_Klient.Models;
using Desktop_Klient.Functions;
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
using RestSharp;
using Newtonsoft.Json;

namespace Desktop_Klient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        PropFunctions propFunc = new PropFunctions();
        public static User LoggedinUser;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login(object sender, RoutedEventArgs e)
        {
            string givenUsername = BrugernavnInput.Text.ToString();
            string givenPassword = AdgangskodeInput.Password.ToString();
            LoggedinUser = new User();


            string URL = "endpoints/klient/login.php";
            Method RestType = Method.POST;
            RestParam[] Params = new RestParam[]
            {
                new RestParam { Name = "brugernavn", Value = givenUsername},
                new RestParam { Name = "password", Value = givenPassword},
            };
            var content = propFunc.CallRest(URL, Params, RestType);
            if (content == "")
            {
                MessageBox.Show("Rest fejl");
                return;
            }

            Response data = JsonConvert.DeserializeObject<Response>(content);
            if(data.Result == 0)
            {
                MessageBox.Show(data.Message);
                return;
            }
            else if (data.Result == 1)
            {
                LoggedinUser.Username = givenUsername;
                LoggedinUser.Token = data.Token;
                LoggedinUser.Rolle = data.Rolle;
                LoggedinUser.RolleNavn = data.RolleNavn;
                LoggedinUser.Firstname = data.Firstname;
                LoggedinUser.Lastname = data.Lastname;
                MessageBox.Show(data.Message);
                OverviewWindow oWin = new OverviewWindow();
                this.Close();
                oWin.ShowDialog();
            }
            
        }
    }
}
