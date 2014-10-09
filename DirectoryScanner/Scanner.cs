using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner
{
    public class Scanner
    {

        private class NugetPackage
        {
            public string FullFileName { get; set; }
            public string FileName { get; set; }
            public Version Version { get; set; }
            public string PackageName { get; set; }
        }

        private List<NugetPackage> _packages;
        private readonly FileManager _fileManager;
        private readonly NugetManager _nugetManager;

        public string Destination { get; set; }
        public string Source { get; set; }

        public Scanner(string source, string destination)
        {
            Source = source;
            Destination = destination;
            _fileManager = new FileManager(source, destination);
            _nugetManager = new NugetManager();
        }

        private List<NugetPackage> GetPackages()
        {
            var retVal = _fileManager.FilesInSource.Select(sourceFile => new NugetPackage
            {
                FileName = Path.GetFileName(sourceFile),
                FullFileName = sourceFile,
                PackageName = _nugetManager.GetPackageName(sourceFile),
                Version = _nugetManager.GetVersion(sourceFile)
            }).ToList();

            return retVal;
        }

        public void Process()
        {
            _packages = GetPackages();

            //Get a list with the latest version of each package
            var latestVersionOfEachPackage = GetLatestVersionOfEachPackage();

            //For each unique package - move the latest version to the destination and delete any older packages
            foreach (var package in latestVersionOfEachPackage)
            {
                _fileManager.CopyFromSourceToDest(package.FileName, package.PackageName);
            }
        }


        /// <summary>
        /// Gets unique list of package names (not filename)
        /// </summary>
        /// <returns></returns>
        private IEnumerable<NugetPackage> GetLatestVersionOfEachPackage()
        {
            var retVal = new List<NugetPackage>();

            foreach (var package in _packages)
            {
                var existingPackage = retVal.FirstOrDefault(x => x.PackageName == package.PackageName);
                if (existingPackage == null)
                {
                    retVal.Add(package);
                }
                else
                {
                    if (existingPackage.Version < package.Version)
                    {
                        retVal.Remove(existingPackage);
                        retVal.Add(package);
                    }
                }
            }
            return retVal;
        }
    }
}
