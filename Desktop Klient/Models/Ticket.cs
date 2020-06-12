using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop_Klient.Models
{
    class Ticket
    {
        public Ticket() { }
        public int ID { get; set; }
        public string CreationDate { get; set; }
        public string Kategori { get; set; }
        public string Titel { get; set; }
        public string Status { get; set; }
        public string Specialitet { get; set; }
    }
}
