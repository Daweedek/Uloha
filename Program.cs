using System;
using System.Net;
using Newtonsoft.Json; // pro pouziti JsonConvert
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UlohaNaPohovor
{
    public class Check
    {
        public static string status = "";
        public static bool isValid = true;

        public static void State(int id)
        {
            if (isValid == false)
            {
                status = status.Remove(status.Length - 2);
                Console.WriteLine(id + ") " + status);
                status = "";
                isValid = true;
            }
            else
            {
                Console.WriteLine(id + ") V pořádku.");
            }
        }

        public static void Id(int id)
        {
            if (id < 0)
            {
                isValid = false;
                status = status.Insert(status.Length, "chybí 'id', ");
            }
            else
            {
                ;
            }
        }

        public static void FirstName(string nameFirst)
        {
            if (nameFirst.Length < 1)
            {
                isValid = false;
                status = status.Insert(status.Length, "chybí 'nameFirst', ");
            }
            else
            {
                if (Char.IsUpper(nameFirst[0]))
                {
                    ;
                }
                else
                {
                    isValid = false;
                    status = status.Insert(status.Length, "'nameFirst' má malé písmeno na začátku, ");
                }
            }
        }

        public static void LastName(string nameLast)
        {
            if (nameLast.Length < 1)
            {
                isValid = false;
                status = status.Insert(status.Length, "chybí 'nameLast', ");
            }
            else
            {
                if (Char.IsUpper(nameLast[0]))
                {
                    ;
                }
                else
                {
                    isValid = false;
                    status = status.Insert(status.Length, "'nameLast' má malé písmeno na začátku, ");
                }
            }
        }

        public static void Phone(string phone)
        {
            if (phone.Length < 1)
            {
                isValid = false;
                status = status.Insert(status.Length, "chybí 'phone', ");
            }
            else
            {
                //kontrola čísla podle pravidel
                if (Regex.Match(phone, @"\(?\d{3}\)?-? *\d{3}-? *-?\d{3}").Success)
                {
                    ;
                }
                else
                {
                    isValid = false;
                    status = status.Insert(status.Length, "'phone' je ve špatném formátu, ");
                }
            }
        }

        public static void Email(string email)
        {
            if (email.Length < 1)
            {
                isValid = false;
                status = status.Insert(status.Length, "chybí 'email', ");
            }
            else
            {
                // kontrola emailu podle pravidel
                if (Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
                {
                    ;
                }
                else
                {
                    isValid = false;
                    status = status.Insert(status.Length, "'email' je ve špatném formátu, ");
                }
            }
        }
    }
    class UserFromAPI
    {
        // verejna promenna - integer/string - kdy get dostane hodnotu a set ji nastavi z 'userList'
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string nameLast { get; set; }
        public string nameFirst { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            // import JSON souboru a ulozeni jej jako string
            string jsonString = new WebClient().DownloadString("https://api.mocki.io/v1/6bb78356");

            // uprava JSON souboru -> na tvar seznamu, aby jej bylo mozne deserializovat (odstraneni znaku na zacatku a konci)
            string editedJsonString = jsonString;
            editedJsonString = editedJsonString.Remove(jsonString.Length - 1);
            editedJsonString = editedJsonString.Remove(0, 8);

            // deserializace seznamu na jednotlive objekty tridy UserFromAPI
            // a ulozeni techto objektu do seznamu
            List<UserFromAPI> userList = JsonConvert.DeserializeObject<List<UserFromAPI>>(editedJsonString);

            //kontrola validity jednotlivych hodnot objektu tridy UserFromAPI
            foreach (var user in userList)
            {
                Console.WriteLine();
                // prochazeni vsech polozek v seznamu + jejich vypis do console pro kontrolu
                Console.WriteLine(user.id + ") " + user.nameFirst + " " + user.nameLast + ", " + user.email + ", " + user.phone);

                // kontrola udaju
                Check.Id(user.id);
                Check.FirstName(user.nameFirst);
                Check.LastName(user.nameLast);
                Check.Email(user.email);
                Check.Phone(user.phone);

                // vypis statusu pro uzivatele
                Check.State(user.id);
            }
        }
    }
}