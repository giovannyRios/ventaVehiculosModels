using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ventaVehiculosModels.Models.Log;
using System.Configuration;

namespace ventaVehiculosModels.Models.cifrado
{
    public static class implementacionCifrado
    {
        /// <summary>
        /// Metodo de implementacion de cifrado en AES256
        /// </summary>
        /// <param name="valUser"></param>
        /// <param name="PlainText"></param>
        /// <returns></returns>
        public static string cifrar(string valUser, string PlainText)
        {
            try
            {
                string llaveUsuario = ConfigurationManager.AppSettings["SecretUser"].ToString();
                string llaveSecreta = ConfigurationManager.AppSettings["Secretkey"].ToString();
                cifradoClass objCifrado = cifradoClass.instance(valUser);
                return objCifrado.cifrarEnString(PlainText, llaveSecreta, llaveUsuario);
            }
            catch (Exception ex)
            {
                cunsumirLog.crearRegistroLog("ventaVehiculosModels" + DateTime.Now.ToShortDateString(), "Ha ocurrido un error al momento de cifrar: " + ex.ToString());
                return "";
            }     
        }


        /// <summary>
        /// Metodo de implementacion para descifrar AES256
        /// </summary>
        /// <param name="valUser"></param>
        /// <param name="textEncrypt"></param>
        /// <returns></returns>
        public static string Descifrar(string valUser, string textEncrypt)
        {
            try
            {
                string llaveUsuario = ConfigurationManager.AppSettings["SecretUser"].ToString();
                string llaveSecreta = ConfigurationManager.AppSettings["Secretkey"].ToString();
                cifradoClass objCifrado = cifradoClass.instance(valUser);
                return objCifrado.descifrarEnString(textEncrypt, llaveSecreta, llaveUsuario);
            }
            catch (Exception ex)
            {
                cunsumirLog.crearRegistroLog("ventaVehiculosModels" + DateTime.Now.ToShortDateString(), "Ha ocurrido un error al momento de cifrar: " + ex.ToString());
                return "";
            }
        }
    }
}