using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ventaVehiculosModels.Models.cifrado
{
    class cifradoClass
    {

        #region singleton
        private static cifradoClass _instance;

        public string value;
        private cifradoClass() { }

        private static readonly object _lock = new object();
        public static cifradoClass instance(string valor)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new cifradoClass();
                        _instance.value = valor;
                    }
                }
            }

            return _instance;
        }
        #endregion
        public byte[] cifrarEnBytes(string textoPlano, string llaveSecreta)
        {
            byte[] textoCifrado;
            using (var salidaStrean = new MemoryStream())
            {
                RijndaelManaged Algoritmo = obtenerAlgoritmoDinamico(llaveSecreta);
                using (var crypto = new CryptoStream(salidaStrean, Algoritmo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(textoPlano);
                    crypto.Write(buffer, 0, buffer.Length);
                    crypto.FlushFinalBlock();
                    textoCifrado = salidaStrean.ToArray();
                }
            }
            return textoCifrado;
        }
        public byte[] cifrarEnBytes(string textoPlano, string llaveSecreta, string llaveUsuario)
        {
            byte[] textoCifrado;
            using (var salidaStrean = new MemoryStream())
            {
                RijndaelManaged Algoritmo = obtenerAlgoritmoEstatico(llaveSecreta, llaveUsuario);
                using (var crypto = new CryptoStream(salidaStrean, Algoritmo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(textoPlano);
                    crypto.Write(buffer, 0, buffer.Length);
                    crypto.FlushFinalBlock();
                    textoCifrado = salidaStrean.ToArray();
                }
            }
            return textoCifrado;
        }
        public string descifrarBytes(byte[] textoCifrado, string llaveSecreta)
        {
            string textoDescifrado = "";
            using (var memoryStream = new MemoryStream(textoCifrado))
            {
                RijndaelManaged algoritmo = obtenerAlgoritmoDinamico(llaveSecreta);
                using (var crypto = new CryptoStream(memoryStream, algoritmo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] salidaBuffer = new byte[memoryStream.Length - 1];
                    int readBytes = crypto.Read(salidaBuffer, 0, (int)memoryStream.Length);
                    textoDescifrado = Encoding.Unicode.GetString(salidaBuffer, 0, readBytes);
                }
            }
            return textoDescifrado;
        }
        public string descifrarBytes(byte[] textoCifrado, string llaveSecreta, string llaveUsuario)
        {
            string textoDescifrado = "";
            using (var memoryStream = new MemoryStream(textoCifrado))
            {
                RijndaelManaged algoritmo = obtenerAlgoritmoEstatico(llaveSecreta, llaveUsuario);
                using (var crypto = new CryptoStream(memoryStream, algoritmo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] salidaBuffer = new byte[memoryStream.Length - 1];
                    int readBytes = crypto.Read(salidaBuffer, 0, (int)memoryStream.Length);
                    textoDescifrado = Encoding.Unicode.GetString(salidaBuffer, 0, readBytes);
                }
            }
            return textoDescifrado;
        }

        public string cifrarEnString(string textoPlano, string llaveSecreta, string llaveUsuario)
        {
            string textoCifrado;
            using (var salidaStrean = new MemoryStream())
            {
                RijndaelManaged Algoritmo = obtenerAlgoritmoEstatico(llaveSecreta, llaveUsuario);
                using (var crypto = new CryptoStream(salidaStrean, Algoritmo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(textoPlano);
                    crypto.Write(buffer, 0, buffer.Length);
                    crypto.FlushFinalBlock();
                    textoCifrado = Convert.ToBase64String(salidaStrean.ToArray());
                }
            }
            return textoCifrado;
        }
        public string cifrarEnString(string textoPlano, string llaveSecreta)
        {
            string textoCifrado;
            using (var salidaStrean = new MemoryStream())
            {
                RijndaelManaged Algoritmo = obtenerAlgoritmoDinamico(llaveSecreta);
                using (var crypto = new CryptoStream(salidaStrean, Algoritmo.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    byte[] buffer = Encoding.Unicode.GetBytes(textoPlano);
                    crypto.Write(buffer, 0, buffer.Length);
                    crypto.FlushFinalBlock();
                    textoCifrado = Convert.ToBase64String(salidaStrean.ToArray());
                }
            }
            return textoCifrado;
        }

        public string descifrarEnString(string textoCifrado, string llaveScreta)
        {
            string textoDescifrado = "";
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(textoCifrado)))
            {
                RijndaelManaged algoritmo = obtenerAlgoritmoDinamico(llaveScreta);
                using (var crypto = new CryptoStream(memoryStream, algoritmo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] salidaBuffer = new byte[memoryStream.Length - 1];
                    int readBytes = crypto.Read(salidaBuffer, 0, (int)memoryStream.Length - 1);
                    textoDescifrado = Encoding.Unicode.GetString(salidaBuffer, 0, readBytes);
                }
            }
            return textoDescifrado;
        }
        public string descifrarEnString(string textoCifrado, string llaveScreta, string llaveUsuario)
        {
            string textoDescifrado = "";
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(textoCifrado)))
            {
                RijndaelManaged algoritmo = obtenerAlgoritmoEstatico(llaveScreta, llaveUsuario);
                using (var crypto = new CryptoStream(memoryStream, algoritmo.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    byte[] salidaBuffer = new byte[memoryStream.Length - 1];
                    int readBytes = crypto.Read(salidaBuffer, 0, (int)memoryStream.Length - 1);
                    textoDescifrado = Encoding.Unicode.GetString(salidaBuffer, 0, readBytes);
                }
            }
            return textoDescifrado;
        }

        public RijndaelManaged obtenerAlgoritmoDinamico(string llaveEncripcion)
        {
            string saltoAlgoritmo = System.Guid.NewGuid().ToString();
            const int TamanioLlave = 256;
            Rfc2898DeriveBytes llaveConstructora = new Rfc2898DeriveBytes(llaveEncripcion, Encoding.Unicode.GetBytes(saltoAlgoritmo));
            RijndaelManaged algoritmo = new RijndaelManaged();
            algoritmo.KeySize = TamanioLlave;
            algoritmo.IV = llaveConstructora.GetBytes((int)(algoritmo.BlockSize / 8));
            algoritmo.Key = llaveConstructora.GetBytes((int)(algoritmo.KeySize / 8));
            algoritmo.Padding = PaddingMode.PKCS7;
            return algoritmo;
        }

        public RijndaelManaged obtenerAlgoritmoEstatico(string llaveEncripcion, string llaveUsuario)
        {
            const int TamanioLlave = 256;
            Rfc2898DeriveBytes llaveConstructora = new Rfc2898DeriveBytes(llaveEncripcion, Encoding.Unicode.GetBytes(llaveUsuario));
            RijndaelManaged algoritmo = new RijndaelManaged();
            algoritmo.KeySize = TamanioLlave;
            algoritmo.IV = llaveConstructora.GetBytes((int)(algoritmo.BlockSize / 8));
            algoritmo.Key = llaveConstructora.GetBytes((int)(algoritmo.KeySize / 8));
            algoritmo.Padding = PaddingMode.PKCS7;
            return algoritmo;
        }


    }
}
