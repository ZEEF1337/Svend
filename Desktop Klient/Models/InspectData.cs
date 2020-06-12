using System;
using System.Collections.Generic;
using System.Text;

namespace Desktop_Klient.Models
{
    public class InspectData
    {
        public InspectData() { }
        public int ID { get; set; }
        public string Fornavn { get; set; }
        public string Efternavn { get; set; }
        public string Titel { get; set; }
        public string Body { get; set; }
        public string CreationDate { get; set; }
        public string Klok { get; set; }
        public string Status { get; set; }
        public int StatusID { get; set; }
        public string Kategori { get; set; }
        public int KategoriID { get; set; }
    }
}
