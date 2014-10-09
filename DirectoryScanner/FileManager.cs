using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner
{
    public class FileManager
    {
        private IList<string> _filesInSource;
        private IList<string> _filesInDestnation;

        public string Source { get; set; }
        public string Destination { get; set; }

        public IList<string> FilesInSource
        {
            get { return _filesInSource ?? (_filesInSource = GetListOfFileNames(Source)); }
        }

        public IList<string> FilesInDestination
        {
            get { return _filesInDestnation ?? (_filesInDestnation = GetListOfFileNames(Destination)); }
        }

        public FileManager(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        private IList<string> GetListOfFileNames(string folder)
        {
            var sortedList = Directory.GetFiles(folder).ToList();
            sortedList.Sort();
            return sortedList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="packageFileName"></param>
        /// <param name="packageName"></param>
        public void CopyFromSourceToDest(string packageFileName, string packageName)
        {
            //first delete all packages
            foreach (var toDelete in FilesInDestination.Where(x => x.Contains(packageName)))
            {
                File.Delete(Path.Combine(Destination, toDelete));
            }

            //copy file from source to destination
            var sourceFile = Path.Combine(Source, packageFileName);
            var destFile = Path.Combine(Destination, packageFileName);
            File.Copy(sourceFile, destFile, true);
        }
    }
}
