using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Runtime.InteropServices;


namespace TppSystem
{
    public class EPCIO_API
    {
        //AOM EPCIO library

        //Strcut
        public struct DDAINT
        {
            byte FIFO0;
            byte FIFO1;
            byte FIFO2;
            byte FIFO3;
            byte FIFO4;
            byte FIFO5;
            byte CYCLE;
        }

        public struct ENCINT
        {
            byte COMP0;
            byte COMP1;
            byte COMP2;
            byte INDEX0;
            byte INDEX1;
            byte INDEX2;
        }
        public struct RIOINT
        {
            byte S0DI0;
            byte S0DI1;
            byte S0DI2;
            byte S0DI3;
            byte S1DI0;
            byte S1DI1;
            byte S1DI2;
            byte S1DI3;
            byte S2DI0;
            byte S2DI1;
            byte S2DI2;
            byte S2DI3;
            byte FAIL;
        }
        public struct ADCINT
        {
            byte COMP0;
            byte COMP1;
            byte COMP2;
            byte COMP3;
            byte COMP4;
            byte COMP5;
            byte COMP6;
            byte COMP7;
            byte CONV;
            byte TAG;
        }
        public struct LIOINT
        {
            byte LDI0;
            byte LDI1;
            byte LDI2;
            byte LDI3;
            byte LDI4;
            byte LDI5;
            byte LDI6;
            byte LDI7;
            byte DFI0;
            byte DFI1;
            byte DFI2;
            byte DFI3;
            byte DFI4;
            byte DFI5;
            byte DFI6;
            byte TIMER;
        }
        public struct PCLINT
        {
            byte OV0;
            byte OV1;
            byte OV2;
            byte OV3;
            byte OV4;
            byte OV5;
        }
        //Delegate
        public delegate void DDAISR(ref DDAINT value);
        public delegate void ENCISR(ref ENCINT value);
        public delegate void RIOISR(ref RIOINT value);
        public delegate void ADCISR(ref ADCINT value);
        public delegate void LIOISR(ref LIOINT value);
        public delegate void PCLISR(ref PCLINT value); 
        //dll path
        const string EPCIO_dll = "MEPCIOPCIDrv.dll"; 
        
        //Set output
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_DAC_SetOutput")] public static extern bool EPCIO_DAC_SetOutput(ushort channel, float value, ushort cardID);

        //Stop convertion
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_DAC_StopConv")] public static extern bool EPCIO_DAC_StopConv(ushort cardID);

        //Close EPCIO
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_Close")] public static extern bool EPCIO_Close(ushort cardID);

        //Initialize 
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO6000_Init")] public static extern bool EPCIO6000_Init(DDAISR ddaisr, ENCISR encisr, ENCISR encisr1, ENCISR encisr2 , RIOISR rioisr, RIOISR rioisr2, ADCISR adcisr, LIOISR lioisr, PCLISR pclisr, int cardID);

        
        //Reset module
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_ResetModule")] public static extern bool EPCIO_ResetModule(ushort setting, ushort cardID);

        //enable pulse and DAC
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_LIO_EnablePulseDAC")] public static extern bool EPCIO_LIO_EnablePulseDAC( ushort cardID);

        //Set servo on
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_LIO_ServoOn")] public static extern bool EPCIO_LIO_ServoOn(ushort channel, ushort cardID);

        //Set serial write frequency divdied value = 10
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_DAC_SetClockDivider")] public static extern bool EPCIO_DAC_SetClockDivider(float value, ushort cardID);

        //Set DAC command
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_DAC_SetCmdSource")] public static extern bool EPCIO_DAC_SetCmdSource(ushort channel, ushort cmd, ushort cardID);

        //Start convertion
        [DllImport(EPCIO_dll, EntryPoint = "EPCIO_DAC_StartConv")] public static extern bool EPCIO_DAC_StartConv(ushort cardID);


  
        
    }
}
