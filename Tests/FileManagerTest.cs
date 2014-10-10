using System;
using System.IO;
using System.Linq;
using DirectoryScanner;
using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class FileManagerTest
    {
        private FileManager _sut;
        private string _sourceDir;
        private string _destDir;

        [SetUp]
        public void Setup()
        {
            _sourceDir = Path.Combine(Directory.GetCurrentDirectory(), "FMSource");
            _destDir = Path.Combine(Directory.GetCurrentDirectory(), "FMDestination");

            _sut = new FileManager(_sourceDir, _destDir);
            
            if(Directory.Exists(_sourceDir))
                Directory.Delete(_sourceDir, true);
            Directory.CreateDirectory(_sourceDir);

            if (Directory.Exists(_destDir))
                Directory.Delete(_destDir, true);
            Directory.CreateDirectory(_destDir);
        }

        [Test]
        public void CanCreateFileManager()
        {
            Assert.IsNotNull(_sut);
        }

        [Test]
        public void CanGetListOfFilesFromSourceDirectory()
        {
            const string fileNameToTestFor = "some.nuget.package.1.2.3.nuget";
            CreateSourceFile(fileNameToTestFor);
            CreateSourceFile("file2.txt");
            CreateDestFile("file1.txt");
            CreateDestFile("file2.txt");
            CreateDestFile("file3.txt");

            var filesInSource = _sut.FilesInSource;
            var filesInDestnation = _sut.FilesInDestination;

            Assert.AreEqual(2, filesInSource.Count);
            Assert.AreEqual(3, filesInDestnation.Count);

            Assert.IsTrue(filesInSource.FirstOrDefault(x => x.EndsWith(fileNameToTestFor)) != null);
        }


        private void CreateSourceFile(string fileName)
        {
            var filePath = Path.Combine(_sourceDir, fileName);

            File.Create(filePath).Close();
        }

        private void CreateDestFile(string fileName)
        {
            var filePath = Path.Combine(_destDir, fileName);

            File.Create(filePath).Close();
        }
    }
}