using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Text;

namespace ventaVehiculosModels.Models.Log
{
    class CreacionLog
    {
        #region patron_singleton

        private static CreacionLog _instance;
        public string Value { get; private set; }
        private CreacionLog() { }

        private static readonly object _lock = new object();
        public static CreacionLog instance(string valor)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new CreacionLog();
                        _instance.Value = valor;
                    }
                }
            }
            return _instance;
        }

        #endregion

        /// <summary>
        /// Metodo que permite escribir archivo en txt con formato
        /// </summary>
        /// <param name="log"></param>
        public void escribirLog(string log)
        {
            try
            {

                string ruta = ConfigurationManager.AppSettings["rutaLog"].ToString();
                string nombreArchivoSinFecha = ConfigurationManager.AppSettings["nombreArchivoLog"].ToString();
                string nombreFecha = nombreArchivoSinFecha.Replace("Fecha", DateTime.Now.ToShortDateString());
                string rutaFinal = ruta + nombreFecha + ".txt";

                if (!File.Exists(rutaFinal)) 
                {
                    using (FileStream fs = File.Create(rutaFinal))
                    {
                        byte[] informacion = new UTF8Encoding(true).GetBytes(log);
                        fs.Write(informacion, 0, informacion.Length);
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(rutaFinal, append:true))
                    {
                        sw.WriteLine(log);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}