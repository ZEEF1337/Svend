using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Desktop_Klient.Models
{
    class Response
    {
        [JsonProperty("Result")]
        public int Result { get; set; }


        [JsonProperty("Message")]
        public string Message { get; set; }


        [JsonProperty("Token")]
        public string Token { get; set; }


        [JsonProperty("Rolle")]
        public int Rolle { get; set; }


        [JsonProperty("RolleNavn")]
        public string RolleNavn { get; set; }


        [JsonProperty("Fornavn")]
        public string Firstname { get; set; }


        [JsonProperty("Efternavn")]
        public string Lastname { get; set; }


        [JsonProperty("Titel")]
        public string Titel { get; set; }


        [JsonProperty("Body")]
        public string Body { get; set; }


        [JsonProperty("CreationDate")]
        public string CreationDate { get; set; }


        [JsonProperty("Klok")]
        public string Klok { get; set; }


        [JsonProperty("Status")]
        public string Status { get; set; }


        [JsonProperty("StatusID")]
        public int StatusID { get; set; }


        [JsonProperty("Kategori")]
        public string Kategori { get; set; }


        [JsonProperty("KategoriID")]
        public int KategoriID { get; set; }


        [JsonProperty("Records")]
        public List<TicketData> Records { get; set; }
    }
}
