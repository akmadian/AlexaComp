using System;
using System.Collections.Generic;
using System.Configuration;
using AlexaComp.Controllers;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security.Cryptography;

using log4net;
using log4net.Config;

namespace AlexaComp {
    
    // MAIN TODO LIST
    // TODO : Implement input validation on all functions.
    // TODO : Create format validation functions.

    class AlexaCompCore {

        public static string exePath = System.Reflection.Assembly.GetEntryAssembly().Location;
        private static string[] splitExePath = exePath.Split('\\').ToArray();
        public static string pathToDebug = string.Join("\\\\", splitExePath.Reverse().Skip(1).Reverse());
        public static string pathToProject = string.Join("\\\\", splitExePath.Reverse().Skip(3).Reverse());

        // Threads
        public static Thread AppWindowThread = new Thread(AlexaCompGUI.StartAppWindow);
        public static Thread ServerThread = new Thread(AlexaCompSERVER.startServer);
        public static Thread ServerLoopThread = new Thread(AlexaCompSERVER.ServerLoop);
        public static Thread LoadingScreenThread = new Thread(LoadingScreenForm.startLoadingScreen);
        public static Thread LightingControlThread = new Thread(LightingController.startLightingThread);

        // Misc
        public static bool updateLogBoxFlag = false;
        public static bool stopProgramFlag = false;
        public static Dictionary<string, string> settingsDict = new Dictionary<string, string>();
        public static readonly ILog _log = LogManager.GetLogger(typeof(AlexaComp));

        // Controller Instances
        public static LightingController RGBController = new LightingController();
        public static AudioController SoundController = new AudioController();

        public static void clog(string tolog, string customLevel = "INFO") {
            switch (customLevel) {
                case "ERROR": _log.Error(tolog); break;
                case "INFO" : _log.Info(tolog);  break;
                case "WARN" : _log.Warn(tolog);  break;
                case "DEBUG": _log.Debug(tolog); break;
                case "FATAL": _log.Fatal(tolog); break;
            }
            Console.WriteLine(tolog);
        }

        public static void stopApplication() {
            clog("CLOSING PROGRAM");
            stopProgramFlag = true;
            // AlexaCompSERVER.stopServer();
            AlexaCompSERVER.delPortMap();
            
            Environment.Exit(1);
        }

        public static string[] splitStringEveryN(string toSplit, int n) {
            List<string> returnArr = new List<string>();
            for (int i = 0; i < toSplit.Length; i += n) {
                returnArr.Add(toSplit.Substring(i, Math.Min(n, toSplit.Length - i)));
            }
            return returnArr.ToArray();
        }

        public static bool validateRGB(RGBColor color) {
            int[] rgbArr = colorMethods.RGBToArr(color);
            foreach (var value in rgbArr) {
                if (255 < value || value < 0) {
                    clog("Invalid RGB - Value Greater Than 255, or Less Than 0.");
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Encryption Methods From: https://odan.github.io/2017/08/10/aes-256-encryption-and-decryption-in-php-and-csharp.html
        /// </summary>
        #region encryption
        public static string encryptString(string plainText, byte[] key, byte[] iv) {
            Aes encryptor = Aes.Create();
            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;
        }

        /*
        public static string DecryptString(string cipherText, byte[] key, byte[] iv) {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;

            // Set key and IV
            byte[] aesKey = new byte[32];
            Array.Copy(key, 0, aesKey, 0, 32);
            encryptor.Key = aesKey;
            encryptor.IV = iv;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            string plainText = String.Empty;

            try {
                // Convert the ciphertext string into a byte array
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                byte[] plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainText;
        }*/

        public static string Decrypt(string cipherData, string keyString, string ivString) {
            byte[] key = Encoding.UTF8.GetBytes(keyString);
            byte[] iv = Encoding.UTF8.GetBytes(ivString);

            try {
                using (var rijndaelManaged =
                       new RijndaelManaged { Key = key, IV = iv, Mode = CipherMode.CBC })
                using (var memoryStream =
                       new MemoryStream(Convert.FromBase64String(cipherData)))
                using (var cryptoStream =
                       new CryptoStream(memoryStream,
                           rijndaelManaged.CreateDecryptor(key, iv),
                           CryptoStreamMode.Read)) {
                    return new StreamReader(cryptoStream).ReadToEnd();
                }
            }
            catch (CryptographicException e) {
                Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
                return null;
            }
            // You may want to catch more exceptions here...
        }

        private static string DecryptStringv2(string cipherText, string key) {

            string plaintext = null;

            byte[] _initialVector = new byte[16];
            byte[] _cipherTextBytesArray = Convert.FromBase64String(cipherText);
            byte[] _originalString = new byte[_cipherTextBytesArray.Length - 16];

            Array.Copy(_cipherTextBytesArray, 0, _initialVector, 0, 16);
            Array.Copy(_cipherTextBytesArray, 16, _originalString, 0, _cipherTextBytesArray.Length - 16);

            using (Aes _aesAlg = Aes.Create()) {
                _aesAlg.Key = Encoding.ASCII.GetBytes(key);
                _aesAlg.IV = _initialVector;
                ICryptoTransform decryptor = _aesAlg.CreateDecryptor(_aesAlg.Key, _aesAlg.IV);

                using (MemoryStream _memoryStream = new MemoryStream(_originalString)) {
                    using (CryptoStream _cryptoStream = new CryptoStream(_memoryStream, decryptor, CryptoStreamMode.Read)) {
                        using (StreamReader _streamReader = new StreamReader(_cryptoStream)) {
                            plaintext = _streamReader.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        public static string easyDecrypt(string toDecrypt) {
            SHA256 mySHA256 = SHA256Managed.Create();
            string encrptKey = GetConfigValue("ENCRYPTIONKEY");

            return null;
            // return DecryptStringv2();
        }

        public static string easyEncrypt(string toEncrypt) {
            SHA256 mySHA256 = SHA256Managed.Create();
            string encrptKey = GetConfigValue("ENCRYPTIONKEY");
            byte[] key = mySHA256.ComputeHash(Encoding.ASCII.GetBytes(encrptKey));
            return null;
            //return encryptString(toEncrypt, key, iv);
        }

        #endregion
        /*
        * Gets a specified config value
        * @ param key - The name or key of the config value to get.
        */
        public static string GetConfigValue(string key) {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
