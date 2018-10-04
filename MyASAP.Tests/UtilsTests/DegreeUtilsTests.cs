using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using MyASAP.Utils;
using MyASAP.Position;

namespace MyASAP.Tests.UtilsTests
{
    public class DegreeUtilsTests
    {
 
        [Fact]
        public void RaToHms_InsertRadians_ReturnsHoursMinutesSeconds()
        {
            double ra = 3.14;
            int hours;
            int minutes;
            double seconds;

            DegreeUtils.RaToHms(ra, out hours, out minutes, out seconds);
            
            Assert.Equal(12,hours);
        }
    }
}
