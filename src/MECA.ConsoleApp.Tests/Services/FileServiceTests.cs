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
    }
}
