using System.IO;
using DirectoryScanner;
using NUnit.Framework;

namespace Tests.Integration
{
    [TestFixture]
    public class ScannerTest
    {
        private string _sourceDir;
        private string _destDir;
        private Scanner _sut;
        private FileManager _fileManager;

        [SetUp]
        public void Setup()
        {
            _sourceDir = Path.Combine(Directory.GetCurrentDirectory(), "ScannerSource");
            _destDir = Path.Combine(Directory.GetCurrentDirectory(), "ScannerDestination");
            SetupFolders();

            _sut = new Scanner(_sourceDir, _destDir);
            _fileManager = new FileManager(_sourceDir, _destDir);

            CreateSourceFile("One.File.1.0.nuget");
        }


        [Test]
        public void CanCopyLatestVersionToDestination()
        {
            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(1, destFiles.Count);
        }

        private void SetupFolders()
        {
            if (Directory.Exists(_sourceDir))
                Directory.Delete(_sourceDir, true);
            Directory.CreateDirectory(_sourceDir);

            if (Directory.Exists(_destDir))
                Directory.Delete(_destDir, true);
            Directory.CreateDirectory(_destDir);
        }

        private void CreateSourceFile(string fileName)
        {
            var filePath = Path.Combine(_sourceDir, fileName);
            var fs = File.Create(filePath);
            fs.Close();
        }

        private void CreateDestFile(string fileName)
        {
            var filePath = Path.Combine(_destDir, fileName);

            var fs = File.Create(filePath);
            fs.Close();
        }
    }
}