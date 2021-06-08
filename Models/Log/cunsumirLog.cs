using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ventaVehiculosModels.Models.Log
{
    public static class cunsumirLog
    {
        public static void crearRegistroLog(string valUser, string valMessageLog)
        {
            CreacionLog objLog = CreacionLog.instance(valUser);
            objLog.escribirLog(valMessageLog);
        }
    }
}