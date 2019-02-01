using System;
using System.Collections.Generic;
using System.Text;

namespace Cyaim.NFC.NFC750
{
    public interface INFCDevice
    {
        IntPtr DeviceID { get; set; }

        byte DSFID { get; set; }
        byte[] UID { get; }
        string UID_Str { get; }

        bool HasDeviceOpen { get; }

        uint DeviceAddress { get; }

        string DeviceModel { get; }

        uint DeviceIndex { get; set; }

        ushort VendorID { get; set; }

        ushort ProductID { get; set; }

        int Inventory(bool autoLength = false);
        (byte[] Data, byte[] Status) Read(ISO15693_Mode readMode, byte[] uid, uint blockID, bool autoLength = false, ushort readNum = 1);
        (byte[] Data, byte[] Status) Read(ISO15693_Mode readMode, string uid, uint blockID, bool autoLength = false, ushort readNum = 1);
        (string Data, string Status) ReadToString(ISO15693_Mode readMode, byte[] uid, uint blockID, bool autoLength = false, ushort readNum = 1);
        (string Data, string Status) ReadToString(ISO15693_Mode readMode, string uid, uint blockID, bool autoLength = false, ushort readNum = 1);
        int Write(ISO15693_Mode writeMode, byte[] uid, uint blockID, byte[] data);
        int Write(ISO15693_Mode writeMode, string uid, uint blockID, string data);
    }
}
