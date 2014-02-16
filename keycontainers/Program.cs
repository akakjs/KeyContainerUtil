using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace keycontainers
{
    class Program
    {
        static void Main(string[] args)
        {
            const string KEY_STORE_NAME = "testKeyStore.1.0.0.1";

            var bob = new CspParameters();
            bob.Flags = CspProviderFlags.UseArchivableKey | CspProviderFlags.UseMachineKeyStore;
            bob.KeyContainerName = KEY_STORE_NAME;

            Enumerate(bob);

            var rsaCSP = new RSACryptoServiceProvider(bob);

            //Enumerate(rsaCSP);
            //Enumerate(rsaCSP.CspKeyContainerInfo);

            var rsaParametersInPrivate = rsaCSP.ExportParameters(true);

            Enumerate(rsaParametersInPrivate);

            Console.Read();
        }

        private static void Enumerate<T>(T instance)
        {
            if (instance == null)
                return;

            var t = typeof (T);
            Console.WriteLine("-----------------------");
            Console.WriteLine(t.Name);
            Console.WriteLine("-----------------------");

            var propInfos = t.GetProperties();

            foreach (var propertyInfo in propInfos)
            {
                Console.WriteLine(String.Format("{0} ==== {1}", propertyInfo.Name, propertyInfo.GetValue(instance)));
            }

            var fieldInfos = t.GetFields();
            foreach (var fieldInfo in fieldInfos)
            {
                var value = fieldInfo.GetValue(instance);
                var bytes = value as byte[];
                if (bytes != null)
                {
                    Console.WriteLine("{0} ==== {1} ({2})", fieldInfo.Name, bytes, bytes.Length);
                }
                else
                {
                    Console.WriteLine("{0} ==== {1}", fieldInfo.Name, value);
                }
            }
        }
    }
}
