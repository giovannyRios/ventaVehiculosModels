using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ventaVehiculosModels.Models.Log
{
    public static class consumirLog
    {
        public static void crearRegistroLog(string valUser, string valMessageLog)
        {
            CreacionLog objLog = CreacionLog.instance(valUser);
            objLog.escribirLog(valMessageLog);
        }
    }
}