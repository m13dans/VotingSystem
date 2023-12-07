using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using static Xunit.Assert;

namespace VotingSystem.test
{
    public class VotingPollTest
    {
        [Fact]
        public void ZeroCounterWhenCreated()
        {
            // Given
            var poll = new VotingPoll();

            // When

            // Then
            Empty(poll.Counters);
        }
    }

    public class VotingPollFactoryTest
    {
        private VotingPollFactory factory = new VotingPollFactory();
        private VotingPollFactory.Request _request = new VotingPollFactory.Request
        {
            Names = new[] { "name1", "name2" },
            Title = "Title",
            Description = "Description"
        };

        [Fact]
        public void Create_ThrowsWhenLessThanTwoCounterNames()
        {
            _request.Names = new[] { "name" };
            Throws<ArgumentException>(() => factory.Create(_request));
            _request.Names = new string[] { };
            Throws<ArgumentException>(() => factory.Create(_request));
        }

        [Fact]
        public void Create_AddCounterToThePollForEachName()
        {
            // When
            var poll = factory.Create(_request);

            // Then
            foreach (var name in _request.Names)
            {
                Contains(name, poll.Counters.Select(x => x.Name));
            }

        }

        [Fact]
        public void Create_AddTitleToThePoll()
        {
            var poll = factory.Create(_request);

            Equal(_request.Title, poll.Tittle);
        }

        [Fact]
        public void Create_AddDescriptionToThePoll()
        {
            var poll = factory.Create(_request);

            Equal(_request.Description, poll.Description);
        }
    }
    public class VotingPollFactory
    {
        public class Request
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Names { get; set; }

        }

        public VotingPoll Create(Request request)
        {
            if (request.Names.Length < 2) throw new ArgumentException();

            return new VotingPoll
            {
                Tittle = request.Title,
                Description = request.Description,
                Counters = request.Names.Select(n =>
                    new Counter { Name = n })
            };
        }
    }

    public class VotingPoll
    {
        public VotingPoll()
        {
            Counters = Enumerable.Empty<Counter>();
        }

        public string Tittle { get; set; }
        public string Description { get; set; }
        public IEnumerable<Counter> Counters { get; set; }
    }
}