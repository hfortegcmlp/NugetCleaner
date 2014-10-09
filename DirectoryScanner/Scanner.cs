using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryScanner
{
    public class Scanner
    {
        private List<NugetManager.NugetPackage> _packages;
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

        public void Process()
        {
            _packages = _nugetManager.GetPackages(_fileManager.FilesInSource);

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
        private IEnumerable<NugetManager.NugetPackage> GetLatestVersionOfEachPackage()
        {
            var retVal = new List<NugetManager.NugetPackage>();

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
