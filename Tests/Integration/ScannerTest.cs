using System.IO;
using System.Linq;
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
        private NugetManager _nugetManger;

        [SetUp]
        public void Setup()
        {
            _sourceDir = Path.Combine(Directory.GetCurrentDirectory(), "ScannerSource");
            _destDir = Path.Combine(Directory.GetCurrentDirectory(), "ScannerDestination");
            SetupFolders();

            _sut = new Scanner(_sourceDir, _destDir);
            _fileManager = new FileManager(_sourceDir, _destDir);
            _nugetManger = new NugetManager();
        }


        [Test]
        public void CanCopyLatestVersionToDestination()
        {
            CreateSourceFile("One.File.1.0.nuget");
            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(1, destFiles.Count);
        }

        [Test]
        public void CanCopyLatestVersionWhenMoreThanOneVersionExists()
        {
            CreateSourceFile("One.File.1.0.nuget");
            CreateSourceFile("One.File.1.1.nuget");
            CreateSourceFile("One.File.1.1.1.nuget");
            CreateSourceFile("One.File.1.1.1.1.nuget");
            CreateSourceFile("One.File.1.2.3.4.nuget");
            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(1, destFiles.Count);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Major, 1);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Minor, 2);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Build, 3);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Revision, 4);   
        }

        [Test]
        public void UsesLatestVersionRegardlessOrBuildAndRevision()
        {
            CreateSourceFile("One.File.1.0.nuget");
            CreateSourceFile("One.File.1.1.nuget");
            CreateSourceFile("One.File.1.1.1.nuget");
            CreateSourceFile("One.File.1.1.1.1.nuget");
            CreateSourceFile("One.File.2.nuget");
            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(1, destFiles.Count);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Major, 2);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Minor, 0);
        }

        [Test]
        public void CanCopyMoreThanOneFile()
        {
            CreateSourceFile("One.File.1.0.nuget");
            CreateSourceFile("One.File.1.1.nuget");
            CreateSourceFile("One.File.1.1.1.nuget");
            CreateSourceFile("One.File.1.1.1.1.nuget");
            CreateSourceFile("One.File.2.nuget");

            CreateSourceFile("Two.File.1.0.nuget");
            CreateSourceFile("Two.File.1.1.nuget");
            CreateSourceFile("Two.File.1.1.1.nuget");
            CreateSourceFile("Two.File.1.2.3.4.nuget");
            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(2, destFiles.Count);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Major, 2);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Minor, 0);

            Assert.AreEqual(_nugetManger.GetVersion(destFiles[1]).Major, 1);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[1]).Minor, 2);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[1]).Build, 3);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[1]).Revision, 4);   
        }


        [Test]
        public void DeletesAllFilesWithSamePackageThatAreOlderVersions()
        {
            CreateSourceFile("A.File.1.0.nuget");
            CreateSourceFile("A.File.1.1.nuget");
            CreateSourceFile("A.File.1.1.1.nuget");
            CreateSourceFile("A.File.1.1.1.1.nuget");
            CreateSourceFile("A.File.2.nuget");

            CreateDestFile("A.File.1.1.nuget");
            CreateDestFile("B.File.1.1.1.nuget");
            CreateDestFile("C.File.1.1.1.1.nuget");

            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(3, destFiles.Count);

            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Major, 2);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Minor, 0);
        }

        [Test]
        public void OnlyKeepsLatestVersionFromSourceEvenIfNewerExistsOnDestination()
        {
            CreateSourceFile("A.File.1.0.nuget");
            CreateSourceFile("A.File.1.1.nuget");
            CreateSourceFile("A.File.1.1.1.nuget");
            CreateSourceFile("A.File.1.1.1.1.nuget");
            CreateSourceFile("A.File.2.nuget");

            CreateDestFile("A.File.3.1.nuget");
            CreateDestFile("B.File.1.1.1.nuget");
            CreateDestFile("C.File.1.1.1.1.nuget");

            _sut.Process();
            var destFiles = _fileManager.FilesInDestination;
            Assert.AreEqual(3, destFiles.Count);

            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Major, 2);
            Assert.AreEqual(_nugetManger.GetVersion(destFiles[0]).Minor, 0);
        }


        /// <summary>
        /// Can move and leave alone several different files
        /// </summary>

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
            File.Create(filePath).Close();
        }

        private void CreateDestFile(string fileName)
        {
            var filePath = Path.Combine(_destDir, fileName);
            File.Create(filePath).Close();
        }
    }
}
