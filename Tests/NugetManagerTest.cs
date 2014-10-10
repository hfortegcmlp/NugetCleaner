using System;
using System.Collections.Generic;
using System.IO;
using DirectoryScanner;
using NUnit.Framework;


namespace Tests
{
    [TestFixture]
    public class NugetManagerTest
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
        public void CanGetPackageNameFromFileNameIncludingPath()
        {
            const string fileNameIncludingPath = "c:\\SomePackage.1.2.3.nuget";
            var packageName = _sut.GetPackageName(fileNameIncludingPath);

            Assert.AreEqual("SomePackage", packageName);
        }

        [Test]
        public void CanGetFileName()
        {
            const string packageName = "Package.With-periods!and_strange.stuff";
            const string version = ".1.2.4.8";
            const string path = "c:\\some\\folder\\";

            const string fileName = packageName + version + ".nuget";
            const string fullFileName = path + fileName;

            var packages = _sut.GetPackages(new List<string> {fullFileName});
            Assert.AreEqual(1, packages.Count);
            var package = packages[0];
            Assert.AreEqual(packageName, package.PackageName);
            Assert.AreEqual(fullFileName, package.FullFileName);
            Assert.AreEqual(fileName, package.FileName);
            Assert.AreEqual(1, package.Version.Major);
            Assert.AreEqual(2, package.Version.Minor);
            Assert.AreEqual(4, package.Version.Build);
            Assert.AreEqual(8, package.Version.Revision);
        }

        [Test]
        public void CanSetNugetPackageCorrectlyBasedOnFileName()
        {

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