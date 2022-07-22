using System;
using System.Collections.Generic;
using ViagogoAssessment;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ViagogoAssessment
{
    public class Event
    {
        public string Name { get; set; }
        public string City { get; set; }
    }
    public class Customer
    {
        public string Name { get; set; }
        public string City { get; set; }
    }

    public class ClosestEvent
    {
        public Customer Customer { get; set; }
        public Event Event { get; set; }
        public int Distance { get; set; }
        public int Price { get; set; }
    }

    public class Solution
    {
        static void Main(string[] args)
        {
            var events = new List<Event>{
            new Event{ Name = "Phantom of the Opera", City = "New York"},
            new Event{ Name = "Metallica", City = "Los Angeles"},
            new Event{ Name = "Metallica", City = "New York"},
            new Event{ Name = "Metallica", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "New York"},
            new Event{ Name = "LadyGaGa", City = "Boston"},
            new Event{ Name = "LadyGaGa", City = "Chicago"},
            new Event{ Name = "LadyGaGa", City = "San Francisco"},
            new Event{ Name = "LadyGaGa", City = "Washington"}
};
            //1. find out all events that are in cities of customer
            // then add to email.
            var customer = new Customer { Name = "Mr. Fake", City = "Boston" };

            // 1. TASK
            // Option 1
           // ClientCityEvents(customer, events).ForEach(x => AddToEmail(customer, x));

            // Option 2
            (events.Where(x => x.City.Equals(customer.City, StringComparison.OrdinalIgnoreCase)).ToList()).ForEach(c => AddToEmail(customer, c));


            /*
            * We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */

            SendEmailForClosestEvents(events, customer, 5);
        }

        private static List<Event> ClientCityEvents(Customer customer, List<Event> events)
        {
            return events.Where(x => x.City.Equals(customer.City, StringComparison.OrdinalIgnoreCase)).ToList();
        }


        // You do not need to know how these methods work
        static void AddToEmail(Customer c, Event e, int? price = null)
        {
            var distance = GetDistance(c.City, e.City);
            Console.Out.WriteLine($"{c.Name}: {e.Name} in {e.City}"
            + (distance > 0 ? $" ({distance} miles away)" : "")
            + (price.HasValue ? $" for ${price}" : ""));
        }
        static int GetPrice(Event e)
        {
            return (AlphabeticalDistance(e.City, "") + AlphabeticalDistance(e.Name, "")) / 10;
        }

        // Dictionary caching is recommended for this exercise,
        // because indexing is fast due to it's unique keys
        private static void SendEmailForClosestEvents(List<Event> events, Customer customer, int limit)
        {
            var store = new Dictionary<string, int>();
            var closestEvents = new List<ClosestEvent>();

            events.ForEach(x =>
            {
                var closestEvent = new ClosestEvent();  
                var cacheKey = $"{customer.City}:{x.City}";
                if (store.ContainsKey(cacheKey))
                {
                    closestEvent.Customer = customer;
                    closestEvent.Event = x;
                    closestEvent.Distance = store[cacheKey];
                    closestEvent.Price = GetPrice(x);
                    closestEvents.Add(closestEvent);
                }
                else
                {
                    //Adding a try/catch, to make sure we don't crash the program, if GetDistance fails
                    try
                    {
                        var distance = GetDistance(customer.City, x.City);
                        closestEvent.Customer = customer;
                        closestEvent.Event = x;
                        closestEvent.Distance = distance;
                        closestEvent.Price = GetPrice(x);
                        store.Add(cacheKey, distance);
                        closestEvents.Add(closestEvent);
                    }
                    catch (Exception e)
                    {
                        // Interact with interviewer on what to return here if program crashes.
                        Console.WriteLine(e);
                    }
                }
                
            });

            // We can now sort by either distance or price
            (closestEvents.OrderBy(x => x.Distance).Take(limit).ToList()).ForEach(x =>
            {
                AddToEmail(x.Customer, x.Event);
            });
        }
        
      
        private static int GetDistance(string fromCity, string toCity)
        {
            return AlphabeticalDistance(fromCity, toCity);
        }

        private static int AlphabeticalDistance(string s, string t) 
        {
            var result = 0;
            var i = 0;
            for (i = 0; i < Math.Min(s.Length, t.Length); i++)
            {
                result += Math.Abs(s[i] - t[i]);
            }
            for (; i < Math.Max(s.Length, t.Length); i++)
            {
                // Console.Out.WriteLine($"loop 2 i={i} {s.Length} {t.Length}");
                result += s.Length > t.Length ? s[i] : t[i];
            }
            return result;
        }
    }
}
