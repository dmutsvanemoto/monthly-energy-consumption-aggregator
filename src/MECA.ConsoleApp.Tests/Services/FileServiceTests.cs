using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentAssertions;
using MECA.ConsoleApp.Models;
using MECA.ConsoleApp.Services;
using Xunit;

namespace MECA.ConsoleApp.Tests.Services
{
    public class FileServiceTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GivenAnInvalidFolderNameThenThrowArgumentNullException(string folderName)
        {
            var service = new FileService();

            Func<Task> act = async () => await service.LocateFile(folderName);

            await act.Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("*folderName*");
        }


        public static TheoryData<string> FolderNameTheoryData =>
            new TheoryData<string>
            {
                { "UnexpectedFolderName" },
                { @$"TestData\{Constants.IncomingFolder}\unexpected" }
            };

        [Theory]
        [MemberData(nameof(FolderNameTheoryData))]
        public async Task GivenAnNonExistentFolderNameThenThrowArgumentNullException(string folderName)
        {
            var service = new FileService();

            Func<Task> act = async () => await service.LocateFile(folderName);

            await act.Should()
                .ThrowAsync<DirectoryNotFoundException>()
                .WithMessage($"*{folderName}*");
        }

        [Fact]
        public async Task GivenAFolderNameThenReturnFilePath()
        {
            var service = new FileService();

            var expectedFilePath = @"TestData\incoming\consumption-data.csv";
            var actualFilePath = await service.LocateFile(@$"TestData\{Constants.IncomingFolder}");

            actualFilePath.Should().Be(expectedFilePath);
        }

        [Fact]
        public async Task GivenAFilePathThenReturnStructuredData()
        {
            var service = new FileService();
            var filePath = await service.LocateFile(@$"TestData\{Constants.IncomingFolder}");

            var expected = new List<ConsumptionData>
            {
                new ConsumptionData { Date = DateTime.Parse("2015-01-19"), Consumption = 0 },
                new ConsumptionData { Date = DateTime.Parse("2015-01-20"), Consumption = 27 },
            };

            var data = await service.ReadFile(filePath);

            data.Should().HaveCount(2);

            data.Should().BeEquivalentTo(expected);
        }

        // TODO: When FilePath is invalid then ReadFile should explode
        // TODO: When File data is invalid(unexpected format) then ReadFile should explode for date and or consumption
        // TODO: When File columns don't start with date then ReadFile should explode for date and or consumption
    }
}
