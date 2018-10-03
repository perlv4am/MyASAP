using System;
using Xunit;
using MyASAP.Position;

namespace MyASAP.Tests.PositionTests
{
    public class SimplePointTests
    {
        [Fact]
        public void SimplePoint_ParameterlessConstructor_InitializeZeroCoordinates()
        {
            var simplePoint = new SimplePoint();

            Assert.Equal(0, simplePoint.X);
            Assert.Equal(0, simplePoint.Y);
        }

        [Fact]
        public void SimplePoint_ConstructorTakingTwoDoubles_InitializeCoordinatesAccordingly()
        {
            var simplePoint = new SimplePoint(1.1, 2.2);
            Assert.Equal(1.1, simplePoint.X);
            Assert.Equal(2.2, simplePoint.Y);
        }

        [Fact]
        public void Distance_InsertSimplePoint_ReturnsDistanceToSimplePoint()
        {
            //Arrange
            SimplePoint simplePoint1 = new SimplePoint(0, 0);
            SimplePoint simplePoint2 = new SimplePoint(1, 1);
            var expectedResult = Math.Sqrt(2);

            //Act
            var result = simplePoint1.Distance(simplePoint2);

            //Assert
            Assert.True((bool)(Math.Abs(result-expectedResult) < Double.Epsilon));
        }

       [Fact]
        public void IsTheSame_ProvidingTheSamePoints_ReturnsTrue()
        {
            //Arrange
            SimplePoint simplePoint1 = new SimplePoint(0, 0);
            SimplePoint simplePoint2 = new SimplePoint(0, 0);

            //Act
            var result = simplePoint1.IsTheSame(simplePoint2, 3);

            //Assert
            Assert.True(result);
        }


        [Fact]
        public void IsTheSame_ProvidingDifferentPoints_ReturnsFalse()
        {
            //Arrange
            SimplePoint simplePoint1 = new SimplePoint(0, 0);
            SimplePoint simplePoint2 = new SimplePoint(10, 10);

            //Act
            var result = simplePoint1.IsTheSame(simplePoint2, 3);

            //Assert
            Assert.False(result);
        }
    }
}
