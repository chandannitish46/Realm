using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;



namespace RealmPOC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ViewModel vm = new ViewModel();
            this.DataContext = vm;
            // var realm = Realm.GetInstance();

            // // Use LINQ to query
            //// => 0 because no dogs have been added yet

            // // Update and persist objects with a thread-safe transaction
            // realm.Write(() =>
            // {
            //     realm.Add(new Dog { Name = "Rex", Age = 1 });
            //     realm.Add(new Dog { Name = "Rex1", Age = 2 });
            //     realm.Add(new Dog { Name = "Rex2", Age = 3 });
            // });

            // var puppies = realm.All<Dog>().Where(d => d.Age < 2);


            // // Queries are updated in realtime
            // puppies.Count(); // => 1

            // // LINQ query syntax works as well
            // var oldDogs = from d in realm.All<Dog>() where d.Age > 8 select d;

            // // Query and update from any thread
            // new Thread(() =>
            // {
            //     var realm2 = Realm.GetInstance();
            //     var theDog = realm2.All<Dog>().Where(d => d.Age == 1).First();
            //     realm2.Write(() => theDog.Age = 3);
            // }).Start();
            //CreateConfiguration();
        }

        private void CreateConfiguration()
        {
            // var config = new RealmConfiguration(@"C:\databases\RealmDB\OtherRealm.realm");
           
           // HashPassword  ("pramati123");
            string str = "ZHT99zNNBqinBTRO9tMSu+FR7DxHi/yW+Z1yYW7EbucVaB7OpnzburLdOgHV7tsdMnHoxcIQd1NVLykaeMBlNw==";
            byte[] barr = System.Convert.FromBase64String(str);

            //    config.EncryptionKey = HashPassword("pramati123");
            SameHash("pramati123", "Wfvt8218jZGSDVKsVhMxXnEKkIPtZsNvJcBCPSeHrQQgChTFb6NDFSjQb88lvzwWbGxkni7poKtBOZEE5mgjvw =="
                , "cIc+vWw/7rwA121EdG4wAdMY130c9Bh/5xyQoccSQmHaAqNkjMugl4FXyV3Ow6lU9gldARv46C/bSlqh6oFO6Q==");
                //HashPassword("pramati123");
           // config.EncryptionKey = barr;
          //  var otherRealm = Realm.GetInstance(config);
            // var newPeople= otherRealm.All<Employee>().Where(x=>x.Id==1);
            //new
            // //otherRealm.Find("Employee")
            //otherRealm.Write(() =>
            //{
            //    otherRealm.Add(new Employee
            //    {
            //        Id = 1,
            //        FirstName = "Nitish",
            //        LastName = "Chandan",
            //        Department = "Hyperion"
            //    });
            //    otherRealm.Add(new Employee
            //    {
            //        Id = 2,
            //        FirstName = "Nikhil",
            //        LastName = "Ranjan",
            //        Department = "AWS"
            //    });
            //});
            
        }

        public static bool SameHash(string userpwd, string storedHash, string storedSalt)
        {
            var saltByte = Encoding.UTF8.GetBytes(storedSalt);
            var rfc = new Rfc2898DeriveBytes(userpwd, saltByte, 1000);
            var baseString = Convert.ToBase64String(rfc.GetBytes(64));
            bool c = false;
            c = (baseString == storedHash);
                 
            return baseString == storedHash;
        }
        public const int SaltByteSize = 64;
        public const int HashByteSize = 64; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        public const int Pbkdf2Iterations = 1000;
        public const int IterationIndex = 0;
        public const int SaltIndex = 1;
        public const int Pbkdf2Index = 2;

        public byte[]  HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            string saltStr = "237FB56FA6472F15B868B7742CA5875C";
            byte[] salt = new byte[32];
            salt = Convert.FromBase64String(saltStr);
            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
           // string saltStr = Convert.ToBase64String(salt);
            string hashStr = Convert.ToBase64String(hash);
            return hash;
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt,1000);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
