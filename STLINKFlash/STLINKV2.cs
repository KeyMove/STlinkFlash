using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace STLINKFlash
{
    class STLINKV2
    {
        #region constvalue

        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint FILE_SHARE_READ = 0x00000001;
        public const uint FILE_SHARE_WRITE = 0x00000002;
        public const int OPEN_EXISTING = 3;

        public const int WM_DEVICECHANGE = 0x0219;
        public const int DEVICE_ARRIVAL = 0x8000;
        public const int DEVICE_REMOVECOMPLETE = 0x8004;

        public enum SWDCLK : int
        {
            STLINK_SWDCLK_4MHZ_DIVISOR = 0,
            STLINK_SWDCLK_1P8MHZ_DIVISOR = 1,
            STLINK_SWDCLK_1P2MHZ_DIVISOR = 2,
            STLINK_SWDCLK_950KHZ_DIVISOR = 3,
            STLINK_SWDCLK_480KHZ_DIVISOR = 7,
            STLINK_SWDCLK_240KHZ_DIVISOR = 15,
            STLINK_SWDCLK_125KHZ_DIVISOR = 31,
            STLINK_SWDCLK_100KHZ_DIVISOR = 40,
            STLINK_SWDCLK_50KHZ_DIVISOR = 79,
            STLINK_SWDCLK_25KHZ_DIVISOR = 158,
            STLINK_SWDCLK_15KHZ_DIVISOR = 265,
            STLINK_SWDCLK_5KHZ_DIVISOR = 798,
        }

        const ushort USB_ST_VID = 0x0483; /**< USB Vid */
        const ushort USB_STLINK_PID = 0x3744; /**< USB Pid for stlink v1 */
        const ushort USB_STLINKv2_PID = 0x3748; /**< USB Pid for stlink v2 */
        const ushort USB_NUCLEO_PID = 0x374b; /**< USB Pid for nucleo */
        const byte USB_CONFIGURATION = 1; /**< The sole configuration. */
        const byte USB_INTERFACE = 0; /**< The interface. */
        const byte USB_ALTERNATE = 0; /**< The alternate interface. */
        const byte USB_PIPE_IN = 0x81; /**< Bulk output endpoint for responses */
        const byte USB_PIPE_OUT = 0x02; /**< Bulk input endpoint for commands */
        const byte USB_PIPE_OUT_NUCLEO = 0x01; /**< Bulk input endpoint for commands */
        const byte USB_PIPE_ERR = 0x83; /**< An apparently-unused bulk endpoint. */
        const ushort USB_TIMEOUT_MSEC = 300; /**< The usb bulk transfer timeout in ms */

        #endregion

        #region define

        enum Status
        {
            OK = 0x80, /**< OK return value */
            NOK = 0x81, /**< NOK return value */
            RUNNING = 0x80, /**< Core Running value */
            HALTED = 0x81, /**< Core Halted value */
            UNKNOWN_STATE = 2, /**< Not reported, internal use. */
        }

        enum Mode
        {
            DFU = 0x00,
            MASS = 0x01,
            DEBUG = 0x02, /**< TODO: describe */
            UNKNOWN = -1, /**< TODO: describe */
        }

        enum Cmd
        {
            GetVersion = 0xF1, /**< TODO: describe */
            DebugCommand = 0xF2, /**< TODO: describe */
            DFUCommand = 0xF3, /**< TODO: describe */
            DFUExit = 0x07, /**< TODO: describe */
            DFUGetVersion = 0x08, /**< TODO: describe */
            GetCurrentMode = 0xF5, /**< TODO: describe */
            Reset = 0xF7, /**< TODO: describe */

            EnterJTAG = 0x00, /**< TODO: describe */
            GetStatus = 0x01, /**< TODO: describe */
            ForceDebug = 0x02, /**< TODO: describe */
            ReadMem32bit = 0x07, /**< TODO: describe */
            WriteMem32bit = 0x08, /**< TODO: describe */
            RunCore = 0x09, /**< TODO: describe */
            StepCore = 0x0A, /**< TODO: describe */
            SetFP = 0x0B, /**< TODO: describe */
            ReadMem8bit = 0x0c, /**< TODO: describe */
            WriteMem8bit = 0x0d, /**< TODO: describe */
            Exit = 0x21, /**< TODO: describe */
            ReadCoreID = 0x22, /**< TODO: describe */
            EnterSWD = 0xA3, /**< TODO: describe */

#if false
            Enter = 0x20, /**< TODO: describe */
            ResetSys = 0x03, /**< TODO: describe */
            ReadReg = 0x05, /**< TODO: describe */
            WriteReg = 0x06, /**< TODO: describe */
            WriteDbgReg = 0x0f, /**< TODO: describe */
            ReadAllRegs = 0x04, /**< TODO: describe */
#else
            Enter = 0x30, /**< TODO: describe */
            ReadIDCode = 0x31, /**< TODO: describe */
            ResetSys = 0x32, /**< TODO: describe */
            ReadReg = 0x33, /**< TODO: describe */
            WriteReg = 0x34, /**< TODO: describe */
            WriteDbgReg = 0x35, /**< TODO: describe */
            ReadDbgReg = 0x36, /**< TODO: describe */
            ReadAllRegs = 0x3A, /**< All registers fetched at once */
            HardReset = 0x3C, /**< NRST pull down */
            SWDFREQ = 0x43,
#endif
        }

        enum CortexStatus
        {
            HALT = (1 << 17), /**< TODO: describe */
            SLEEP = (1 << 18), /**< TODO: describe */
            LOCKUP = (1 << 19), /**< TODO: describe */
            RESET = (1 << 25), /**< TODO: describe */
        }

        enum CortexControl:uint
        {
            DBGKEY = 0xA05F0000, /**< TODO: describe */
            DEBUGEN = (1 << 0), /**< TODO: describe */
            HALT = (1 << 1), /**< TODO: describe */
            STEP = (1 << 2), /**< TODO: describe */
            MASKINTS = (1 << 3), /**< TODO: describe */
        }

        enum CortexReg : uint
        {
            CM3_CHIPID = 0xE0042000, /**< TODO: describe */
            CM0_CHIPID = 0x40015800, /**< TODO: describe */
            CM3_CPUID = 0xE000ED00, /**< TODO: describe */
            CM3_FP_CTRL = 0xE0002000, /**< TODO: describe */
            CM3_FP_COMP0 = 0xE0002008, /**< TODO: describe */
            DCB_DHCSR = 0xE000EDF0, /**< TODO: describe */
            DCB_DCRSR = 0xE000EDF4, /**< TODO: describe */
            DCB_DCRDR = 0xE000EDF8, /**< TODO: describe */
            DCB_DEMCR = 0xE000EDFC, /**< TODO: describe */
            REG_AIRCR = 0xe000ed0c,
            REG_DHCSR = 0xe000edf0,
            REG_DEMCR = 0xE000EDFC,
            REG_BP_CTRL = 0xE0002000,
            REG_BP_0 = 0xE0002008,
        }

        enum CortexRegValue : uint
        {
            REG_AIRCR_VECTKEY = 0x05fa0000,
            REG_AIRCR_SYSRESETREQ = 0x00000004,
            REG_AIRCR_VECTRESET = 0x00000001,

            REG_DEMCR_TRCENA = (1 << 24),
            REG_DEMCR_VC_HARDERR = (1 << 10),
            REG_DEMCR_VC_BUSERR = (1 << 8),
            REG_DEMCR_VC_CORERESET = (1 << 0),


            REG_DHCSR_DBGKEY = 0xA05F0000,
            REG_DHCSR_C_DEBUGEN = (1 << 0),
            REG_DHCSR_C_HALT = (1 << 1),
            REG_DHCSR_C_STEP = (1 << 2),
            REG_DHCSR_C_MASKINTS = (1 << 3),
            REG_DHCSR_S_REGRDY = (1 << 16),
            REG_DHCSR_S_HALT = (1 << 17),
            REG_DHCSR_S_SLEEP = (1 << 18),
            REG_DHCSR_S_LOCKUP = (1 << 19),
            REG_DHCSR_S_RETIRE_ST = (1 << 24),
            REG_DHCSR_S_RESET_ST = (1 << 25),

            REG_BP_CTRL_ENABLE = (1<<0),
            REG_BP_CTRL_KEY = (1 << 1),
            REG_BP_CTRL_NUM_CODE_mask = (0xf << 4),
            REG_BP_CTRL_REV_mask = 0xf0000000,


            REG_BPx_ENABLE= (1 << 0),

            /*
             Stores bits [28:2] of the comparison address. The comparison address is
            compared with the address from the Code memory region. Bits [31:29] and
            [1:0] of the comparison address are zero.
            The field is UNKNOWN on power-on reset.
            */
            REG_BPx_COMP_mask = 0x0FFFFFFC,

            /*
             BP_MATCH defines the behavior when the COMP address is matched:
                00 no breakpoint matching.
                01 breakpoint on lower halfword, upper is unaffected.
                10 breakpoint on upper halfword, lower is unaffected.
                11 breakpoint on both lower and upper halfwords.
             */
            REG_BPx_MATCH_mask = 0xC0000000,
        }

        #endregion

        #region link
        public struct SP_DEVICE_INTERFACE_DATA
        {
            public int cbSize;
            public Guid interfaceClassGuid;
            public int flags;
            public int reserved;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
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
        //关闭访问设备句柄，结束进程的时候把这个加上保险点
        [DllImport("kernel32.dll")]
        static public extern int CloseHandle(IntPtr hObject);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_Initialize(IntPtr handle, ref IntPtr interface_handle);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_QueryDeviceInformation(IntPtr handle, uint InformationType, ref uint BufferLength, ref uint data);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_QueryInterfaceSettings(IntPtr handle, byte AlternateInterfaceNumber, ref ulong UsbAltInterfaceDescriptor);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_QueryPipe(IntPtr handle, byte AlternateInterfaceNumber, byte PipeIndex, ref ulong PipeInformation);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_ReadPipe(IntPtr handle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, uint ptr = 0);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_WritePipe(IntPtr handle, byte PipeID, byte[] Buffer, uint BufferLength, ref uint LengthTransferred, uint ptr = 0);

        [DllImport("winusb.dll")]
        private static extern Boolean WinUsb_Free(IntPtr handle);

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
        #endregion

        private IntPtr usbHandle;
        private IntPtr winusb;
        private byte[] cmdbuff;
        private byte[] recvbuff;

        public int stlink { get; private set; }
        public int jtag { get; private set; }
        public int swim { get; private set; }
        public uint BP_NUM { get; private set; }
        public uint BP_REV { get; private set; }

        public List<string> getHandle(int vid= 483, int pid= 3748)
        {
            var guidHID = Guid.Parse("{DBCE1CD9-A320-4b51-A365-A0C3F3C5FB29}");
            var hDevInfo = SetupDiGetClassDevs(ref guidHID, 0, IntPtr.Zero, DIGCF.DIGCF_PRESENT | DIGCF.DIGCF_DEVICEINTERFACE);
            SP_DEVICE_INTERFACE_DATA DeviceInterfaceData = new SP_DEVICE_INTERFACE_DATA();
            DeviceInterfaceData.cbSize = Marshal.SizeOf(DeviceInterfaceData);
            SP_DEVINFO_DATA strtInterfaceData = new SP_DEVINFO_DATA();
            List<string> HIDUSBAddress = new List<string>();
            int bufferSize = 0;
            for (int i = 0; i < 64; i++)
            {

                if (!SetupDiEnumDeviceInterfaces(hDevInfo, IntPtr.Zero, ref guidHID, (UInt32)i, ref DeviceInterfaceData))
                    continue;
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
        }

        public bool openSTLink(string devicename)
        {
            bool ret;
            usbHandle = CreateFile(
                devicename,
                GENERIC_READ | GENERIC_WRITE,// | GENERIC_WRITE,//读写，或者一起
               FILE_SHARE_READ | FILE_SHARE_WRITE,// | FILE_SHARE_WRITE,//共享读写，或者一起
               0,
                OPEN_EXISTING,
                0x40000000,
                0);
            ret = WinUsb_Initialize(usbHandle, ref winusb);
            if (!ret) return ret;
            uint len = 1;
            //uint data = 0;
            //ret = WinUsb_QueryDeviceInformation(winusb, 1, ref len, ref data);
            //if (!ret) return ret;
            //USB_INTERFACE_DESCRIPTOR s = new USB_INTERFACE_DESCRIPTOR();
            //byte[] dt = new byte[9];
            //ret = WinUsb_QueryInterfaceSettings(winusb, 0, ref s.ptr);
            //if (!ret) return ret;

            //for (int i = 0; i < s.bNumEndpoints; i++)
            //{
            //    WINUSB_PIPE_INFORMATION info = new WINUSB_PIPE_INFORMATION();
            //    ulong x = 0;
            //    ret = WinUsb_QueryPipe(winusb, 0, (byte)i, ref info.ptr);
            //    if (ret)
            //    {

            //        if (info.PipeType == USBD_PIPE_TYPE.UsbdPipeTypeControl)
            //        {

            //        }
            //    }

            //}
            
            byte[] cmd = new byte[16];
            byte[] recv = new byte[16];
            cmd[0] = 0xf1;
            cmd[1] = 0x80;
            WinUsb_WritePipe(winusb, USB_PIPE_OUT, cmd, (uint)cmd.Length, ref len, 0);
            WinUsb_ReadPipe(winusb, USB_PIPE_IN, recv, 6, ref len, 0);

            stlink = (recv[0] & 0xf0) >> 4;
            jtag = ((recv[0] & 0x0f) << 2) | ((recv[1] & 0xc0) >> 6);
            swim = recv[1] & 0x3f;
            return ret;
        }

        [StructLayout(LayoutKind.Explicit,Size = 4)]
        struct union
        {
            [FieldOffset(0)]
            public uint[] ui;
            [FieldOffset(0)]
            public int[] i;
            [FieldOffset(0)]
            public byte[] b;
            [FieldOffset(0)]
            public ushort[] us;
            [FieldOffset(0)]
            public short[] s;
            [FieldOffset(0)]
            public ulong[] ul;
        }

        union b2i = new union();
        union uni = new union();
        public STLINKV2()
        {
            uni.b = new byte[8];
            b2i.b = new byte[256];
            cmdbuff = new byte[16];
            recvbuff = new byte[16];
        }

        public void closeSTLink()
        {
            if (winusb != IntPtr.Zero)
            {
                WinUsb_Free(winusb);
                CloseHandle(usbHandle);
                winusb = usbHandle = IntPtr.Zero;
            }
        }

        byte[] CMD(int rlen,params Cmd[] value)
        {
            uint len=0;
            for (int i = 0; i < value.Length; i++)
                b2i.b[i] = (byte)value[i];
            WinUsb_WritePipe(winusb, USB_PIPE_OUT, b2i.b, (uint)16, ref len, 0);
            if(rlen!=0)
                WinUsb_ReadPipe(winusb, USB_PIPE_IN, b2i.b, (uint)rlen, ref len, 0);
            return b2i.b;
        }

        public bool exitDFU()
        {
            if (getMode() == (byte)Mode.DFU)
                CMD(0, Cmd.DFUCommand, Cmd.DFUExit);
            return true;
        }

        public uint ReadReg(int index)
        {
            b2i.ul[1]= b2i.ul[0] = 0;
            CMD( 8, Cmd.DebugCommand, Cmd.ReadReg, (Cmd)index);
            return b2i.ui[1];
        }

        public bool WriteReg(int index, uint dat)
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            b2i.ui[1] = dat;
            b2i.ul[0] >>= 8;
            CMD( 2, Cmd.DebugCommand, Cmd.WriteReg, (Cmd)index);
            return ReadReg(index) == dat;
        }

        public uint ReadDBGREG(uint addr)
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            b2i.ui[1] = addr;
            b2i.ul[0] >>= 16;
            CMD( 8, Cmd.DebugCommand, Cmd.ReadDbgReg);
            return b2i.ui[1];
        }

        public bool WriteDBGREG(uint addr,uint data)
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            b2i.ui[0] = addr;
            b2i.ui[2] = b2i.ui[1] = data;
            b2i.ul[0] <<= 16;
            b2i.ui[2] >>= 16;
            CMD(2, Cmd.DebugCommand, Cmd.WriteDbgReg);
            var ststs = b2i.b[0];
            //ReadDBGREG(addr);
            return ststs == (byte)Status.OK;
        }

        public bool isHalt()
        {
            uint st = ReadDBGREG((uint)CortexReg.DCB_DHCSR);
            return ((st & (uint)CortexStatus.HALT) != 0);
        }

        public uint getCoreID()
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            CMD( 4, Cmd.DebugCommand, Cmd.ReadCoreID);
            return b2i.ui[0];
        }

        public byte getMode()
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            CMD(2, Cmd.GetCurrentMode);
            return b2i.b[0];
        }

        public bool SWDMode()
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            CMD(2, Cmd.DebugCommand, Cmd.Enter, Cmd.EnterSWD);
            return true;
        }

        public bool reset()
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            CMD(2, Cmd.DebugCommand, Cmd.ResetSys);
            return true;
        }

        public bool HWreset()
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            CMD(8, Cmd.Reset);
            b2i.ul[0] = 0;
            CMD(2, Cmd.DebugCommand, Cmd.HardReset,(Cmd)2);
            return true;
        }

        public bool Go()
        {
           return WriteDBGREG((uint)CortexReg.DCB_DHCSR, (uint)(CortexControl.DBGKEY | CortexControl.DEBUGEN));
        }

        public bool Halt()
        {
            return WriteDBGREG((uint)CortexReg.DCB_DHCSR, (uint)(CortexControl.DBGKEY | CortexControl.HALT| CortexControl.DEBUGEN));
        }

        public bool writeMem(uint addr,int len,byte[] data)
        {
            uint count=0;
            b2i.ul[1] = 0;
            b2i.ui[0] = addr;
            b2i.us[2] = (ushort)len;
            b2i.ul[0] <<= 16;
            CMD(0,Cmd.DebugCommand, Cmd.WriteMem32bit);
            return WinUsb_WritePipe(winusb, USB_PIPE_OUT, data, (uint)len, ref count, 0);
        }

        public bool readMem(uint addr,int len,byte[] data)
        {
            uint count = 0;
            b2i.ul[1] = 0;
            b2i.ui[0] = addr;
            b2i.us[2] = (ushort)len;
            b2i.ul[0] <<= 16;
            CMD(0, Cmd.DebugCommand, Cmd.ReadMem32bit);
            return WinUsb_ReadPipe(winusb, USB_PIPE_IN, data, (uint)len, ref count, 0);
        }

        public bool SWDSpeed(SWDCLK clk)
        {
            b2i.ul[1] = b2i.ul[0] = 0;
            b2i.us[1] = (ushort)clk;
            CMD(2, Cmd.DebugCommand, Cmd.SWDFREQ);
            return true;
        }

        public bool soft_reset()
        {
            Halt();
            WriteDBGREG((uint)CortexReg.REG_DEMCR, (uint)CortexRegValue.REG_DEMCR_VC_CORERESET);
            ReadDBGREG((uint)CortexReg.DCB_DHCSR);
            WriteDBGREG((uint)CortexReg.REG_AIRCR, (uint)(CortexRegValue.REG_AIRCR_VECTKEY| CortexRegValue.REG_AIRCR_SYSRESETREQ));
            System.Threading.Thread.Sleep(10);
            WriteReg(16, 0x01000000);
            WriteDBGREG((uint)CortexReg.REG_DEMCR, (uint)CortexRegValue.REG_DEMCR_TRCENA);
            return true;
        }

        public bool InitBP()
        {
            WriteDBGREG((uint)CortexReg.REG_BP_CTRL, (uint)(CortexRegValue.REG_BP_CTRL_ENABLE | CortexRegValue.REG_BP_CTRL_KEY));
            var v=ReadDBGREG((uint)CortexReg.REG_BP_CTRL);
            BP_NUM = (v & (uint)CortexRegValue.REG_BP_CTRL_NUM_CODE_mask) >> 4;
            BP_REV = (v >> 28)&0xf;
            for(int i = 0; i < BP_NUM; i++)
                WriteDBGREG((uint)((uint)CortexReg.REG_BP_0+i*4), 0);
            return true;
        }

        public bool SetBP(uint index,uint addr)
        {
            return WriteDBGREG((uint)CortexReg.CM3_FP_CTRL + index * 4, (((addr & 2u) != 0) ? 2u << 30 : 1u << 30) | addr | 1u);
        }

        public bool ClearBP(uint index)
        {
            return WriteDBGREG((uint)CortexReg.CM3_FP_CTRL + index * 4, 0);
        }

        int readhex(string str, int num, int offset)
        {
            int index = offset;
            int value = 0;
            while (num-- != 0)
            {
                value <<= 4;
                value |= str[index] > '9' ? str[index] - ('A' - 10) : str[index] - '0';
                index++;
            }
            return value;
        }

        public int formatRead(string str, string format, int[] data,int pos=0)
        {
            int readcount = 0;
            int len = format.Length;
            int offset = pos;
            int index = 0;
            int num;
            for (int i = 0; i < format.Length; i++)
            {
                if (format[i] == '%')
                {
                    num = 1;
                    if (format[i + 1] >= '0' && format[i + 1] <= '9')
                    {
                        i++;
                        num = format[i] - '0';
                    }
                    switch (format[i + 1])
                    {
                        case 'c':
                            data[index++] = str[offset];
                            break;
                        case 'd': break;
                        case 'x':
                            data[index++] = readhex(str, num, offset);
                            break;
                        case 's':

                            break;
                    }
                    offset += num;
                    i++;
                    readcount++;
                    continue;
                }
                if (format[i] != str[offset])
                {
                    break;
                }
                offset++;
            }
            return readcount;
        }

        public byte[] hex80(string hex)
        {
            int pos = 0;
            int[] data = new int[3];
            int addr = 0,len=0,type=0;
            var ms = new System.IO.MemoryStream();
            while (hex[pos]!=0)
            {
                if (hex[pos] == ':')
                {
                    pos++;
                    formatRead(hex, "%2x%4x%2x", data,pos);
                    pos += 8;
                    if (data[2] == 0)
                    {
                        for (int i = 0; i < data[0]; i++)
                        {
                            ms.WriteByte((byte)readhex(hex, 2, pos));
                            pos += 2;
                        }
                    }
                    else if (data[2] == 1)
                        return ms.ToArray();
                }
                else
                    pos++;
            }
            return ms.ToArray();
        }

    }
}
