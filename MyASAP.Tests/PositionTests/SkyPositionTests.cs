using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;
using Xunit;
using MyASAP.Position;


namespace MyASAP.Tests.PositionTests
{
    public class SkyPositionTests
    {
        [Fact]
        public void SkyPosition_ParameterlessConstructor_InitializeCoordinatesToZero()
        {
            var skyPosition = new SkyPosition();

            Assert.Equal(0,skyPosition.Dec);
            Assert.Equal(0, skyPosition.Ra);
        }

        [Fact]
        public void SkyPosition_ConstructorTakingSkyPositionParameter_InitializeCoordinatesAccordingly()
        {
            var otherSkyPosition = new SkyPosition();
            otherSkyPosition.Ra = 37;
            otherSkyPosition.Dec = 55;

            var skyPosition = new SkyPosition(otherSkyPosition);

            Assert.Equal(55, skyPosition.Dec);
            Assert.Equal(37, skyPosition.Ra);
        }

        [Fact]
        public void DistanceInDegrees_InsertSkyPosition_ReturnsDistanceInDegrees()
        {
            var skyPosition1 = new SkyPosition();
            var skyPosition2 = new SkyPosition();
            skyPosition2.Ra = 12;
            skyPosition2.Dec = 90;

            var result = skyPosition1.DistanceInDegrees(skyPosition2);

            Assert.Equal(112,result,1);
        }
    }
}
