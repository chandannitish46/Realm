using Realms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RealmPOC
{
    /// <summary>
    /// SETTING UP THE HASH FOR ENCRYPTION
    /// </summary>
    public static class DbSetUp
    {
        
        public static string CreateDirectorySetup()
        {
            string folder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string specificFolder = Path.Combine(folder, "database");
            if (!Directory.Exists(specificFolder))
                Directory.CreateDirectory(specificFolder);
            return specificFolder;
        }

        public static Realm CreateDbFile(string directory)
        {
            string database = "Employee.realm";
            string filePath = string.Format("{0}//{1}",directory, database);
            RealmConfiguration config = new RealmConfiguration(filePath);
            config.EncryptionKey = HashPassword("pramati@123");
            
            return Realm.GetInstance(config);
          
        }
        
    public const int Pbkdf2Iterations = 1000;
        public const int HashByteSize = 64;
        public static byte[] HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            string saltStr = "237FB56FA6472F15B868B7742CA5875C";
            byte[] salt = new byte[64];
            salt = Convert.FromBase64String(saltStr);
            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            string hashStr = Convert.ToBase64String(hash);
            return hash;
        }
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 1000);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
