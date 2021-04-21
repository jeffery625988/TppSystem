#if !defined(_EPCIO_DEV_H_)
#define _EPCIO_DEV_H_

#ifdef WIN32
	#include <windows.h>
#else // WIN32
	typedef unsigned long		DWORD;
	typedef unsigned short int	WORD;
	typedef unsigned char		BYTE;
	typedef int					BOOL;

	#define TRUE				1
	#define FALSE				0
#endif // !WIN32

#ifdef __cplusplus
extern "C" {
#endif

typedef struct _EPCIO_INT
{
	BYTE DDA;
	BYTE ENC012;
	BYTE ENC345;
	BYTE ENC678;
	BYTE RIO0;
	BYTE RIO1;
	BYTE ADC;
	BYTE LIO;
	BYTE PCL;
} EPCIOINT;

typedef struct _DDA_INT
{
	BYTE FIFO0;
	BYTE FIFO1;
	BYTE FIFO2;
	BYTE FIFO3;
	BYTE FIFO4;
	BYTE FIFO5;
	BYTE CYCLE;
} DDAINT;

typedef struct _ENC_INT
{
	BYTE COMP0;
	BYTE COMP1;
	BYTE COMP2;
	BYTE INDEX0;
	BYTE INDEX1;
	BYTE INDEX2;
} ENCINT;

typedef struct _RIO_INT
{
	BYTE S0DI0;
	BYTE S0DI1;
	BYTE S0DI2;
	BYTE S0DI3;
	BYTE S1DI0;
	BYTE S1DI1;
	BYTE S1DI2;
	BYTE S1DI3;
	BYTE S2DI0;
	BYTE S2DI1;
	BYTE S2DI2;
	BYTE S2DI3;
	BYTE FAIL;
} RIOINT;

typedef struct _ADC_INT
{
	BYTE COMP0;
	BYTE COMP1;
	BYTE COMP2;
	BYTE COMP3;
	BYTE COMP4;
	BYTE COMP5;
	BYTE COMP6;
	BYTE COMP7;
	BYTE CONV;
	BYTE TAG;
} ADCINT;

typedef struct _LIO_INT
{
	BYTE LDI0;
	BYTE LDI1;
	BYTE LDI2;
	BYTE LDI3;
	BYTE LDI4;
	BYTE LDI5;
	BYTE LDI6;
	BYTE LDI7;
	BYTE DFI0;
	BYTE DFI1;
	BYTE DFI2;
	BYTE DFI3;
	BYTE DFI4;
	BYTE DFI5;
	BYTE DFI6;
	BYTE TIMER;
} LIOINT;

typedef struct _PCL_INT
{
	BYTE OV0;
	BYTE OV1;
	BYTE OV2;
	BYTE OV3;
	BYTE OV4;
	BYTE OV5;
} PCLINT;

typedef struct _LIO_INT_EX
{
	BYTE LDI0;
	BYTE LDI1;
	BYTE LDI2;
	BYTE LDI3;
	BYTE LDI4;
	BYTE LDI5;
	BYTE LDI6;
	BYTE TIMER;
} LIOINT_EX;

typedef struct _ENC_INT_EX
{
	BYTE COMP0;
	BYTE COMP1;
	BYTE COMP2;
	BYTE COMP3;
	BYTE COMP4;
	BYTE COMP5;
	BYTE INDEX0;
	BYTE INDEX1;
	BYTE INDEX2;
	BYTE INDEX3;
	BYTE INDEX4;
	BYTE INDEX5;
} ENCINT_EX;

typedef struct _RIO_INT_EX
{
	BYTE SET0_DI0;
	BYTE SET0_DI1;
	BYTE SET0_DI2;
	BYTE SET0_DI3;
	BYTE SET0_FAIL;
	BYTE SET1_DI0;
	BYTE SET1_DI1;
	BYTE SET1_DI2;
	BYTE SET1_DI3;
	BYTE SET1_FAIL;
} RIOINT_EX;


//  Define ISR function type for each page
typedef void(_stdcall *DDAISR)(DDAINT*);
typedef void(_stdcall *ENCISR)(ENCINT*);
typedef void(_stdcall *PCLISR)(PCLINT*); 
typedef void(_stdcall *LIOISR)(LIOINT*);
typedef void(_stdcall *RIOISR)(RIOINT*);
typedef void(_stdcall *ADCISR)(ADCINT*);
typedef void(_stdcall *LIOISR_EX)(LIOINT_EX*);
typedef void(_stdcall *ENCISR_EX)(ENCINT_EX*);
typedef void(_stdcall *RIOISR_EX)(RIOINT_EX*);


_declspec(dllexport)void _stdcall EPCIO_GetVersion(char*);


//  Initial EPCIO
_declspec(dllexport)int  _stdcall EPCIO_GetCardNum();
_declspec(dllexport)void _stdcall EPCIO_SetISRFunction(RIOISR, RIOISR, ADCISR, LIOISR, PCLISR, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_Init(DDAISR, ENCISR, ENCISR, ENCISR, RIOISR, RIOISR, ADCISR, LIOISR, PCLISR, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO4000_Init(DDAISR, ENCISR, ENCISR, ENCISR, RIOISR, RIOISR, ADCISR, LIOISR, PCLISR, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO6000_Init(DDAISR, ENCISR, ENCISR, ENCISR, RIOISR, RIOISR, ADCISR, LIOISR, PCLISR, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO400_Init(WORD, WORD, DDAISR, ENCISR, ENCISR, ENCISR, RIOISR, RIOISR, ADCISR, LIOISR, PCLISR, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO600_Init(WORD, WORD, DDAISR, ENCISR, ENCISR, ENCISR, RIOISR, RIOISR, ADCISR, LIOISR, PCLISR, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_Close(WORD wCardIndex = 0);


//  Bus interface control functions
_declspec(dllexport)BOOL _stdcall EPCIO_SetActivePage(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_GetActivePage(WORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_GetIntSource(EPCIOINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ResetModule(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ResetModuleEx(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ResetIRQNo(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_SetIntPeriod(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_SetIRQNo(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_SetWaitState(WORD, WORD wCardIndex = 0);


//  DDA control functions
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_GetIntCondition(DDAINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_GetCurrentCmd(WORD, int*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_GetStockCount(WORD, WORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SendPulse(WORD, int, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetBitLength(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetClockDivider(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetMinStockNo(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetOutputFormat(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetPulseWidth(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_StartEngine(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_StopEngine(WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DDA_ShiftOutFIFOCmd(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_CheckFIFOEmpty(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_CheckFIFOFull(WORD, WORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DDA_GetTime(float*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetTime(float, WORD, WORD wCardIndex = 0);
_declspec(dllexport)float _stdcall EPCIO_DDA_AuxSetTime(float, WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableOutABSwap(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableOutABSwap(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableOutAInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableOutAInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableOutBInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableOutBInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableStockInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableStockInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableCycleInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableCycleInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableOutputChannel(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableOutputChannel(WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnableEmgcStop(WORD wCardIndex = 0); // Extender
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisableEmgcStop(WORD wCardIndex = 0);// Extender


//  DDA Extender control functions
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_ClearPulseCounter(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_ClearPulseDivider(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_DisablePulseCounter(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EnablePulseCounter(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_EraseFIFOCmd(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_GetOutputPulse(WORD, long*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DDA_SetPulseDivider(WORD, WORD, WORD wCardIndex = 0);


//  Encoder Counter functions
_declspec(dllexport)BOOL _stdcall EPCIO_ENC012_GetIntCondition(ENCINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC345_GetIntCondition(ENCINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC678_GetIntCondition(ENCINT*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ENC_ClearCounter(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_GetIndexStatus(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_GetLatchValue(WORD, long*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ENC_GetValue(WORD, long*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_SetCompValue(WORD, long, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_SetFilterClock(WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ENC_StartInput(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_StopInput(WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ENC_GetInputRate(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_SetInputRate(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_GetInputType(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_SetInputType(WORD, WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ENC_EnableInAInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_DisableInAInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_EnableInBInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_DisableInBInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_EnableInCInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_DisableInCInverse(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_EnableInABSwap(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_DisableInABSwap(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_SetTrigMode(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_SetTrigSource(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_EnableIndexInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_DisableIndexInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_EnableCompInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ENC_DisableCompInt(WORD, WORD wCardIndex = 0);


//  Remote Digital IO functions
_declspec(dllexport)BOOL _stdcall EPCIO_RIO0_GetIntCondition(RIOINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO1_GetIntCondition(RIOINT*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_RIO_GetInputValue(WORD, WORD, WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_GetMasterStatus(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_GetSlaveStatus(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_GetSlaveFail(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_GetTransStatus(WORD, WORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_RIO_SetClockDivider(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_SetIntType(WORD, WORD, WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_SetOutputValue(WORD, WORD, WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_SetTransError(WORD, WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_RIO_EnableInputInt(WORD, WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_DisableInputInt(WORD, WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_EnableTransInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_DisableTransInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_EnableSlaveControl(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_DisableSlaveControl(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_EnableSetControl(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_RIO_DisableSetControl(WORD, WORD wCardIndex = 0);


//  ADC IO control functions
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_GetIntCondition(ADCINT*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ADC_GetInput(WORD, float*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_GetWorkStatus(WORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetCompMask(WORD mask, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetCompType(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetClockDivider(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetConvMode(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetTagChannel(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetSingleChannel(WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ADC_StartConv(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_StopConv(WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ADC_GetCompValue(WORD, float*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetCompValue(WORD, float, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_GetConvType(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_SetConvType(WORD, WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_ADC_EnableCompInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_DisableCompInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_EnableTagInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_DisableTagInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_EnableConvInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_DisableConvInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_EnableConvChannel(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_ADC_DisableConvChannel(WORD, WORD wCardIndex = 0);


//  Local IO control functions
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetIntCondition(LIOINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetDFIInput(DWORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetLDIInput(DWORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_LIO_SetDFIIntType(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_SetLDIIntType(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_SetLDOOutput(DWORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetLDOOutput(DWORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_LIO_RefreshWDogTimer(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_SetTimer(DWORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_SetWDogTimer(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_SetWDogReset(DWORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnableLDIInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisableLDIInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnableDFIInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisableDFIInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnableTimer(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisableTimer(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnableTimerInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisableTimerInt(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnableWDogTimer(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisableWDogTimer(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnableLDOOutput(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisableLDOOutput(WORD, WORD wCardIndex = 0);


//  Define local I/O for dedicate input/Output
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_Get24VSensor(WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetEmgcStopStatus(WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetHomeSensor(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetOverTravelUp(WORD, WORD*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_GetOverTravelDown(WORD, WORD*, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_LIO_ServoOn(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_ServoOff(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnablePrdy(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisablePrdy(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_EnablePulseDAC(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_LIO_DisablePulseDAC(WORD wCardIndex = 0);


//  PCL control functions
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_ClearErrorCounter(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_GetIntCondition(PCLINT*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_GetErrorCounter(WORD, int*, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_SetScaleGain(WORD, WORD, int, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_PCL_EnableOverflowInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_DisableOverflowInt(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_EnableErrorCounter(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_DisableErrorCounter(WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_PCL_StartControl(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_PCL_StopControl(WORD wCardIndex = 0);


//  DAC IO control functions
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_SetClockDivider(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_SetCmdSource(WORD, WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_SetOutput(WORD, float, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_SetTrigOutput(WORD, float, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_SetTrigSource(WORD, DWORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DAC_EnableTrigMode(WORD, WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_DisableTrigMode(WORD, WORD wCardIndex = 0);

_declspec(dllexport)BOOL _stdcall EPCIO_DAC_StartConv(WORD wCardIndex = 0);
_declspec(dllexport)BOOL _stdcall EPCIO_DAC_StopConv(WORD wCardIndex = 0);

_declspec(dllexport)WORD  _stdcall EPCIO_GetCardCount(BYTE byCardType);
_declspec(dllexport)WORD  _stdcall EPCIO_GetCardType(WORD wCardIndex);
_declspec(dllexport)WORD  _stdcall EEPROM_ReadWord(WORD wCardIndex);


//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
typedef struct _CARD_INFO
{
	WORD wCardType;
	WORD wCardID;  
} CARD_INFO;

typedef struct _MOTION_CARD_INFO
{
	WORD      wCardNum;
	CARD_INFO stCardInfo[12];

} MOTION_CARD_INFO;


_declspec(dllexport)BOOL _stdcall EPCIO_GetCardInfo(MOTION_CARD_INFO *pstMotionCardInfo);

#ifdef __cplusplus
}
#endif

#endif

