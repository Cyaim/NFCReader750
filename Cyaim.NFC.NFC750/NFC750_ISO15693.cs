using Cyaim.NFC.NFC750.ExternalAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cyaim.NFC.NFC750
{
    public class NFC750_ISO15693 : INFCDevice
    {
        public IntPtr DeviceID { get; set; } = (IntPtr)(-1);

        public byte[] UID { get; private set; } = new byte[8];

        public byte DSFID { get; set; }

        public string UID_Str
        {
            get
            {
                return HfrdApi.ToHexString(UID, 8);
            }
        }

        /// <summary>
        /// 设备是否打开
        /// </summary>
        public bool HasDeviceOpen
        {
            get
            {
                try
                {
                    var r = HfrdApi.Sys_GetDeviceAddr(DeviceID, ref deviceAddress);
                    return r == 0 && deviceAddress != 0;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private uint deviceAddress;

        /// <summary>
        /// 设备地址
        /// </summary>
        public uint DeviceAddress
        {
            get
            {
                var flag = HasDeviceOpen;
                if (flag)
                {
                    return deviceAddress;
                }
                return 0;
            }
        }

        /// <summary>
        /// 读卡器型号
        /// </summary>
        public string DeviceModel
        {
            get
            {
                byte[] data = new byte[0];
                byte len = 0;
                HfrdApi.Sys_GetModel(this.DeviceID, data, ref len);

                data = new byte[len];
                var r = HfrdApi.Sys_GetModel(this.DeviceID, data, ref len);
                if (r != 0)
                {
                    return null;
                }

                return Encoding.ASCII.GetString(data);
            }
        }

        /// <summary>
        /// 设备索引
        /// </summary>
        public uint DeviceIndex { get; set; }

        public ushort VendorID { get; set; }

        public ushort ProductID { get; set; }

        public NFC750_ISO15693()
        {
        }

        public NFC750_ISO15693(IntPtr deviceID)
        {
            this.DeviceID = deviceID;
        }

        /// <summary>
        /// 寻找符合ISO15693标准的卡(1 slot方式)
        /// </summary>
        /// <param name="autoLength">是否自动确定数据长度</param>
        /// <returns>1字节DSFID+8字节UID</returns>
        public int Inventory(bool autoLength = false)
        {
            try
            {
                byte len = 0;
                byte[] data = null;
                int or = -1;
                Win32.Sleep(5);

                if (autoLength)
                {
                    data = new byte[0];
                    HfrdApi.I15693_Inventory(this.DeviceID, data, ref len);
                    data = new byte[len];
                    or = HfrdApi.I15693_Inventory(this.DeviceID, data, ref len);

                    goto Succ;
                }
                data = new byte[9];
                or = HfrdApi.I15693_Inventory(this.DeviceID, data, ref len);

            Succ:
                {
                    if (or != 0)
                    {
                        return or;
                    }
                    DSFID = data[0];
                    for (int i = 0, j = 1; j < data.Length; i++, j++)
                    {
                        UID[i] = data[j];
                    }

                    return or;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 读ISO15693卡的块内容
        /// </summary>
        /// <param name="readMode">
        /// Select_flag、Addres_flag位不能同时设为1；
        /// 若Select_flag = 1，只有处于SELECT状态的卡执行该命令；
        /// 若Addres_flag = 1，只有UID符合的卡执行该命令；
        /// 若Option_flag = 0，pData数据格式为：4字节Data，根据需要重复；
        /// 若Option_flag = 1，pData数据格式为：1字节Block security status + 4字节Data，根据需要重复；
        /// </param>
        /// <param name="uid">8字节UID</param>
        /// <param name="blockID">块号</param>
        /// <param name="autoLength">自动获取数据块长度</param>
        /// <param name="readNum">读取的块数</param>
        /// <returns></returns>
        public (byte[] Data, byte[] Status) Read(ISO15693_Mode readMode, string uid, uint blockID, bool autoLength = false, ushort readNum = 1)
        {
            try
            {
                byte[] uids = HfrdApi.ToDigitsBytes(uid);
                var or = Read(readMode, uids, blockID, autoLength, readNum);
                return (or.Data, or.Status);
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 读ISO15693卡的块内容
        /// </summary>
        /// <param name="readMode">
        /// Select_flag、Addres_flag位不能同时设为1；
        /// 若Select_flag = 1，只有处于SELECT状态的卡执行该命令；
        /// 若Addres_flag = 1，只有UID符合的卡执行该命令；
        /// 若Option_flag = 0，pData数据格式为：4字节Data，根据需要重复；
        /// 若Option_flag = 1，pData数据格式为：1字节Block security status + 4字节Data，根据需要重复；
        /// </param>
        /// <param name="uid">8字节UID</param>
        /// <param name="blockID">块号</param>
        /// <param name="autoLength">自动获取数据块长度</param>
        /// <param name="readNum">读取的块数</param>
        /// <returns></returns>
        public (byte[] Data, byte[] Status) Read(ISO15693_Mode readMode, byte[] uid, uint blockID, bool autoLength = false, ushort readNum = 1)
        {
            try
            {
                if (readNum > 9)
                {
                    throw new ArgumentOutOfRangeException("读取的块数不能大于9");
                }
                byte[] reDate = new byte[4];
                byte len = 0;

                if (autoLength)
                {
                    HfrdApi.I15693_Read(this.DeviceID, (byte)readMode, uid, (byte)blockID, (byte)readNum, reDate, ref len);
                    if (len == reDate.Length)
                    {
                        goto Succ;
                    }
                    reDate = new byte[len];
                    var ores = HfrdApi.I15693_Read(this.DeviceID, (byte)readMode, uid, (byte)blockID, (byte)readNum, reDate, ref len);
                    if (ores != 0)
                    {
                        goto Err;
                    }
                    goto Succ;
                }

                var or = HfrdApi.I15693_Read(this.DeviceID, (byte)readMode, uid, (byte)blockID, (byte)readNum, reDate, ref len);

                if (or != 0)
                {
                    goto Err;
                }

            Succ:
                {
                    byte[] status = new byte[1];
                    HfrdApi.I15693_GetBlockSecurity(this.DeviceID, (byte)readMode, uid, (byte)blockID, (byte)readNum, status, ref len);
                    return (reDate, status);
                }
            }
            catch (Exception)
            {

                throw;
            }

        Err: return (null, null);
        }

        /// <summary>
        /// 读ISO15693卡的块内容
        /// </summary>
        /// <param name="readMode">
        /// Select_flag、Addres_flag位不能同时设为1；
        /// 若Select_flag = 1，只有处于SELECT状态的卡执行该命令；
        /// 若Addres_flag = 1，只有UID符合的卡执行该命令；
        /// 若Option_flag = 0，pData数据格式为：4字节Data，根据需要重复；
        /// 若Option_flag = 1，pData数据格式为：1字节Block security status + 4字节Data，根据需要重复；
        /// </param>
        /// <param name="uid">8字节UID</param>
        /// <param name="blockID">块号</param>
        /// <param name="autoLength">自动获取数据块长度</param>
        /// <param name="readNum">读取的块数</param>
        /// <returns></returns>
        public (string Data, string Status) ReadToString(ISO15693_Mode readMode, string uid, uint blockID, bool autoLength = false, ushort readNum = 1)
        {
            var or = Read(readMode, uid, blockID, autoLength, readNum);

            return (HfrdApi.ToHexString(or.Data), HfrdApi.ToHexString(or.Status));
        }

        /// <summary>
        /// 读ISO15693卡的块内容
        /// </summary>
        /// <param name="readMode">
        /// Select_flag、Addres_flag位不能同时设为1；
        /// 若Select_flag = 1，只有处于SELECT状态的卡执行该命令；
        /// 若Addres_flag = 1，只有UID符合的卡执行该命令；
        /// 若Option_flag = 0，pData数据格式为：4字节Data，根据需要重复；
        /// 若Option_flag = 1，pData数据格式为：1字节Block security status + 4字节Data，根据需要重复；
        /// </param>
        /// <param name="uid">8字节UID</param>
        /// <param name="blockID">块号</param>
        /// <param name="autoLength">自动获取数据块长度</param>
        /// <param name="readNum">读取的块数</param>
        /// <returns></returns>
        public (string Data, string Status) ReadToString(ISO15693_Mode readMode, byte[] uid, uint blockID, bool autoLength = false, ushort readNum = 1)
        {
            var or = Read(readMode, uid, blockID, autoLength, readNum);

            return (HfrdApi.ToHexString(or.Data), HfrdApi.ToHexString(or.Status));
        }

        /// <summary>
        /// 写1块内容到ISO15693卡
        /// </summary>
        /// <param name="writeMode">操作类型</param>
        /// <param name="uid">十六进制UID</param>
        /// <param name="blockID">区块号</param>
        /// <param name="data">最多写入6位字符</param>
        /// <returns></returns>
        public int Write(ISO15693_Mode writeMode, string uid, uint blockID, string data)
        {
            var dataLength = data.Length;
            if (dataLength > 8)
            {
                throw new ArgumentOutOfRangeException($"最多写入8位字符，但是欲写入{dataLength}位字符");
            }
            byte[] uids = HfrdApi.ToDigitsBytes(uid);
            byte[] tempData = HfrdApi.ToDigitsBytes(data);

            var or = Write(writeMode, uids, blockID, tempData);
            return or;
        }

        /// <summary>
        /// 写1块内容到ISO15693卡
        /// </summary>
        /// <param name="writeMode">操作类型</param>
        /// <param name="uid">十六进制UID</param>
        /// <param name="blockID">区块号</param>
        /// <param name="data">最多写入4字节</param>
        /// <returns></returns>
        public int Write(ISO15693_Mode writeMode, byte[] uid, uint blockID, byte[] data)
        {
            var dataLength = data.Length;
            if (dataLength > 4)
            {
                throw new ArgumentOutOfRangeException($"最多写入4位字符，但是欲写入{dataLength}位字符");
            }
            var or = HfrdApi.I15693_Write(this.DeviceID, (byte)writeMode, uid, (byte)blockID, data);

            return or;
        }
    }

    public enum ISO15693_Mode
    {
        Select = 0x00,
        Addres = 0x01,
        Option = 0x02
    }
}
