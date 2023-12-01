using Moq;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using Xunit;

namespace VotingSystem.test
{
    public interface ITestOne
    {
        public int Add(int a, int b);
        public void Out(string msg);
    }

    public class MathOne
    {
        private readonly ITestOne testOne;

        public MathOne(ITestOne testOne)
        {
            this.testOne = testOne;
        }

        public int Add(int a, int b) => testOne.Add(a, b);
        public void Out(string msg) => testOne.Out(msg);
    }

    public class MathOneTest
    {

        [Fact]
        public void MathOneAddTwoNumber()
        {
            
            var tesOneMock = new Mock<ITestOne>();
            tesOneMock.Setup(x => x.Add(1, 2)).Returns(3);
            var mathOne = new MathOne(tesOneMock.Object);
            Assert.Equal(3, mathOne.Add(1, 2));

        }

        [Fact]
        public void VerifyFunctionHasBeenCalled()
        {

            var tesOneMock = new Mock<ITestOne>();
            var mathOne = new MathOne(tesOneMock.Object);
            var text = "hello";

            mathOne.Out(text);
            tesOneMock.Verify(x => x.Out(text), Times.Once);

        }

    }

    public class TestOne : ITestOne
    {
        public int Add(int a, int b) => a + b;

        public void Out(string msg)
        {
            Console.WriteLine(msg);
        }
    }

    public class TestOneTest
    {
        [Fact]
        public void Add_AddsTwoNumberTogether()
        {
            var result = new TestOne().Add(1, 2);
            Assert.Equal(3, result);
        }

        [Theory]
        [InlineData(1, 1, 2)]
        [InlineData(1, 0, 1)]
        [InlineData(0, 2, 2)]
        public void Add_AddsTwoNumberTogether_Theory(int a, int b, int expected)
        {
            var result = new TestOne().Add(a, b);
            Assert.Equal(expected, result);
        }
    }
}
