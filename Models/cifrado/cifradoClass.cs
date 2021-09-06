using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using ventaVehiculosModels.Models.Log;

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
            algoritmo.BlockSize = TamanioLlave;
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
            algoritmo.BlockSize = TamanioLlave;
            algoritmo.IV = llaveConstructora.GetBytes((int)(algoritmo.BlockSize / 8));
            algoritmo.Key = llaveConstructora.GetBytes((int)(algoritmo.KeySize / 8));
            algoritmo.Padding = PaddingMode.PKCS7;
            return algoritmo;
        }

        public string cifrarAes(string key, string IV, string plainText, string user) 
        {
            try 
            {
                string resultado = "";
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(IV))
                {
                    throw new ArgumentNullException("key or IV is null or Empty");
                }

                using (AesManaged AesAlg = new AesManaged())
                {
                    AesAlg.Key = Encoding.ASCII.GetBytes(key);
                    AesAlg.IV = Encoding.ASCII.GetBytes(IV);
                    AesAlg.KeySize = 256;
                    AesAlg.BlockSize = 256;
                    ICryptoTransform cryptoTransform = AesAlg.CreateEncryptor(AesAlg.Key, AesAlg.IV);
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream crypto = new CryptoStream(memoryStream,cryptoTransform,CryptoStreamMode.Write))
                        {
                            using (StreamWriter stream = new StreamWriter(crypto))
                            {
                                stream.Write(plainText);
                            }
                            resultado = System.Text.Encoding.Default.GetString(memoryStream.ToArray());
                        }
                    }

                }

                return resultado;
            } 
            catch (Exception ex)
            {
                consumirLog.crearRegistroLog(user,ex.ToString());
                throw new Exception(ex.ToString());
            }

        }

        public string desCifrarAes(string key, string IV, string TextEncrypted, string user)
        {
            try
            {
                string resultado = "";
                if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(IV))
                {
                    throw new ArgumentNullException("key or IV is null or Empty");
                }

                using (AesManaged AesAlg = new AesManaged())
                {
                    AesAlg.Key = Encoding.ASCII.GetBytes(key);
                    AesAlg.IV = Encoding.ASCII.GetBytes(IV);
                    AesAlg.KeySize = 256;
                    AesAlg.BlockSize = 256;
                    ICryptoTransform cryptoTransform = AesAlg.CreateEncryptor(AesAlg.Key, AesAlg.IV);
                    using (MemoryStream memoryStream = new MemoryStream(Encoding.ASCII.GetBytes(TextEncrypted)))
                    {
                        using (CryptoStream crypto = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                        {
                            using (StreamReader stream = new StreamReader(crypto))
                            {
                                resultado = stream.ReadToEnd();
                            }
                        }
                    }

                }

                return resultado;

            }
            catch (Exception ex)
            {
                consumirLog.crearRegistroLog(user, ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

    }
}
