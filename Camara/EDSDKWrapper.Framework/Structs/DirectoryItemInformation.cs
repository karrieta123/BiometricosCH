using EDSDKWrapper.Framework.Miscellaneous;
using System;
using System.Runtime.InteropServices;

namespace EDSDKWrapper.Framework.Structs
{
    /// <summary>
    /// TODO - document
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct DirectoryItemInformation
    {
        public UInt64 Size;
        public int IsFolder;
        public UInt32 GroupID;   // K234 later
        public UInt32 Option;    // K234 later EdsEnumTransferOption

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = GlobalConstants.MAX_NAME)]
        public string FileName;
    }
}
