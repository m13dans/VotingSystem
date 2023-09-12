using System;
using System.Collections.Generic;
using System.Linq;

namespace SandBox
{
    public class Counter
    {
        public Counter(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; }
        public int Count { get; }

        public double GetPercent(int total)
        {
            return Math.Round(Count * 100.0 / total, 2);
        }
    }

    public class CounterManager
    {
        public CounterManager(params Counter[] counters)
        {
            Counters = new List<Counter>(counters);
        }

        public List<Counter> Counters { get; set; }

        public int Total() => Counters.Sum(x => x.Count);
        
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var yes = new Counter("Yes", 4);
            var no = new Counter("No", 4);
            var maybe = new Counter("Maybe", 4);

            var manager = new CounterManager(yes, no, maybe);

            var yesPercent = yes.GetPercent(manager.Total());
            var noPercent = no.GetPercent(manager.Total());
            var maybePercent = maybe.GetPercent(manager.Total());

            var excess = Math.Round(100 - yesPercent - noPercent - maybePercent, 2);

            Console.WriteLine(Math.Round(yesPercent + noPercent + maybePercent, 2));
            Console.WriteLine("Excess : " + excess);

            if (yes.Count > no.Count && yes.Count > maybe.Count)
            {
                Console.WriteLine($"Yes Won by {yesPercent}%");
                yesPercent += excess;
            }
            else if (no.Count > yes.Count && no.Count > maybe.Count)
            {
                Console.WriteLine($"No Won by {noPercent}%");
                noPercent += excess;
            }
            else if (maybe.Count > yes.Count && maybe.Count > no.Count)
            {
                Console.WriteLine($"Maybe won by {maybePercent}%");
                maybePercent += excess;
            }

            else
            {
                Console.WriteLine($"Draw");
            }

            Console.WriteLine($"Yes count : {yes.Count}, Percentage : {Math.Round(yesPercent, 2)}%");
            Console.WriteLine($"No count : {no.Count}, Percentage : {Math.Round(noPercent, 2)}%");
            Console.WriteLine($"Maybe count : {maybe.Count}, Percentage : {Math.Round(maybePercent, 2)}%");

            Console.WriteLine();
            Console.WriteLine($"Total percentage : {Math.Round(yesPercent, 2) + Math.Round(noPercent, 2) + Math.Round(maybePercent, 2)}%");

        }
    }
}
