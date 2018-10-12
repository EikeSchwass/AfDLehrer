using System;
using System.Collections.Generic;
using System.Threading;
using Flurl.Http;

namespace Launcher
{
    public static class Program
    {
        public const string Url = "http://www.stefan-raepple.de/mein-lehrer-hetzt/";

        public static void Main(string[] args)
        {
            Console.WriteLine("Dieses Program ist rein für akademische Zwecke gedacht. Tatsächliches Ausführen ist nicht vorgesehen! Drücke eine beliebige Taste wenn du dies zur Kenntnis genommen hast und trotzdem fortfahren möchtest");
            Console.ReadKey();

            Console.WriteLine("Das Program wird jetzt in zufälligen Zeitabständen semi-sinnvolle Inhalte posten. Kannst das Programm einfach im Hintergrund laufen lassen.");

            while (true)
            {
                GenerateRequest();
                int timeout = Extensions.Random.Next(15, 300);
                Console.WriteLine($"Wartet {timeout} sekunden");
                Thread.Sleep(timeout * 1000);
            }
        }

        private static void GenerateRequest()
        {
            var header = new Dictionary<string, string>
            {
                {"Accept", "text/html,application/xhtml+xm…plication/xml;q=0.9,*/*;q=0.8"},
                {"Accept-Encoding", "gzip, deflate"},
                {"Accept-Language", "de,en-US;q=0.7,en;q=0.3"},
                {"Connection", "keep-alive"},
                {"Content-Type", "multipart/form-data; boundary=…-----------------727231584857"},
                {"DNT", "1"},
                {"Host", "www.stefan-raepple.de"},
                {"Referer", @"http://www.stefan-raepple.de/mein-lehrer-hetzt/"},
                {"Upgrade-Insecure-Requests", "1"},
                {
                    "User-Agent",
                    $"Mozilla/{Extensions.Random.Next(1, 6)}.0 (Windows NT {Extensions.Random.Next(4, 11)}.0; Win{(Extensions.Random.NextDouble() > 0.35 ? "64" : "32")}; x{(Extensions.Random.NextDouble() > 0.35 ? "64" : "86")}; rv: {Extensions.Random.Next(12, 70)}.0) Gecko/20100101 Firefox/{Extensions.Random.Next(1, 62)}.0"
                }
            };

            string schule = Schule.Schulen.SelectRandomly();
            string lehrername = GetRandomName(true);
            string username = GetRandomName(false);
            string email = (username.Replace(" ", ".") + "@" + Emails.Domains.SelectRandomly()).ToLower();
            string message = GetRandomMessage(username);
            string phone = GetRandomPhone();

            Url.WithHeaders(header)
               .PostMultipartAsync(b =>
                                   {
                                       b.AddString("_wpcf7", "384");
                                       b.AddString("_wpcf7_version", "5.0.4");
                                       b.AddString("_wpcf7_locale", "de_DE");
                                       b.AddString("_wpcf7_unit_tag", "wpcf7-f384-p385-o3");
                                       b.AddString("_wpcf7_container_post", "385");
                                       b.AddString("Schule", schule);
                                       b.AddString("Lehrername", lehrername);
                                       b.AddString("Lehrername", lehrername);
                                       b.AddString("your-email", email);
                                       b.AddString("Telefonnummer", phone);
                                       b.AddString("your-message", message);
                                       b.AddString("your-consent", "1");
                                   })
               .ReceiveString()
               .Wait();
            Console.WriteLine("---");
            Console.WriteLine("Schule: " + schule);
            Console.WriteLine("Lehrername: " + lehrername);
            Console.WriteLine("Du: " + username);
            Console.WriteLine("Email: " + email);
            Console.WriteLine("Telefonnummer: " + phone);
            Console.WriteLine("Nachricht:");
            Console.WriteLine(message);
            Console.WriteLine("---");
        }

        private static string GetRandomPhone()
        {
            string vorwahl = Telefone.Numbers.SelectRandomly();
            string seperator = Telefone.Seperators.SelectRandomly();
            var number = "";
            int length = Extensions.Random.Next(6, 9);
            for (var i = 0; i < length; i++) number += Extensions.Random.Next(1, 10);

            return $"{vorwahl}{seperator}{number}";
        }

        private static string GetRandomMessage(string name)
        {
            string greeting = Phrases.Greetings.SelectRandomly();
            if (!string.IsNullOrWhiteSpace(greeting))
                greeting += Environment.NewLine;
            string phrase1 = Phrases.Phrases1.SelectRandomly();
            string accusation = Extensions.Random.NextDouble() < 0.5 ? Phrases.PhrasesMale.SelectRandomly() : Phrases.PhrasesFemale.SelectRandomly();
            accusation += " ";
            accusation += Phrases.Phrases2.SelectRandomly();
            string phrase3 = Phrases.Phrases3.SelectRandomly();
            string phrase4 = Phrases.Phrases4.SelectRandomly();
            string phrase5 = Phrases.Phrases5.SelectRandomly();

            string message = $"{greeting}{phrase1} {accusation}";
            if (Extensions.Random.NextDouble() < 0.25)
                message += Environment.NewLine;
            else
                message += " ";
            message += phrase3 + " ";
            message += phrase4;
            if (!string.IsNullOrWhiteSpace(phrase5))
            {
                message += Environment.NewLine;
                message += phrase5;
                if (Extensions.Random.NextDouble() < 0.4)
                {
                    message += Environment.NewLine;
                    message += name;
                }
            }

            int index = Extensions.Random.Next(0, message.Length);
            message = message.Remove(index, 1);

            return message;
        }

        private static string GetRandomName(bool doubleNames)
        {
            string name = Extensions.Random.NextDouble() < 0.5 ? WomenNames.Names.SelectRandomly() : MenNames.Names.SelectRandomly();
            name += " " + FamilyNames.Names.SelectRandomly();
            if (Extensions.Random.NextDouble() < 0.1 && doubleNames)
                name += "-" + FamilyNames.Names.SelectRandomly();
            return name;
        }
    }

    public static class Extensions
    {
        public static Random Random { get; } = new Random();

        public static string SelectRandomly(this string[] list)
        {
            int next = Random.Next(0, list.Length);
            return list[next];
        }
    }
}