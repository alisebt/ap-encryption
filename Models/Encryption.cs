using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;

namespace SignatureSample.Models
{
    public class Encryption
    {
        string filePath = @"D:\Dropbox\Desktop\Documents\AsanPardakht\Bridge\NewCert\gapfilm.p12";

        public string Encrypt()
        {
            var data = "{\"hi\":2384,\"htran\":6263090,\"htime\":1663066044,\"hop\":2001,\"hkey\":\"df238cdf-f779-4ff6-b2eb-b44dfa2b74b7\",\"ao\":652910,\"merch\":\"35571\",\"stime\":1663066044,\"utran\":12955,\"Stkn\":\"vf68573d1ffqrp2vl2kfiuf13t\"}";
            var file = System.IO.File.ReadAllText(filePath);
            var signedData = SignData(Encoding.Unicode.GetBytes(data), filePath, "=E=QY]!{PjD53Mq");
            return signedData;
        }

        public static string SignData(byte[] data, string pkcs12File, string pkcs12Password)
        {
            X509Certificate2 signerCert = new X509Certificate2(pkcs12File, pkcs12Password, X509KeyStorageFlags.Exportable);
            RSACryptoServiceProvider rsaCSP = new RSACryptoServiceProvider();
            rsaCSP.FromXmlString(signerCert.PrivateKey.ToXmlString(true));
            var SignedData = rsaCSP.SignData(data, CryptoConfig.MapNameToOID("SHA256"));
            return Convert.ToBase64String(SignedData);
        }

        public static bool VerifySignature(byte[] data, string signature, string publicCert)
        {
            X509Certificate2 partnerCert = new X509Certificate2(publicCert);
            RSACryptoServiceProvider rsaCSP = (RSACryptoServiceProvider)partnerCert.PublicKey.Key;
            return rsaCSP.VerifyData(data, CryptoConfig.MapNameToOID("SHA256"), Convert.FromBase64String(signature));
        }
    }
}