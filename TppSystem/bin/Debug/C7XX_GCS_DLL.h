/////////////////////////////////////////////////////////////////////////////
// This is a part of the PI-Software Sources
// Copyright (C) 1995-2003 Physik Instrumente (PI) GmbH & Co. KG
// All rights reserved.
//
// This source code belongs to the Dll for the C-880 system
//
// File: C880_dll.h
/////////////////////////////////////////////////////////////////////////////


#ifdef __USE_STDAFX_H
#include <stdafx.h>
#else
#include <windows.h>
#endif

#ifdef __cplusplus
extern "C" {
#endif

#undef C7XX_FUNC_DECL
#ifdef C7XX_DLL_EXPORTS
#define C7XX_FUNC_DECL __declspec(dllexport) WINAPI
#else
#define C7XX_FUNC_DECL __declspec(dllimport) WINAPI
#endif


/////////////////////////////////////////////////////////////////////////////
// DLL initialization and comm functions
int C7XX_FUNC_DECL C7XX_InterfaceSetupDlg(const char* szRegKeyName);
int C7XX_FUNC_DECL C7XX_ConnectRS232(const int nPortNr, long nBaudRate);
int C7XX_FUNC_DECL C7XX_ConnectTCPIP(const char* szHostname, int port);
BOOL C7XX_FUNC_DECL C7XX_IsConnected(int ID);
void C7XX_FUNC_DECL C7XX_CloseConnection(int ID);
int C7XX_FUNC_DECL C7XX_GetError(int ID);
BOOL C7XX_FUNC_DECL C7XX_SetErrorCheck(int ID, BOOL bErrorCheck);
BOOL C7XX_FUNC_DECL C7XX_TranslateError(int errNr, char* szBuffer, int maxlen);

/////////////////////////////////////////////////////////////////////////////
// special C880 functions
BOOL C7XX_FUNC_DECL C7XX_CLR(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_DEL(int ID, double dmSeconds);
BOOL C7XX_FUNC_DECL C7XX_GetSmallestStep(int ID, const char* szAxes, double* pdValarray);

BOOL C7XX_FUNC_DECL C7XX_SVO(int ID, const char* szAxes, const BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_qSVO(int ID, const char* szAxes, BOOL* pbValarray);

BOOL C7XX_FUNC_DECL C7XX_qTIM(int ID, int* pnTime);
BOOL C7XX_FUNC_DECL C7XX_qTAC(int ID, int* pnNr);

BOOL C7XX_FUNC_DECL C7XX_qSSNText(int ID, char* szBuffer, int maxlen);

BOOL C7XX_FUNC_DECL C7XX_IsReferenceOK(int ID, const char* szAxes, BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_SPA(int ID, const char* szAxes, const int* iCmdarray, const double* dValarray, const char* szStageName);
BOOL C7XX_FUNC_DECL C7XX_qSPA(int ID, const char* szAxes, const int* iCmdarray, double* dValarray, char* szStageName, int buflen);
BOOL C7XX_FUNC_DECL C7XX_ITD(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_RST(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_SAV(int ID, const char* szAxes);


BOOL C7XX_FUNC_DECL C7XX_SAI(int ID, const char* szOldAxes, const char* szNewAxes);
BOOL C7XX_FUNC_DECL C7XX_qSAI(int ID, char* szAxes, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_qSAI_ALL(int ID, char* szAxes, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_qTVI(int ID, char* szAxes, int maxlen);

BOOL C7XX_FUNC_DECL C7XX_CST(int ID, const char* szAxes, const char* names);
BOOL C7XX_FUNC_DECL C7XX_qCST(int ID, const char* szAxes, char* names, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_qVST(int ID, char* buffer, int maxlen);

BOOL C7XX_FUNC_DECL C7XX_SCA(int ID, char cAxisLeftRight, char cAxisUpDown); 
BOOL C7XX_FUNC_DECL C7XX_qSCA(int ID, char* pcAxisLeftRight, char* pcAxisUpDown); 


/////////////////////////////////////////////////////////////////////////////
// general
BOOL C7XX_FUNC_DECL C7XX_SMO(int ID, const char* szAxes, const int* pnValarray);
BOOL C7XX_FUNC_DECL C7XX_qSMO(int ID, const char* szAxes, int* pnValarray);

BOOL C7XX_FUNC_DECL C7XX_qVER(int ID, char* szBuffer, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_qIDN(int ID, char* szBuffer, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_qERR(int ID, int* pnError);
BOOL C7XX_FUNC_DECL C7XX_qHLP(int ID, char* szBuffer, int maxlen);

BOOL C7XX_FUNC_DECL C7XX_INI(int ID, const char* szAxes);

BOOL C7XX_FUNC_DECL C7XX_MOV(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qMOV(int ID, const char* szAxes, double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_MVR(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qPOS(int ID, const char* szAxes, double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qONT(int ID, const char* szAxes, BOOL* pbValarray);

BOOL C7XX_FUNC_DECL C7XX_MVS(int ID, const char* szAxes, const double* pdStep, const double* pdStepSize, const int* pnDelay);
BOOL C7XX_FUNC_DECL C7XX_qMVS(int ID, const char* szAxes, double* pdStep, double* pdStepSize, int* pnDelay);

BOOL C7XX_FUNC_DECL C7XX_VMO(int ID, const char* szAxes, const double* pdValarray, BOOL* pbMovePossible);
BOOL C7XX_FUNC_DECL C7XX_POS(int ID, const char* szAxes, const double* pdValarray);

BOOL C7XX_FUNC_DECL C7XX_HLT(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_STP(int ID);
BOOL C7XX_FUNC_DECL C7XX_EmergencyStop(int ID);
BOOL C7XX_FUNC_DECL C7XX_SystemAbort(int ID);

BOOL C7XX_FUNC_DECL C7XX_VEL(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qVEL(int ID, const char* szAxes, double* pdValarray);

BOOL C7XX_FUNC_DECL C7XX_HasPosChanged(int ID, const char* szAxes, BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_IsMoving(int ID, const char* szAxes, BOOL* pbValarray);

BOOL C7XX_FUNC_DECL C7XX_WAI(int ID, const char* szAxes);

BOOL C7XX_FUNC_DECL C7XX_IsIdle(int ID, BOOL* pbIdle);

/////////////////////////////////////////////////////////////////////////////
// display
BOOL C7XX_FUNC_DECL C7XX_CLS(int ID);
BOOL C7XX_FUNC_DECL C7XX_MSG(int ID, const char* szMessage);
BOOL C7XX_FUNC_DECL C7XX_DSP(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_qDSP(int ID, char* szBuffer, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_HID(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_qHID(int ID, char* szBuffer, int maxlen);

/////////////////////////////////////////////////////////////////////////////
// limits
BOOL C7XX_FUNC_DECL C7XX_MNL(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_MPL(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_REF(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_qREF(int ID, const char* szAxes, BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_qLIM(int ID, const char* szAxes, BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_GetRefResult(int ID, const char* szAxes, int* pnResult);
BOOL C7XX_FUNC_DECL C7XX_IsReferencing(int ID, const char* szAxes, BOOL* pbIsReferencing);

BOOL C7XX_FUNC_DECL C7XX_RON(int ID, const char* szAxes, const BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_qRON(int ID, const char* szAxes, BOOL* pbValarray);

BOOL C7XX_FUNC_DECL C7XX_NLM(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qNLM(int ID, const char* szAxes, double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_PLM(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qPLM(int ID, const char* szAxes, double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_SSL(int ID, const char* szAxes, const BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_qSSL(int ID, const char* szAxes, BOOL* pbValarray);

BOOL C7XX_FUNC_DECL C7XX_qTMN(int ID, const char* szAxes, double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qTMX(int ID, const char* szAxes, double* pdValarray);

BOOL C7XX_FUNC_DECL C7XX_DFF(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qDFF(int ID, const char* szAxes, double* pdValarray);

BOOL C7XX_FUNC_DECL C7XX_DFH(int ID, const char* szAxes);
BOOL C7XX_FUNC_DECL C7XX_qDFH(int ID, const char* szAxes, double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_GOH(int ID, const char* szAxes);

BOOL C7XX_FUNC_DECL C7XX_SST(int ID, const char* szAxes, const double* pdValarray);
BOOL C7XX_FUNC_DECL C7XX_qSST(int ID, const char* szAxes, double* pdValarray);

BOOL C7XX_FUNC_DECL C7XX_DRT(int ID, const int* piRecordChannelIds, const int* piTriggerSource, const char* szValues, int iArraySize);
BOOL C7XX_FUNC_DECL C7XX_qDRT(int ID, const int* piRecordChannelIds, int* piTriggerSource, char* szValues, int iArraySize, int iValueBufferLength);
BOOL C7XX_FUNC_DECL C7XX_DRC(int ID, const int* iRecTableId, const char* sRecSourceId, const int* iRecOption, const int* TriggerOption);
BOOL C7XX_FUNC_DECL C7XX_qDRC(int ID, const int* iRecTableId, char* sRecSourceId, int* iRecOption, int* TriggerOption, int iArraySize);
BOOL C7XX_FUNC_DECL C7XX_qDRR_SYNC(int ID, long iRecTableId, int iOffset, int nrValues, double* pdValArray);
BOOL C7XX_FUNC_DECL C7XX_qTNR(int ID, int* pNr);
BOOL C7XX_FUNC_DECL C7XX_qDRR(int ID, const int* piRecTableIds, int iNumberOfRecChannels, int iOffset, int nrValues, double** pdValArray, char* szGcsArrayHeader, int iGcsArrayHeaderMaxSize);
int C7XX_FUNC_DECL C7XX_GetAsyncBufferIndex(int ID);
BOOL C7XX_FUNC_DECL C7XX_GetAsyncBuffer(int ID, double** pdValArray);

/////////////////////////////////////////////////////////////////////////////
// macro commands
BOOL C7XX_FUNC_DECL C7XX_IsRecordingMacro(int ID, BOOL* pbRecordingMacro);
BOOL C7XX_FUNC_DECL C7XX_IsRunningMacro(int ID, BOOL* pbRunningMacro);

BOOL C7XX_FUNC_DECL C7XX_MAC_DEL(int ID, const char* szName);

BOOL C7XX_FUNC_DECL C7XX_MAC_START(int ID, const char* szName);
BOOL C7XX_FUNC_DECL C7XX_MAC_NSTART(int ID, const char* szName, int nrRuns);
BOOL C7XX_FUNC_DECL C7XX_qMAC(int ID, const char* szName, char* szBuffer, int maxlen);

BOOL C7XX_FUNC_DECL C7XX_MAC_BEG(int ID, const char* szName);
BOOL C7XX_FUNC_DECL C7XX_MAC_END(int ID);

BOOL C7XX_FUNC_DECL C7XX_SaveMacroToFile(int ID, const char* szFileName, const char* szMacroName);
BOOL C7XX_FUNC_DECL C7XX_LoadMacroFromFile(int ID, const char* szFileName, const char* szMacroName);

BOOL C7XX_FUNC_DECL C7XX_qTIO(int ID, int* pINr, int* pONr);
BOOL C7XX_FUNC_DECL C7XX_GetInputChannelNames(int ID, char* szBuffer, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_GetOutputChannelNames(int ID, char* szBuffer, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_DIO(int ID, const char* szChannels, const BOOL* pbValarray);
BOOL C7XX_FUNC_DECL C7XX_qDIO(int ID, const char* szChannels, BOOL* pbValarray);

BOOL C7XX_FUNC_DECL C7XX_GcsCommandset(int ID, const char* szCommand);
BOOL C7XX_FUNC_DECL C7XX_GcsGetAnswer(int ID, char* szAnswer, int bufsize);
BOOL C7XX_FUNC_DECL C7XX_GcsGetAnswerSize(int ID, int* iAnswerSize);


BOOL C7XX_FUNC_DECL C7XX_TRO(int ID, const int* iTriggerLines, const BOOL* pbValarray, int iArraySize);
BOOL C7XX_FUNC_DECL C7XX_qTRO(int ID, const int* iTriggerLines, BOOL* pbValarray, int iArraySize);
BOOL C7XX_FUNC_DECL C7XX_qCTO(int ID, const int* iTriggerLines, const int* pParamID, char* szBuffer, int iArraySize, int iBufferMaxlen);
BOOL C7XX_FUNC_DECL C7XX_CTO(int ID, const int* iTriggerLines, const int* pParamID, const char* szValues, int iArraySize);

/////////////////////////////////////////////////////////////////////////////

BOOL C7XX_FUNC_DECL C7XX_IFC(int ID, const char* szParams, const char* szValues);
BOOL C7XX_FUNC_DECL C7XX_qIFC(int ID, const char* szParams, char* szBuffer, int maxlen);
BOOL C7XX_FUNC_DECL C7XX_IFS(int ID, const char* szPassword, const char* szParams, const char* szValues);
BOOL C7XX_FUNC_DECL C7XX_qIFS(int ID, const char* szParams, char* szBuffer, int maxlen);

/////////////////////////////////////////////////////////////////////////////

BOOL C7XX_FUNC_DECL C7XX_WAA(int ID, unsigned int iWaitTime);
BOOL C7XX_FUNC_DECL C7XX_IsWaitingForAllAxes(int ID, BOOL* pbIsWaitingForAllAxes);
BOOL C7XX_FUNC_DECL C7XX_GetWaaResult(int ID, BOOL* pbWaaResult);


#ifdef __cplusplus
}
#endif