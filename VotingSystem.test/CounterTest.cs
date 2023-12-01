using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Sdk;
using static Xunit.Assert;

namespace VotingSystem.test
{
    public class CounterTest
    {
        public const string CounterName = "Counter Name";
        public const int CounterCount = 5;
        public Counter _counter = new Counter {  Name = CounterName, Count = CounterCount };

        [Fact]
        public void HasName()
        {
            Equal(CounterName, _counter.Name);
        }

        [Fact]
        public void GetStatistics_IncludesCounterName()
        {
            var statistics = _counter.GetStatistic(5);

            Equal(CounterName, statistics.Name);
        }

        [Fact]
        public void GetStatistics_IncludesCounterCount()
        {
            var statistics = _counter.GetStatistic(5);

            Equal(5, statistics.Count);
        }

        [Theory]
        [InlineData(5, 10, 50)]
        [InlineData(1, 3, 33.33)]
        [InlineData(2, 8, 25)]
        public void GetStatistics_ShowPercentageBasedOnTotalCount(int count, int total, double percent)
        {
            _counter.Count = count;

            var statistics = _counter.GetStatistic(total);

            Equal(percent, statistics.Percent);
        }

        [Fact]
        public void ResolveExcess_DoesntAddExcessWhenAllCountersAreEqual()
        {
            var counter1 = new Counter { Count = 1, Percent = 33.33 };
            var counter2 = new Counter { Count = 1, Percent = 33.33 };
            var counter3 = new Counter { Count = 1, Percent = 33.33 };
            var counters = new List<Counter> { counter1, counter2, counter3};

            new CounterManager().ResolveExcess(counters);

            Equal(33.33, counter1.Percent);
            Equal(33.33, counter2.Percent);
            Equal(33.33, counter3.Percent);

        }

        [Theory]
        [InlineData(66.66, 66.67, 33.33)]
        [InlineData(66.65, 66.67, 33.33)]
        public void ResolveExcess_AddsExcessToHighestCounter(double initial, double expected, double lowest)
        {
            var counter1 = new Counter { Count = 2, Percent = initial };
            var counter2 = new Counter { Count = 1, Percent = lowest };
            var counters = new List<Counter> { counter1, counter2 };

            new CounterManager().ResolveExcess(counters);

            Equal(expected, counter1.Percent);
            Equal(lowest, counter2.Percent);
        }

        [Theory]
        [InlineData(11.11, 11.12, 44.44)]
        [InlineData(11.10, 11.12, 44.44)]
        public void ResolveExcess_AddsExcessToLowestCountWhenMoreThanOneHighestCount(double initialLowest, double expected, double highest)
        {
            var counter1 = new Counter { Count = 2, Percent = highest };
            var counter2 = new Counter { Count = 2, Percent = highest };
            var counter3 = new Counter { Count = 1, Percent = initialLowest };
            var counters = new List<Counter> { counter1, counter2, counter3 };

            new CounterManager().ResolveExcess(counters);

            Equal(highest, counter1.Percent);
            Equal(highest, counter2.Percent);
            Equal(expected, counter3.Percent);
        }

        [Fact]
        public void ResolveExcess_DoesntAddExcessIfTotalPercentIs100()
        {
            var counter1 = new Counter { Count = 4, Percent = 80 };
            var counter2 = new Counter { Count = 1, Percent = 20 };
            var counters = new List<Counter> { counter1, counter2 };

            new CounterManager().ResolveExcess(counters);

            Equal(80, counter1.Percent);
            Equal(20, counter2.Percent);
        }
    }

    public class Counter
    {
        public Counter()
        {
        }

        public string Name { get; set; }
        public int Count { get; set; }
        public double Percent { get; set; }


        public Counter GetStatistic(int total)
        {
            Percent = Math.Round( Count * 100.0 / total, 2);
            return this;
        }


        
    }
        public class CounterManager
        {
            public void ResolveExcess(List<Counter> counters)
            {
                var totalPercent = counters.Sum(x => x.Percent);
                var excess = Math.Round(100 - totalPercent, 2);
                var winnerCount = counters.Max(counter => counter.Count);
                var winners = counters.Where(c => c.Count == winnerCount).ToList();

            if (winners.Count == 1)
                winners.First().Percent += excess;

            else if (winners.Count != counters.Count)
            {
                var lowestPercent = counters.Min(c => c.Percent);
                var loser = counters.First(c => c.Percent == lowestPercent);
                loser.Percent += excess;
            }

            //Func<IEnumerable<Counter>, double, double> addExcessToWInner =
            //        (IEnumerable<Counter> winners, double excess) =>
            //            winners.Max(p => p.Percent) + excess;

            //    addExcessToWInner(counters, excess);

            }
        }
}
