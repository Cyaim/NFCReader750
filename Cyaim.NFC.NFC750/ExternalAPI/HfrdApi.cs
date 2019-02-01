using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Cyaim.NFC.NFC750.ExternalAPI
{
    public class HfrdApi
    {
        #region 系统函数
        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetLibVersion(ref uint pVer);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetDeviceNum(ushort vid, ushort pid, ref uint pNum);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetHidSerialNumberStr(UInt32 deviceIndex,
                                                    UInt16 vid,
                                                    UInt16 pid,
                                                    [Out]StringBuilder deviceString,
                                                    UInt32 deviceStringLength);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_Open(ref IntPtr device,
                                   UInt32 index,
                                   UInt16 vid,
                                   UInt16 pid);

        [DllImport("hfrdapi.dll")]
        public static extern bool Sys_IsOpen(IntPtr device);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_Close(IntPtr device);


        [DllImport("hfrdapi.dll")]
        public static extern int Sys_SetTimeouts(IntPtr device, uint getReportTimeout, uint setReportTimeout);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetTimeouts(IntPtr device, ref uint getReportTimeout, ref uint setReportTimeout);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_SetDllDeviceAddr(uint deviceAddr);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetDllDeviceAddr(uint deviceAddr);


        [DllImport("hfrdapi.dll")]
        public static extern int Sys_SetDeviceAddr(IntPtr device, uint deviceAddr);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetDeviceAddr(IntPtr device, ref uint deviceAddr);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetModel(IntPtr device, byte[] data, ref byte length);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_GetSnr(IntPtr device, ref byte snr);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_SetLight(IntPtr device, byte color);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_SetBuzzer(IntPtr device, byte msec);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_SetAntenna(IntPtr device, byte mode);

        [DllImport("hfrdapi.dll")]
        public static extern int Sys_InitType(IntPtr device, byte type);


        [DllImport("hfrdapi.dll")]
        public static extern int Sys_Reset(IntPtr device, byte type);
        #endregion

        #region 辅助函数
        [DllImport("hfrdapi.dll")]
        public static extern int Aux_SingleDES(byte desType,
                                       byte[] key,
                                       byte[] srcData,
                                       UInt32 srcDataLen,
                                       byte[] destData,
                                       ref UInt32 destDataLen);

        [DllImport("hfrdapi.dll")]
        public static extern int Aux_TripleDES(byte desType,
                                        byte[] key,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] destData,
                                        ref UInt32 destDataLen);

        [DllImport("hfrdapi.dll")]
        public static extern int Aux_SingleMAC(byte[] key,
                                        byte[] initData,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] macData);

        [DllImport("hfrdapi.dll")]
        public static extern int Aux_TripleMAC(byte[] key,
                                        byte[] initData,
                                        byte[] srcData,
                                        UInt32 srcDataLen,
                                        byte[] macData);
        #endregion


        //=========================== M1 Card Function =============================
        [DllImport("hfrdapi.dll")]
        public static extern int TyA_Request(IntPtr device, byte mode, ref UInt16 pTagType);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_Anticollision(IntPtr device,
                                            byte bcnt,
                                            byte[] pSnr,
                                            ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_Select(IntPtr device,
                                     byte[] pSnr,
                                     byte snrLen,
                                     ref byte pSak);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_Halt(IntPtr device);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Authentication2(IntPtr device,
                                                 byte mode,
                                                 byte block,
                                                 byte[] pKey);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Read(IntPtr device,
                                      byte block,
                                      byte[] pData,
                                      ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Write(IntPtr device, byte block, byte[] pData);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_InitValue(IntPtr device, byte block, Int32 value);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_ReadValue(IntPtr device, byte block, ref Int32 pValue);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Decrement(IntPtr device, byte block, Int32 value);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Increment(IntPtr device, byte block, Int32 value);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Restore(IntPtr device, byte block);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CS_Transfer(IntPtr device, byte block);

        //======================= Ultralight(C) Card Function ====================== 
        [DllImport("hfrdapi.dll")]
        public static extern int TyA_UL_Select(IntPtr device, byte[] pSnr, ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_UL_Write(IntPtr device, byte page, byte[] pdata);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_UL_Authentication(IntPtr device, byte[] pKey);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_UL_ChangeKey(IntPtr device, byte[] pKey);


        //========================= ISO15693 Card Function =========================
        [DllImport("hfrdapi.dll")]
        public static extern int I15693_Inventory(IntPtr device, byte[] pData, ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_Inventorys(IntPtr device, byte[] pData, ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_GetSystemInformation(IntPtr device,
                                                      byte mode,
                                                      byte[] pUID,
                                                      byte[] pData,
                                                      ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_Select(IntPtr device, byte[] pUID);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_ResetToReady(IntPtr device, byte mode, byte[] pUID);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_StayQuiet(IntPtr device, byte[] pUID);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_GetBlockSecurity(IntPtr device,
                                                  byte mode,
                                                  byte[] pUID,
                                                  byte block,
                                                  byte number,
                                                  byte[] pData,
                                                  ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_Read(IntPtr device,
                                      byte mode,
                                      byte[] pUID,
                                      byte block,
                                      byte number,
                                      byte[] pData,
                                      ref byte pLen);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_Write(IntPtr device,
                                       byte mode,
                                       byte[] pUID,
                                       byte block,
                                       byte[] pData);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_LockBlock(IntPtr device,
                                           byte mode,
                                           byte[] pUID,
                                           byte block);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_WriteAFI(IntPtr device,
                                          byte mode,
                                          byte[] pUID,
                                          byte AFI);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_LockAFI(IntPtr device, byte mode, byte[] pUID);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_WriteDSFID(IntPtr device,
                                            byte mode,
                                            byte[] pUID,
                                            byte DSFID);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_LockDSFID(IntPtr device, byte mode, byte[] pUID);

        [DllImport("hfrdapi.dll")]
        public static extern int I15693_DirectTransmit(IntPtr device,
                                                byte[] pCommand,
                                                byte cmdLen,
                                                byte[] pData,
                                                ref byte pMsgLg);


        //======================== ISO14443A-4 Card Function =======================
        [DllImport("hfrdapi.dll")]
        public static extern int TyA_Reset(IntPtr device,
                                    byte mode,
                                    byte[] pData,
                                    ref byte pMsgLg);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_CosCommand(IntPtr device,
                                         byte[] pCommand,
                                         byte cmdLen,
                                         byte[] pData,
                                         ref byte pMsgLg);

        [DllImport("hfrdapi.dll")]
        public static extern int TyA_Deselect(IntPtr device);

        //==========================================================================

        public static readonly char[] hexDigits = {
            '0','1','2','3','4','5','6','7',
            '8','9','A','B','C','D','E','F'};

        public static byte GetHexBitsValue(byte ch)
        {
            byte sz = 0;
            if (ch <= '9' && ch >= '0')
                sz = (byte)(ch - 0x30);
            if (ch <= 'F' && ch >= 'A')
                sz = (byte)(ch - 0x37);
            if (ch <= 'f' && ch >= 'a')
                sz = (byte)(ch - 0x57);

            return sz;
        }

        /// <summary>
        /// 单个字节转字字符.
        /// </summary>
        /// <param name="ib">字节.</param>
        /// <returns>转换好的字符.</returns>
        public static string ByteHex(Byte ib)
        {
            string _str = string.Empty;
            try
            {
                //char[] Digit = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A',
                //'B', 'C', 'D', 'E', 'F' };
                char[] ob = new char[2];
                ob[0] = hexDigits[(ib >> 4) & 0X0F];
                ob[1] = hexDigits[ib & 0X0F];
                _str = new String(ob);
            }
            catch (Exception)
            {
                new Exception("byteHEX error !");
            }
            return _str;

        }

        public static string ToHexString(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            String hexString = String.Empty;
            for (int i = 0; i < bytes.Length; i++)
                hexString += ByteHex(bytes[i]);

            return hexString;
        }

        public static string ToHexString(byte[] bytes, byte len)
        {
            String hexString = String.Empty;
            for (int i = 0; i < len; i++)
                hexString += ByteHex(bytes[i]);

            return hexString;
        }

        public static byte[] ToDigitsBytes(string theHex)
        {
            byte[] bytes = new byte[theHex.Length / 2 + (((theHex.Length % 2) > 0) ? 1 : 0)];
            for (int i = 0; i < bytes.Length; i++)
            {
                char lowbits = theHex[i * 2];
                char highbits;

                if ((i * 2 + 1) < theHex.Length)
                    highbits = theHex[i * 2 + 1];
                else
                    highbits = '0';

                int a = (int)GetHexBitsValue((byte)lowbits);
                int b = (int)GetHexBitsValue((byte)highbits);
                bytes[i] = (byte)((a << 4) + b);
            }

            return bytes;
        }
    }
}
