using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace AlexaComp.Core {
    
    class ProgramInventory : AlexaCompCore {

        private static readonly string rootPath = @"C:\Program Files (x86)";
        private static int numMatches = 0;

        public static void ScanDir(string dir = @"C:\Program Files (x86)") {
            Stopwatch timer = new Stopwatch();
            timer.Start();
            Clog("Scanning for .exes in - " + dir);
            try {
                string[] allfiles = Directory.GetDirectories(dir);
                Console.WriteLine("Files Length - " + allfiles.Length.ToString());
                foreach (string subdir in allfiles) {
                    // Console.WriteLine("Running Scan - Depth: 1");
                    CheckForMatches(subdir);

                    // Console.WriteLine("Running Scan - Depth: 2");
                    string[] files = Directory.GetDirectories(subdir);
                    foreach (string subsubdir in files) {
                        // Console.WriteLine("Checking For Matches In - " + subsubdir);
                        CheckForMatches(subsubdir);
                    }
                }
            } catch (UnauthorizedAccessException e) {
                Clog("UnauthorizedAccessException Caught When Scanning Dir" + e.Message, "ERROR");
            }
            timer.Stop();
            Clog(String.Format("Scanned dir in {0} ms. Found {1} matches.", timer.ElapsedMilliseconds, numMatches));
            numMatches = 0;
        }

        public static void CheckForMatches(string path) {
            string[] exes = Directory.GetFileSystemEntries(Path.Combine(rootPath, path), "*.exe", SearchOption.TopDirectoryOnly);

            // If there is only one .exe in the directory
            if (exes.Length == 1) {
                foreach (var file in exes) {
                    FileInfo info = new FileInfo(file);
                    string[] splitExePath = path.Split('\\').ToArray();
                    Clog("EXE MATCH FOUND: " + splitExePath[splitExePath.Length - 1] + " -- " + info.Name);
                    numMatches++;
                }
            }

            // If multiple .exes
            foreach (var file in exes) {
                FileInfo info = new FileInfo(file);
                string[] splitExePath = path.Split('\\').ToArray();
                if (splitExePath[splitExePath.Length - 1].Replace(" ", "").ToLower() == Path.GetFileNameWithoutExtension(info.Name).ToLower()) {
                    Clog("EXE MATCH FOUND: " + splitExePath[splitExePath.Length - 1] + " -- " + info.Name);
                    numMatches++;
                }
            }
        }
    }
}
