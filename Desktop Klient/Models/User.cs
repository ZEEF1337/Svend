using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop_Klient.Models
{
    public class User
    {
        public User() { }
        public string Username { get; set; }
        public string Token { get; set; }
        public int Rolle { get; set; }
        public string RolleNavn { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
    }
}
