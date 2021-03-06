﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectoryScanner
{
    public class NugetManager
    {
        public Version GetVersion(string fileName)
        {
            var parts = fileName.Split('.');
            var versionNumbers = new List<int>();
            //last part will be nuget, ignore that
            foreach (string t in parts)
            {
                int v = 0;
                if (t == "nuget")
                {
                    break;
                }
                if (int.TryParse(t, out v))
                {
                    versionNumbers.Add(v);
                }
            }

            //Minimum version has 2 numbers (i.e 3 => 3.0)
            if (versionNumbers.Count == 1)
            {
                versionNumbers.Add(0);
            }

            var version = Version.Parse(String.Join(".", versionNumbers));
            return version;
        }


        /// <summary>
        /// I am ugly, but I work and I was implemented real fast
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public string GetPackageName(string fileName)
        {
            var parts = fileName.Split('.');
            var nameParts = new List<string>();
            //last part will be nuget, ignore that
            foreach (string t in parts)
            {
                int v = 0;
                if (t == "nuget")
                {
                    break;
                }
                if (int.TryParse(t, out v))
                {
                    break;
                }
                else
                {
                    nameParts.Add(t);
                }
            }

            var packageName = String.Join(".", nameParts);
            return packageName;
        }
    }
}
