using System;
using System.Collections.Generic;
using System.Linq;

namespace SandBox
{
    public class Counter
    {
        private double? _percentage;

        public Counter(string name, int count)
        {
            Name = name;
            Count = count;
        }

        public string Name { get; }
        public int Count { get; }

        public double GetPercent(int total) =>
            _percentage ?? (_percentage = Math.Round(Count * 100.0 / total, 2)).Value;

        public void AddExcess(double excess) => _percentage += excess;
    }

    public class CounterManager
    {
        public CounterManager(params Counter[] counters)
        {
            Counters = new List<Counter>(counters);
        }

        public List<Counter> Counters { get; set; }

        public int Total() => Counters.Sum(x => x.Count);
        public double TotalPercentage() => Counters.Sum(x => x.GetPercent(Total()));

        public void AnounceWinner()
        {
            var excess = Math.Round(100 - TotalPercentage(), 2);

            Console.WriteLine("Excess : " + excess);

            var biggestAmountOfVotes = Counters.Max(x => x.Count);
            var winners = Counters.Where(x => x.Count == biggestAmountOfVotes).ToList();

            if (winners.Count == 1)
            {
                var winner = winners.First();
                winner.AddExcess(excess);
                Console.WriteLine($"{winner.Name} Won!");
            }
            else
            {
                if (winners.Count != Counters.Count)
                {
                    var lowestAmountOfVotes = Counters.Min(x => x.Count);
                    var looser = Counters.First(x => x.Count == lowestAmountOfVotes);
                    looser.AddExcess(excess);
                }

                Console.WriteLine(string.Join(" -DRAW- ", winners.Select(x => x.Name)));
                
            }

            foreach (var c in Counters)
            {
                Console.WriteLine($"{c.Name} count : {c.Count}, Percentage : {c.GetPercent(Total())}%");
            }

            Console.WriteLine();
            Console.WriteLine($"Total percentage : {Math.Round(TotalPercentage(), 2)}%");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var yes = new Counter("Yes", 4);
            var no = new Counter("No", 4);
            var maybe = new Counter("Maybe", 4);
            var hopefully = new Counter("Hopefully", 4);

            var manager = new CounterManager(yes, no, maybe, hopefully);

            manager.AnounceWinner();
        }
    }
}
/*
var listDecimal = new List<decimal>() { 1.23m, 12.3m, 34.03432m, 2.324m, 10.32m };
var sumOfListDecimal = listDecimal.Sum();
var containSome = listDecimal.Contains(12.3m);
var containSomeInIndex = listDecimal.FindIndex(a => a == 12.3m);
Console.WriteLine(sumOfListDecimal);

Console.WriteLine(containSome
    ? $"yes that right in index {containSomeInIndex}"
    : "Doesn't contain some decimal");
foreach (var c in listDecimal)
{
    Console.WriteLine(c);
}


var listInt = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8 };
var sumOfListInt = listInt.Sum();
Console.WriteLine(sumOfListInt);
*/