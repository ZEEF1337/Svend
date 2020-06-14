using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop_Klient.Models
{
    public class TicketData
    {
        public TicketData() { }
        public int ID { get; set; }
        public string CreationDate { get; set; }
        public string Kategori { get; set; }
        public string Titel { get; set; }
        public string Status { get; set; }
        public string Specialitet { get; set; }
        public string Navn { get; set; }
        public string Rolle { get; set; }
        public string Klok { get; set; }
        public string Body { get; set; }
    }
}
