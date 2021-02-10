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

        public static TheoryData<Dictionary<string, int>> WriteToFileAggregatedTheoryData =>
            new TheoryData<Dictionary<string, int>>
            {
                { null },
                { new Dictionary<string, int>() }
            };

        [Theory]
        [MemberData(nameof(WriteToFileAggregatedTheoryData))]
        public async Task WriteToFile_GivenEmptyDataThenThrowException(Dictionary<string, int> data)
        {
            var service = new FileService();

            Func<Task> act = async () => await service.WriteToFile(data, null as string);

            await act.Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage($"*{Constants.AggregatedDataIsRequired}*");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task WriteToFile_GivenAnInvalidFolderNameThenThrowArgumentNullException(string folderName)
        {
            var service = new FileService();
            var data = new Dictionary<string, int> { { "2015 Jan", 27 } };

            Func<Task> act = async () => await service.WriteToFile(data, folderName);

            await act.Should()
                .ThrowAsync<ArgumentNullException>()
                .WithMessage("*folderName*");
        }


        public static TheoryData<string> WriteToFileFolderNameTheoryData =>
            new TheoryData<string>
            {
                { "UnexpectedFolderName" },
                { @$"TestData\{Constants.AggregatesFolder}\unexpected" }
            };

        [Theory]
        [MemberData(nameof(WriteToFileFolderNameTheoryData))]
        public async Task WriteToFile_GivenAnNonExistentFolderNameThenThrowArgumentNullException(string folderName)
        {
            var service = new FileService();
            var data = new Dictionary<string, int> { { "2015 Jan", 27 } };

            Func<Task> act = async () => await service.WriteToFile(data, folderName);

            await act.Should()
                .ThrowAsync<InvalidOperationException>()
                .WithMessage(Constants.OutputFolderDoesNotExist);
        }

        [Fact]
        public async Task WriteToFile_GivenAggregatedDataThenWriteToFile()
        {
            var service = new FileService();

            var data = new Dictionary<string, int> {{"2015 Jan", 27}};

            var aggregatesPath = @$"TestData\{Constants.AggregatesFolder}";

            await service.WriteToFile(data, aggregatesPath);

            var fileExists = File.Exists(@$"{aggregatesPath}\consumption-output.csv");

            fileExists.Should().BeTrue();
        }
    }
}
