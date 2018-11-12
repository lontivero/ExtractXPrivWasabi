
using System;
using System.IO;
using System.Linq;
using System.Text;
using NBitcoin;
using NBitcoin.DataEncoders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tool
{
    class Program
    {
        static void Main(string[] args)
        {
            var walletPath = args[0];
            var password = args[1];

            var encryptedSecret = string.Empty;
            var chainCode = string.Empty;

            var lines = File.ReadAllLines(walletPath, Encoding.UTF8);
            foreach(var line in lines.Select(x=>x.Trim()))
            {
                if(line.Contains("EncryptedSecret"))
                    encryptedSecret = line.Split().Last();
                if(line.Contains("ChainCode"))
                    chainCode = line.Split().Last();
            }

            encryptedSecret = encryptedSecret.Substring(1, encryptedSecret.Length - 3);
            chainCode = chainCode.Substring(1, chainCode.Length - 3);

            var secret = new BitcoinEncryptedSecretNoEC(encryptedSecret, Network.Main);
            var ccode  = Encoders.Base64.DecodeData(chainCode); 

            var extKey = new ExtKey(secret.GetKey(password), ccode);
            Console.WriteLine(extKey.GetWif(Network.Main));
        }
    }
}
