using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Internship
{
    public partial class Form1 : Form
    {
        List<TimeZone> timeZones = new List<TimeZone>();
        List<Location> locations = new List<Location>();
        public Form1()
        {
            InitializeComponent();
           
        }


        
        private void Form1_Load(object sender, EventArgs e)
        {
            SqlConnection conn = new Connection().GetConnection();
            conn.Open();
            SqlCommand cmdLoc = new SqlCommand("SELECT * FROM tb_locations", conn);
            SqlDataReader dr = cmdLoc.ExecuteReader();
            while (dr.Read())
            {
                comboBoxLocation.Items.Add(dr["location_name"]);
                locations.Add(new Location()
                {
                    location_id = ((int)dr["location_id"]),
                    location_name = dr["location_name"] as string
                });
            }
            conn.Close();
            conn.Open();
            SqlCommand cmdTZone = new SqlCommand("SELECT * FROM tb_timezones", conn);
            SqlDataReader dr1 = cmdTZone.ExecuteReader();
            while (dr1.Read())
            {
                timeZones.Add(new TimeZone()
                {
                    timezone_id = ((int)dr1["timezone_id"]),
                    timezone_name = dr1["timezone_name"] as string,
                    location_id = ((int)dr1["location_id"])
                });
            }
            conn.Close();
        }
        private string[] GetTZoneById(int id)
        {
            return timeZones.Where(line => line.location_id == id).Select(l => l.timezone_name).ToArray();
        }

        private void comboBoxLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxTimeZone.Items.Clear();
            int id = locations[comboBoxLocation.SelectedIndex].location_id;
            foreach (string name in GetTZoneById(id))
            {
                this.comboBoxTimeZone.Items.Add(name);
            }
        }
       

        private const string urlPattern = "http://rate-exchange-1.appspot.com/currency?from={0}&to={1}";
        public string CurrencyConversion(decimal amount, string fromCurrency, string toCurrency)
        {
            string url = string.Format(urlPattern, fromCurrency, toCurrency);

            using (var wc = new WebClient())
            {
                var json = wc.DownloadString(url);

                Newtonsoft.Json.Linq.JToken token = Newtonsoft.Json.Linq.JObject.Parse(json);
                decimal exchangeRate = (decimal)token.SelectToken("rate");

                return Math.Round(amount * exchangeRate,2).ToString();
            }
        }


        private void convertButton_Click(object sender, EventArgs e)
        {
            decimal amount = amountUpDown.Value;
            string from = fromCurrency.Text;
            string to = toCurrency.Text;

           notesTextBox.Text = CurrencyConversion(amount,from,to).ToString();
            
        }


    }

    class Location
    {
        public int location_id { get; set; }
        public string location_name { get; set; }
    }

    class TimeZone
    {
        public int timezone_id { get; set; }
        public string timezone_name { get; set; }
        public int location_id { get; set; }
    }
}
