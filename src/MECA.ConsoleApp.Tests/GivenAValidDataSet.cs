using System;
using Xunit;
using MECA.ConsoleApp;

namespace MECA.ConsoleApp.Tests
{
    public class GivenAValidDataSet
    {
        public class WhenWeLoadTheFile
        {
            [Fact]
            public void ThenNoErrorIsThrown()
            {
                Program.Main(new [] {"arg1"});
            }
        }
    }
}
