using System;
using System.IO;
using DirectoryScanner;
using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class NugetVersionManagerTest
    {
        private NugetManager _sut;
        private string _baseDir;

        [SetUp]
        public void Setup()
        {
            _sut = new NugetManager();
        }

        [Test]
        public void CanCreateScanner()
        {
            Assert.IsNotNull(_sut);
        }

        [Test]
        public void CanGetMajorVersion()
        {
            const string fileName = "Package.1.nuget";
            var version = _sut.GetVersion(fileName);

            Assert.AreEqual(1, version.Major);
        }

        [Test]
        public void CanGetFileName()
        {
            const string fileName = "Package.With-periods!and_strange.stuff.1.nuget";
            
            var packageName = _sut.GetPackageName(fileName);

            Assert.AreEqual("Package.With-periods!and_strange.stuff", packageName);
        }

        [Test]
        public void CanGetAllVersions()
        {
            const string fileName = "Package.1.2.3.4.nuget";
            var version = _sut.GetVersion(fileName);

            Assert.AreEqual(1, version.Major);
            Assert.AreEqual(2, version.Minor);
            Assert.AreEqual(3, version.Build);
            Assert.AreEqual(4, version.Revision);
        }
    }
}