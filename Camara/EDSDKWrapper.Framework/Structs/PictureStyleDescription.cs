using EDSDKWrapper.Framework.Enums;
using System;
using System.Runtime.InteropServices;

namespace EDSDKWrapper.Framework.Structs
{
    /// <summary>
    /// TODO - document
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct PictureStyleDescription
    {
        public int Contrast;
        public UInt32 Sharpness;
        public int Saturation;
        public int ColorTone;
        public MonochromeFilterEffect MonochromeFilterEffect;
        public MonochromeTone MonochromeTone;
    }
}
