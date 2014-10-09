using System.IO;
using System.Linq;
using DirectoryScanner;
using NUnit.Framework;

namespace Tests.Acceptance
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

            _sut = new Scanner(_sourceDir, _destDir);
            _fileManager = new FileManager(_sourceDir, _destDir);
            _nugetManger = new NugetManager();
        }

    }
}
