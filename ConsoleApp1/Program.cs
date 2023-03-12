using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;
using System.Reflection;
using System.Collections.ObjectModel;

namespace ReadingDataFromCSV
{
    class Program
    {
        static void Main(string[] args)
        {

            ReadCSVFile();
            Console.ReadLine();
        }
        static void ReadCSVFile()
        {
            string address = "C:\\Users\\Sefa\\Desktop\\misson_4\\ConsoleApp1\\country_vaccination_stats.csv";//changeable
            var lines = File.ReadAllLines(address);
            var list = new List<Contact>();
            for (int i = 1; i < lines.Length; i++)
            {
                var values = lines[i].Split(',');
                if (values.Length == 4)
                {
                    var contact = new Contact() { Country = values[0], Date = Convert.ToDateTime(values[1], CultureInfo.InvariantCulture), Daily_vaccinations = (values[2] == "") ? 0 : Convert.ToInt32(values[2]), Vaccines = values[3] };
                    list.Add(contact);
                }
                else if (values.Length == 5)
                {
                    var contact = new Contact() { Country = values[0], Date = Convert.ToDateTime(values[1], CultureInfo.InvariantCulture), Daily_vaccinations = (values[2] == "") ? 0 : Convert.ToInt32(values[2]), Vaccines = values[3] + "," + values[4] };
                    list.Add(contact);
                }
                else if (values.Length == 6)
                {
                    var contact = new Contact() { Country = values[0], Date = Convert.ToDateTime(values[1], CultureInfo.InvariantCulture), Daily_vaccinations = (values[2] == "") ? 0 : Convert.ToInt32(values[2]), Vaccines = values[3] + "," + values[4] + "," + values[5] };
                    list.Add(contact);
                }
            }


            var All_County_Median = new List<Country_Median>();
            foreach (var x in list)
            {
                int check = 0;
                foreach (var y in All_County_Median)
                {
                    if (y.Country == x.Country)
                        check = 1;

                }
                if (check == 0)
                {
                    var newadd = new Country_Median() { Country = x.Country, Median_Daily_vaccinations = 0 };
                    All_County_Median.Add(newadd);
                }

            }



            List<int> Country_Daily_vaccinations = new List<int>();
            int counter = 0;
            foreach (var x in All_County_Median)
            {


                Country_Daily_vaccinations.Clear();
                counter = 0;
                foreach (var y in list)
                {
                    if (x.Country == y.Country)
                    {
                        if (y.Daily_vaccinations != 0)
                        {
                            Country_Daily_vaccinations.Add(y.Daily_vaccinations);
                            counter++;
                        }

                    }

                }

                Country_Daily_vaccinations.Sort();

                //for (int i = 0; i < counter; i++)
                //    Console.WriteLine(Country_Daily_vaccinations[i].ToString());
                //Console.WriteLine();
                //Console.WriteLine();



                if (Country_Daily_vaccinations.Count % 2 == 0 && counter > 0)
                {
                    x.Median_Daily_vaccinations = ((Country_Daily_vaccinations[counter / 2] + Country_Daily_vaccinations[(counter / 2) - 1]) / 2);
                    //Console.WriteLine("cift :" + x.Median_Daily_vaccinations.ToString());

                }
                else if (counter == 1)
                {
                    x.Median_Daily_vaccinations = Country_Daily_vaccinations[counter - 1];
                    //Console.WriteLine("bir :" + x.Median_Daily_vaccinations.ToString());
                }
                else if (counter == 0)
                {
                    //Console.WriteLine("YOK YOK");
                }
                else
                {
                    x.Median_Daily_vaccinations = Country_Daily_vaccinations[counter / 2];
                    //Console.WriteLine("tek :" + x.Median_Daily_vaccinations.ToString());
                }


            }


            All_County_Median = All_County_Median.OrderByDescending(o => o.Median_Daily_vaccinations).ToList();
            int timer = 0;
            foreach (var x in All_County_Median)
            {
                Console.WriteLine(x.Country + " " + x.Median_Daily_vaccinations);
                timer++;
                if (timer == 3)
                    break;
            }


        }
        public class Contact
        {
            public string Country { get; set; }
            public DateTime Date { get; set; }
            public int Daily_vaccinations { get; set; }
            public string Vaccines { get; set; }

        }

        public class Country_Median
        {
            public string Country { get; set; }
            public int Median_Daily_vaccinations { get; set; }

        }
    }
}
