using System;
using System.Threading.Tasks;
using Xunit;

namespace MECA.ConsoleApp.Tests
{
    public class GivenAValidDataSet
    {
        public class WhenWeLoadTheFile
        {
            [Fact]
            public async Task ThenNoErrorIsThrown()
            {
                await Program.Main(new [] {"arg1"});
            }
        }
    }
}
