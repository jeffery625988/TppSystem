using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TppSystem
{
    //Need check each function's input with reference (PI_GCS2)
    public class PI_API
    {
        private const string LongXY_DLL = "C7XX_GCS_DLL.dll" 
                            ,ShortXY_DLL = "E7XX_GCS_DLL.dll"
                            ,Z_DLL = "PI_GCS2_DLL.dll";
        #region Long XY
        //Set servo on/off
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_SVO")] public static extern bool C7XX_SVO(int ID, string szAxes, bool pbValarray);
        //Get servo state
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_qSVO")] public static extern bool C7XX_qSVO(int ID, string szAxes, bool pbValarray);
        //Connect controller with TCPIP
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_ConnectTCPIP")] public static extern int C7XX_ConnectTCPIP(string szHostname, int port);
        //Close connection
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_CloseConnection")] public static extern int C7XX_CloseConnection(int ID);
        //Set velocity 
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_VEL")] public static extern bool C7XX_VEL(int ID, string szAxes, double pdValarray);
        //Get velocity
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_qVEL")] public static extern bool C7XX_qVEL(int ID, string szAxes, double pdValarray);
        //Check is connected?
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_IsConnected")] public static extern bool C7XX_IsConnected(int ID);
        [DllImport(LongXY_DLL, EntryPoint = "C7XX_MOV")] public static extern bool C7XX_MOV(int ID, string szAxes, double pdValarray);
        [DllImport(LongXY_DLL, EntryPoint = " C7XX_MVR")] public static extern bool C7XX_MVR(int ID, string szAxes, double pdValarray);

        #endregion

        #region Short XY
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_SVO")] public static extern bool E7XX_SVO(int ID, string szAxes, bool pbValueArray);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_qSVO")] public static extern bool E7XX_qSVO(int ID, string szAxes, bool pbValueArray);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_ConnectTCPIP")] public static extern int E7XX_ConnectTCPIP(string szHostname, int port);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_CloseConnection")] public static extern int E7XX_CloseConnection(int ID);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_VEL")] public static extern bool E7XX_VEL(int ID, string szAxes, double pdValarray);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_qVEL")] public static extern bool E7XX_qVEL(int ID, string szAxes, double pdValarray);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_IsConnected")] public static extern bool E7XX_IsConnected(int ID);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_ConnectNIgpib")] public static extern int E7XX_ConnectNIgpib(int iBoardNumber, int iDeviceAddress);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_MOV")] public static extern int E7XX_MOV(int ID, string szAxes, double pdValueArray);
        [DllImport(ShortXY_DLL, EntryPoint = "E7XX_MVR")] public static extern int E7XX_MVR(int ID, string szAxes, double pdValueArray);
        #endregion

        #region Z axid
        [DllImport(Z_DLL, EntryPoint = "PI_SVO")] public static extern bool PI_SVO(int ID, string szAxes, bool pbValueArray);
        [DllImport(Z_DLL, EntryPoint = "PI_qSVO")] public static extern bool PI_qSVO(int ID, string szAxes, bool pbValueArray);
        [DllImport(Z_DLL, EntryPoint = "PI_ConnectTCPIP")] public static extern int PI_ConnectTCPIP(string szHostname, int port);
        [DllImport(Z_DLL, EntryPoint = "PI_ConnectUSB")] public static extern int PI_ConnectUSB(string szDescription);
        [DllImport(Z_DLL, EntryPoint = "PI_VEL")] public static extern bool PI_VEL(int ID, string szAxes, double pdValarray);
        [DllImport(Z_DLL, EntryPoint = "PI_qVEL")] public static extern bool PI_qVEL(int ID, string szAxes, double pdValarray);
        [DllImport(Z_DLL, EntryPoint = "PI_IsConnected")] public static extern bool PI_IsConnected(int ID);
        [DllImport(Z_DLL, EntryPoint = "PI_CloseConnection")] public static extern void PI_CloseConnection(int ID);
        [DllImport(Z_DLL, EntryPoint = "PI_MOV")] public static extern void PI_MOV(int ID, string szAxes, double pdValueArray);
        [DllImport(Z_DLL, EntryPoint = "PI_MVR")] public static extern void PI_MVR(int ID, string szAxes, double pdValueArray);
        #endregion

    }
}
