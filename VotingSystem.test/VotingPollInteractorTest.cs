using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace VotingSystem.test
{
    public class VotingPollInteractorTest
    {
        [Fact]
        public void CreateVotingPoll_UsesVotingPollFactoryToCreateVotingPoll()
        {
            var request = new VotingPollFactory.Request();
            var mockLibrary = new Mock<IVotingPollFactory>();
            var interactor = new VotingPollInteractor(mockLibrary.Object);

            interactor.CreateVotingPoll(request);

            mockLibrary.Verify(x => x.Create(request));
        }
    }

    public class VotingPollInteractor
    {
        private readonly IVotingPollFactory _factory;

        public VotingPollInteractor(IVotingPollFactory factory)
        {
            _factory = factory;
        }

        public void CreateVotingPoll(VotingPollFactory.Request request)
        {
            _factory.Create(request);

        }
    }
}
