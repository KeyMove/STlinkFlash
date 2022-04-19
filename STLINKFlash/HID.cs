using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace STLINKFlash
{
    class HID
    {
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid interfaceClassGuid;
            public int flags;
            public int reserved;
        }

        [StructLayout(LayoutKind.Sequential,Pack = 1)]
        public class SP_DEVINFO_DATA
        {
            public int cbSize = Marshal.SizeOf(typeof(SP_DEVINFO_DATA));
            public Guid classGuid = Guid.Empty; // temp
            public int devInst = 0; // dumy
            public int reserved = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 2)]
        internal struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            internal int cbSize;
            internal short devicePath;
        }

        public enum DIGCF
        {
            DIGCF_DEFAULT = 0x1,
            DIGCF_PRESENT = 0x2,
            DIGCF_ALLCLASSES = 0x4,
            DIGCF_PROFILE = 0x8,
            DIGCF_DEVICEINTERFACE = 0x10
        }

        public struct HID_ATTRIBUTES
        {
            public int Size;
            public ushort VendorID;
            public ushort ProductID;
            public ushort VersionNumber;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HIDP_CAPS
        {
            public System.UInt16 Usage;                    // USHORT   
            public System.UInt16 UsagePage;                // USHORT   
            public System.UInt16 InputReportByteLength;
            public System.UInt16 OutputReportByteLength;
            public System.UInt16 FeatureReportByteLength;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 17)]
            public System.UInt16[] Reserved;                // USHORT  Reserved[17];               
            public System.UInt16 NumberLinkCollectionNodes;
            public System.UInt16 NumberInputButtonCaps;
            public System.UInt16 NumberInputValueCaps;
            public System.UInt16 NumberInputDataIndices;
            public System.UInt16 NumberOutputButtonCaps;
            public System.UInt16 NumberOutputValueCaps;
            public System.UInt16 NumberOutputDataIndices;
            public System.UInt16 NumberFeatureButtonCaps;
            public System.UInt16 NumberFeatureValueCaps;
            public System.UInt16 NumberFeatureDataIndices;
        }

        [DllImport("hid.dll")]
        public static extern void HidD_GetHidGuid(ref Guid HidGuid);

        [DllImport("setupapi.dll", SetLastError = true)]
        public static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, uint Enumerator, IntPtr HwndParent, DIGCF Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Boolean SetupDiEnumDeviceInterfaces(IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, UInt32 memberIndex, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData);

        [DllImport("setupapi.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool SetupDiGetDeviceInterfaceDetail(IntPtr deviceInfoSet, ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData, IntPtr deviceInterfaceDetailData,
             int deviceInterfaceDetailDataSize, ref int requiredSize, SP_DEVINFO_DATA deviceInfoData);

        [DllImport("setupapi.dll", SetLastError = true)]
        internal static extern IntPtr SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        //获取设备文件
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr CreateFile(
             string lpFileName,                            // file name
            uint dwDesiredAccess,                        // access mode
            uint dwShareMode,                            // share mode
            uint lpSecurityAttributes,                    // SD
            uint dwCreationDisposition,                    // how to create
            uint dwFlagsAndAttributes,                    // file attributes
            uint hTemplateFile                            // handle to template file
            );
        //读取设备文件
        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern bool ReadFile
             (
                 IntPtr hFile,
                 byte[] lpBuffer,
                 uint nNumberOfBytesToRead,
                 ref uint lpNumberOfBytesRead,
                 IntPtr lpOverlapped
             );

        //释放设备
        [DllImport("hid.dll")]
        static public extern bool HidD_FreePreparsedData(ref IntPtr PreparsedData);
        //关闭访问设备句柄，结束进程的时候把这个加上保险点
        [DllImport("kernel32.dll")]
        static public extern int CloseHandle(IntPtr hObject);

        //关闭访问设备句柄，结束进程的时候把这个加上保险点
        [DllImport("kernel32.dll")]
        static public extern long GetLastError();

        [DllImport("hid.dll")]
        private static extern Boolean HidD_GetAttributes(IntPtr hidDevice, out HID_ATTRIBUTES attributes);

        [DllImport("hid.dll")]
        private static extern Boolean HidD_GetPreparsedData(IntPtr hidDeviceObject, out IntPtr PreparsedData);

        [DllImport("hid.dll")]
        private static extern uint HidP_GetCaps(IntPtr PreparsedData, out HIDP_CAPS Capabilities);

        [DllImport("hid.dll")]
        private static extern Boolean HidD_FreePreparsedData(IntPtr PreparsedData);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_Initialize(IntPtr handle,ref IntPtr interface_handle);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_QueryDeviceInformation(IntPtr handle, uint InformationType,ref uint BufferLength,ref uint data);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_QueryInterfaceSettings(IntPtr handle, byte AlternateInterfaceNumber, ref ulong UsbAltInterfaceDescriptor);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_QueryPipe(IntPtr handle, byte AlternateInterfaceNumber,byte PipeIndex, ref ulong PipeInformation);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_ReadPipe(IntPtr handle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, uint ptr = 0);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_WritePipe(IntPtr handle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, uint ptr = 0);

        [StructLayout(LayoutKind.Explicit)]
        public class USB_INTERFACE_DESCRIPTOR
        {
            [FieldOffset(0)]
            public ulong ptr;
            [FieldOffset(8)]
            public ulong ptr2;
            [FieldOffset(0)]
            public byte bLength;[FieldOffset(1)]
            public byte bDescriptorType;[FieldOffset(2)]
            public byte bInterfaceNumber;[FieldOffset(3)]
            public byte bAlternateSetting;[FieldOffset(4)]
            public byte bNumEndpoints;[FieldOffset(5)]
            public byte bInterfaceClass;[FieldOffset(6)]
            public byte bInterfaceSubClass;[FieldOffset(7)]
            public byte bInterfaceProtocol;[FieldOffset(8)]
            public byte iInterface;
        }

        public enum USB_INTERFACE_DESCRIPTOR_TYPE
        {
            bLength=0,
            bDescriptorType,
            bInterfaceNumber,            bAlternateSetting,            bNumEndpoints,
            bInterfaceClass,
            bInterfaceSubClass,
            bInterfaceProtocol,
            iInterface,
        }

        public enum USBD_PIPE_TYPE
        {
            UsbdPipeTypeControl,
            UsbdPipeTypeIsochronous,
            UsbdPipeTypeBulk,
            UsbdPipeTypeInterrupt
        }

        [StructLayout(LayoutKind.Explicit)]
        public class WINUSB_PIPE_INFORMATION
        {
            [FieldOffset(0)]
            public ulong ptr;
            [FieldOffset(0)]
            public USBD_PIPE_TYPE PipeType;
            [FieldOffset(4)]
            public byte PipeId;
            [FieldOffset(5)]
            public ushort MaximumPacketSize;
            [FieldOffset(7)]
            public byte Interval;
        }



        IntPtr hDevInfo;
        IntPtr HidHandle = IntPtr.Zero;
        Guid guidHID = Guid.Empty;

        int OutputLength = 0;
        int InputLength = 0;

        FileStream HidStream;

        IntPtr FAIL = new IntPtr(-1);
        private IntPtr winusb;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const int OPEN_EXISTING = 3;

        public const int WM_DEVICECHANGE = 0x0219;
        public const int DEVICE_ARRIVAL = 0x8000;
        public const int DEVICE_REMOVECOMPLETE = 0x8004;

        public int CheckMessage(Message m)
        {
            if (m.Msg == WM_DEVICECHANGE)
            {
                int pid = m.WParam.ToInt32();
                int vid = m.LParam.ToInt32();
                switch (m.WParam.ToInt32())
                {
                    case DEVICE_ARRIVAL:
                        return 2;
                    case DEVICE_REMOVECOMPLETE:
                        return 3;
                }
                return 1;
            }
            return 0;
        }

        public List<string> getHandle(int vid, int pid)
        {
            guidHID = Guid.Parse("{DBCE1CD9-A320-4b51-A365-A0C3F3C5FB29}");
            hDevInfo = SetupDiGetClassDevs(ref guidHID, 0, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);
            var err = GetLastError();
            SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
            DeviceInterfaceData.cbSize = Marshal.SizeOf(DeviceInterfaceData);
            SP_DEVINFO_DATA strtInterfaceData = new SP_DEVINFO_DATA();
            List<string> HIDUSBAddress = new List<string>();
            int bufferSize = 0;
            for (int i = 0; i < 64; i++)
            {

                if (!SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guidHID, (UInt32)i, ref DeviceInterfaceData))
                {
                    err=GetLastError();
                    continue;
                }
                bufferSize = 0;
                SetupDiGetDeviceInterfaceDetail(hDevInfo, ref DeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, strtInterfaceData);
                IntPtr detailDataBuffer = Marshal.AllocHGlobal(bufferSize);
                SP_DEVICE_INTERFACE_DETAIL_DATA detailData = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                detailData.cbSize = Marshal.SizeOf(typeof(SP_DEVICE_INTERFACE_DETAIL_DATA));
                Marshal.StructureToPtr(detailData, detailDataBuffer, false);
                if (SetupDiGetDeviceInterfaceDetail(hDevInfo, ref DeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, strtInterfaceData))
                {
                    string deviceVIDPID = string.Format("vid_{0:D4}&pid_{1:D4}", vid, pid);
                    IntPtr pdevicePathName = (IntPtr)((int)detailDataBuffer + 4);
                    string devicePathName = Marshal.PtrToStringAuto(pdevicePathName);
                    if (devicePathName.Contains(deviceVIDPID))
                    {
                        HIDUSBAddress.Add(devicePathName);
                    }
                    Marshal.FreeHGlobal(detailDataBuffer);
                }
            }
            SetupDiDestroyDeviceInfoList(hDevInfo);
            return HIDUSBAddress;
            //while (true)
            //{
            //获取设备，true获取到

            //for (int i = 0; i < 3; i++)
            //{
            //bool result = SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guidHID, (UInt32)index, ref DeviceInterfaceData);
            //}
            //第一次调用出错，但可以返回正确的Size 

            //result = SetupDiGetDeviceInterfaceDetail(hDevInfo, ref DeviceInterfaceData, IntPtr.Zero, 0, ref bufferSize, strtInterfaceData);
            //第二次调用传递返回值，调用即可成功



            //result = SetupDiGetDeviceInterfaceDetail(hDevInfo, ref DeviceInterfaceData, detailDataBuffer, bufferSize, ref bufferSize, strtInterfaceData);
            //if (result == false)
            //{
            //break;
            //}
            //获取设备路径访


            //HIDUSBAddress.Add(devicePathName);
            //index++;
            //break;
            //}

            //连接设备文件
            //int aa = CT_CreateFile(devicePathName);
            //bool bb = USBDataRead(HidHandle);
        }

        //建立和设备的连接
        public bool OpenUSBHID(string DeviceName)
        {
            HidHandle = CreateFile(
                DeviceName,
                GENERIC_READ | GENERIC_WRITE,// | GENERIC_WRITE,//读写，或者一起
               FILE_SHARE_READ | FILE_SHARE_WRITE,// | FILE_SHARE_WRITE,//共享读写，或者一起
               0,
                OPEN_EXISTING,
                0,
                0);
            if (HidHandle == FAIL) return false;
            HID_ATTRIBUTES att;
            IntPtr p;
            HIDP_CAPS caps;
            HidD_GetAttributes(HidHandle, out att);
            HidD_GetPreparsedData(HidHandle, out p);
            HidP_GetCaps(p, out caps);
            HidD_FreePreparsedData(p);
            OutputLength = caps.OutputReportByteLength;
            InputLength = caps.InputReportByteLength;
            HidStream = new FileStream(new SafeFileHandle(HidHandle, false), FileAccess.ReadWrite, InputLength, false);
            return true;
        }

        public bool openwinusb(string devicename)
        {
            bool ret;
            HidHandle = CreateFile(
                devicename,
                GENERIC_READ | GENERIC_WRITE,// | GENERIC_WRITE,//读写，或者一起
               FILE_SHARE_READ | FILE_SHARE_WRITE,// | FILE_SHARE_WRITE,//共享读写，或者一起
               0,
                OPEN_EXISTING,
                0x40000000,
                0);
            ret = WinUsb_Initialize(HidHandle, ref winusb);
            if (!ret) { var err = GetLastError(); return ret; }
            uint len = 1;
            uint data=0;
            ret = WinUsb_QueryDeviceInformation(winusb, 1, ref len,ref data);
            if (!ret) return ret;
            USB_INTERFACE_DESCRIPTOR s = new USB_INTERFACE_DESCRIPTOR();
            byte[] dt = new byte[9];
            ret = WinUsb_QueryInterfaceSettings(winusb, 0, ref s.ptr);
            if (!ret) return ret;
            
            for(int i = 0; i < s.bNumEndpoints; i++)
            {
                WINUSB_PIPE_INFORMATION info = new WINUSB_PIPE_INFORMATION();
                ulong x = 0;
                ret = WinUsb_QueryPipe(winusb, 0, (byte)i, ref info.ptr);
                if (ret)
                {

                    if (info.PipeType == USBD_PIPE_TYPE.UsbdPipeTypeControl)
                    {

                    }
                }

            }
            byte ibulk = 0x81, obulk = 2;
            byte[] cmd = new byte[16];
            byte[] recv = new byte[16];
            cmd[0] = 0xf1;
            cmd[1] = 0x80;
            WinUsb_WritePipe(winusb, obulk, cmd, (uint)cmd.Length, ref len, 0);
            WinUsb_ReadPipe(winusb, ibulk, recv, 6, ref len, 0);
            //return ret;
            var stlink = (recv[0] & 0xf0) >> 4;
            var jtag= ((recv[0] & 0x0f) << 2) | ((recv[1] & 0xc0) >> 6);
            var swim= recv[1] & 0x3f;
            return ret;
        }



        public bool OpenUSBHID(int vid = 2211, int pid = 4433)
        {
            var list = getHandle(vid, pid);
            if (list.Count != 0)
                return openwinusb(list[0]);
            return false;
        }

        public bool OpenUSBHID()
        {
            return OpenUSBHID(2211, 4433);
        }

        public void Close()
        {
            if (HidHandle != FAIL)
            {
                CloseHandle(HidHandle);
            }
        }

        public void WriteData(byte[] data)
        {
            if (HidStream == null) return;
            HidStream.Write(data, 0, data.Length);
        }


        //根据CreateFile拿到的设备handle访问文件，并返回数据
        public bool USBDataRead(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
                handle = HidHandle;
            while (true)
            {
                uint read = 0;
                //注意字节的长度，我这里写的是8位，其实可以通过API获取具体的长度，这样安全点，
                //具体方法我知道，但是没有写，过几天整理完代码，一起给出来
                Byte[] m_rd_data = new Byte[8];
                bool isread = ReadFile(handle, m_rd_data, (uint)8, ref read, IntPtr.Zero);
                //这里已经是拿到的数据了
                Byte[] m_rd_dataout = new Byte[read];
                Array.Copy(m_rd_data, m_rd_dataout, read);
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
