using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;


namespace AlexaComp.Core {
    
    class ProgramInventory {

        private static string rootPath = @"C:\Program Files (x86)";

        public static void scanDir(string dir = @"C:\Program Files (x86)") {
            string[] allfiles = Directory.GetDirectories(dir);
            Console.WriteLine("Files Length - " + allfiles.Length.ToString());
            Console.ReadLine();
            foreach (string subdir in allfiles) {
                // Console.WriteLine("Running Scan - Depth: 1");
                checkForMatches(subdir);

                // Console.WriteLine("Running Scan - Depth: 2");
                string[] files = Directory.GetDirectories(subdir);
                foreach (string subsubdir in files) {
                    // Console.WriteLine("Checking For Matches In - " + subsubdir);
                    checkForMatches(subsubdir);
                }
            }
        }

        public static void checkForMatches(string path) {
            string[] exes = Directory.GetFileSystemEntries(Path.Combine(rootPath, path), "*.exe", SearchOption.TopDirectoryOnly);

            // If there is only one .exe in the directory
            if (exes.Length == 1) {
                foreach (var file in exes) {
                    FileInfo info = new FileInfo(file);
                    string[] splitExePath = path.Split('\\').ToArray();
                    Console.WriteLine("EXE MATCH FOUND: " + splitExePath[splitExePath.Length - 1] + " -- " + info.Name);
                }
            }

            // If multiple .exes
            foreach (var file in exes) {
                FileInfo info = new FileInfo(file);
                string[] splitExePath = path.Split('\\').ToArray();
                if (splitExePath[splitExePath.Length - 1].Replace(" ", "").ToLower() == Path.GetFileNameWithoutExtension(info.Name).ToLower()) {
                    Console.WriteLine("EXE MATCH FOUND: " + splitExePath[splitExePath.Length - 1] + " -- " + info.Name);
                }
            }
        }

        public static string TryGetElementValue(XElement parentEl, string key) {
            var foundEl = parentEl.Element(key);

            if (foundEl != null) {
                return foundEl.Value;
            }

            return null;
        }

        public static string multStr(int n) {
            return "";
        }
    }
}
