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
            foreach (var item in ClientCityEvents(customer, events))
            {
                AddToEmail(customer, item);
            }
            /*
            * We want you to send an email to this customer with all events in their city
            * Just call AddToEmail(customer, event) for each event you think they should get
            */

            SendEmailForClosestEvents(events, customer, 5);
        }

        private static IEnumerable<Event> ClientCityEvents(Customer customer, IEnumerable<Event> events)
        {
            return events.Where(x => x.City == customer.City);
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

        private static void SendEmailForClosestEvents(IReadOnlyCollection<Event> events, Customer customer, int limit)  
        {   
            var closestForEmail = (from @event in events
                    let distance = GetDistance(customer.City,
                        @event.City)
                    select new ClosestEvent
                    {
                        Customer = customer,
                        Distance = distance,
                        Event = @event
                    }).Where(x => x.Event.City != customer.City)
                      .ToList()
                      .OrderBy(x => x.Distance)
                      .Take(limit);

            foreach (var customerEvent in closestForEmail)
            {
                AddToEmail(customer, customerEvent.Event);
            }
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
