using Cyaim.NFC.NFC750.Common;
using Cyaim.NFC.NFC750.ExternalAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Cyaim.NFC.NFC750
{
    /// <summary>
    /// NFC 750系列
    /// </summary>
    public class NFC750Helper
    {
        /// <summary>
        /// 实例化时将会初始化环境
        /// </summary>
        public NFC750Helper()
        {
            var path = Directory.GetCurrentDirectory() + "/hfrdapi.dll";
            var flag = File.Exists(path);
            if (!flag)
            {
                InitEnvironment(path);
            }
        }

        /// <summary>
        /// 释放依赖库文件
        /// </summary>
        /// <param name="path">文件名hfrdapi.dll，默认程序运行目录</param>
        public void InitEnvironment(string path = null)
        {
            try
            {
                byte[] data = null;
                if (string.IsNullOrEmpty(path))
                {
                    path = Directory.GetCurrentDirectory() + "/hfrdapi.dll";
                }

                if (Environment.Is64BitProcess)
                {
                    data = LibFile.hfrdapi_64;
                }
                else
                {
                    data = LibFile.hfrdapi_86;
                }
                File.WriteAllBytes(path, data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获取动态库链接库版本号
        /// </summary>
        /// <returns></returns>
        public uint GetLibVersion()
        {
            uint ver = 0;
            HfrdApi.Sys_GetLibVersion(ref ver);
            return ver;
        }

        /// <summary>
        /// 获取连接到PC的设备数
        /// </summary>
        /// <param name="vendorID">Vendor ID</param>
        /// <param name="productID">Product ID</param>
        /// <returns></returns>
        public uint GetDeviceNum(ushort vendorID = 0x0416, ushort productID = 0x8020)
        {
            uint dNum = 0;
            var r = HfrdApi.Sys_GetDeviceNum(vendorID, productID, ref dNum);
            if (r != 0)
            {
                throw new Exception("error code:" + r);
            }
            return dNum;
        }

        /// <summary>
        /// 获取设备序列号
        /// </summary>
        /// <param name="deviceIndex">设备索引</param>
        /// <param name="bufferSize">缓冲区大小</param>
        /// <param name="vendorID"></param>
        /// <param name="productID"></param>
        /// <returns></returns>
        public string GetDeviceSerialNumber(uint deviceIndex, uint bufferSize = 256, ushort vendorID = 0x0416, ushort productID = 0x8020)
        {
            StringBuilder r = new StringBuilder();
            r.Length = (int)bufferSize;
            var or = HfrdApi.Sys_GetHidSerialNumberStr(deviceIndex, vendorID, productID, r, bufferSize);
            if (or != 0)
            {
                return null;
            }
            return r.ToString();
        }

        /// <summary>
        /// 打开HID设备
        /// </summary>
        /// <param name="deviceType">设备标准</param>
        /// <param name="deviceIndex">设备索引</param>
        /// <param name="vendorID">Vendor ID</param>
        /// <param name="productID">Product ID</param>
        /// <returns></returns>
        public INFCDevice Open(DeviceType deviceType, uint deviceIndex, ushort vendorID = 0x0416, ushort productID = 0x8020)
        {
            IntPtr deviceID = (IntPtr)(-1);
            int or = HfrdApi.Sys_Open(ref deviceID, deviceIndex, vendorID, productID);
            Win32.Sleep(5);
            if (or != 0)
            {
                throw new InvalidOperationException("打开设备失败，错误代码：" + or);
            }

            switch (deviceType)
            {
                case DeviceType.ISO14443A:
                    throw new NotImplementedException();
                case DeviceType.ISO14443B:
                    throw new NotImplementedException();
                case DeviceType.ISO15693:
                    return new NFC750_ISO15693(deviceID);
                default:
                    throw new Exception("error code:" + or);
            }

        }

        /// <summary>
        /// 打开HID设备
        /// </summary>
        /// <param name="device">设备标准</param>
        /// <param name="deviceIndex">设备索引</param>
        /// <param name="vendorID">Vendor ID</param>
        /// <param name="productID">Product ID</param>
        /// <returns></returns>
        public int Open(INFCDevice device, uint deviceIndex, ushort vendorID = 0x0416, ushort productID = 0x8020)
        {
            IntPtr deviceID = (IntPtr)(-1);
            int or = HfrdApi.Sys_Open(ref deviceID, device.DeviceIndex, device.VendorID, device.ProductID);
            device.DeviceID = deviceID;

            return or;
        }

        /// <summary>
        /// 创建设备操作对象
        /// </summary>
        /// <param name="deviceType">设备标准</param>
        /// <param name="deviceIndex">设备索引</param>
        /// <param name="vendorID">Vendor ID</param>
        /// <param name="productID">Product ID</param>
        /// <returns></returns>
        public INFCDevice CreateNFCDevice(DeviceType deviceType)
        {
            switch (deviceType)
            {
                case DeviceType.ISO14443A:
                    throw new NotImplementedException();
                case DeviceType.ISO14443B:
                    throw new NotImplementedException();
                case DeviceType.ISO15693:
                    return new NFC750_ISO15693();
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        /// <summary>
        /// 设备是否打开
        /// ，“设备已经打开”并不代表设备与计算机处于正常连接状态，比如在设备打开成功后拔掉USB数据线，可以查询到设备仍然是“设备已经打开”状态，但实际上不是处于连接状态了，确认连接状态可通过调用GetDeviceInfo或GetDeviceAddress来判断
        /// </summary>
        /// <returns>
        /// true已打开，false未打开
        /// </returns>
        public bool IsOpen(INFCDevice device)
        {
            return HfrdApi.Sys_IsOpen(device.DeviceID);
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public bool Close(INFCDevice device)
        {
            var or = HfrdApi.Sys_Close(device.DeviceID);
            return or == 0;
        }

        /// <summary>
        /// 读取读写卡器型号信息
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        public string GetModel(INFCDevice device)
        {
            byte[] data = new byte[0];
            byte length = 0;
            HfrdApi.Sys_GetModel(device.DeviceID, data, ref length);

            data = new byte[length];
            var r = HfrdApi.Sys_GetModel(device.DeviceID, data, ref length);
            if (r != 0)
            {
                return null;
            }

            var s = Encoding.ASCII.GetString(data);
            return s;
        }

        /// <summary>
        /// 获取设备的产品序列号
        /// </summary>
        /// <param name="device"></param>
        /// <returns>返回读写卡器产品序列号,4 字节，高字节在前</returns>
        public byte GetSnr(INFCDevice device)
        {
            byte r = 0;
            HfrdApi.Sys_GetSnr(device.DeviceID, ref r);
            return r;
        }

        /// <summary>
        /// 设置指示灯颜色
        /// </summary>
        /// <param name="device"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public bool SetLight(INFCDevice device, DeviceLightColor color)
        {
            var r = HfrdApi.Sys_SetLight(device.DeviceID, (byte)color);
            return r == 0;
        }

        /// <summary>
        /// 蜂鸣
        /// </summary>
        /// <param name="device"></param>
        /// <param name="milliseconds"></param>
        /// <returns></returns>
        public bool PlayBeep(INFCDevice device, uint milliseconds)
        {
            var r = HfrdApi.Sys_SetBuzzer(device.DeviceID, (byte)(milliseconds / 10));
            return r == 0;
        }

        /// <summary>
        /// 设置读写器天线状态,在不需要操作卡时可以将天线关闭，以降低读写器的功耗；通过关闭、打开天线可实现卡片重新上电、复位；
        /// </summary>
        /// <param name="device"></param>
        /// <param name="isOpen"></param>
        /// <returns></returns>
        public bool SetAntenna(INFCDevice device, bool isOpen)
        {
            var r = HfrdApi.Sys_SetAntenna(device.DeviceID, (byte)(isOpen ? 1 : 0));
            return r == 0;
        }

        /// <summary>
        /// 设置读写器非接触工作方式。
        /// <para>只支持单一协议的读写器仅对部分方式有效</para>
        /// <para>
        /// type = 'A' -> 设置为ISO14443A方式，ISO14443A类别的函数使用此方式；
        /// type = 'B' -> 设置为ISO14443B方式，SGIDC(二代证)、ISO14443B-4类别的函数使用此方式；
        /// type = 'r' -> 设置为AT88RF020卡方式，AT88RF020类别的函数使用此方式；
        /// type = 's' -> 设置为ST卡方式，SR176、SRI4K类别的函数使用此方式；
        /// type = '1' -> 设置为ISO15693方式，ISO15693类别的函数使用此方式；
        /// </para>
        /// </summary>
        /// <param name="device"></param>
        /// <param name="type"></param>
        /// <returns>0成功，非0为错误代码</returns>
        public int SetInitType(INFCDevice device, char type)
        {
            var chr = Regex.IsMatch(type.ToString(), "(A|B|r|s|1){1}");
            if (!chr)
            {
                throw new ArgumentException("参数type只能为A、B、r、s、1，而接收到的参数是：" + type);
            }
            var r = HfrdApi.Sys_InitType(device.DeviceID, (byte)type);
            return r;
        }

        /// <summary>
        /// 复位
        /// </summary>
        /// <param name="device"></param>
        /// <param name="type"></param>
        /// <returns>0成功，非0为错误代码</returns>
        public int SetReset(INFCDevice device, DeviceResetType type)
        {
            var r = HfrdApi.Sys_InitType(device.DeviceID, (byte)type);
            return r;
        }


    }

    public enum DeviceType
    {
        ISO14443A = 0,
        ISO14443B = 1,
        ISO15693 = 2,
    }

    public enum DeviceLightColor
    {
        Extinguish = 0,
        Red = 1,
        Green = 2,
        Yellow = 3
    }

    public enum DeviceResetType
    {
        /// <summary>
        /// 读写器
        /// </summary>
        Reader = 0x01,
        /// <summary>
        /// 射频IC
        /// </summary>
        RFIC = 0x11
    }
}
