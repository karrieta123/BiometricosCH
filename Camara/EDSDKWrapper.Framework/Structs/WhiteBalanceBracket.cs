using EDSDKWrapper.Framework.Enums;
using System.Runtime.InteropServices;

namespace EDSDKWrapper.Framework.Structs
{
    /// <summary>
    /// Indicates the white balance bracket amount.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WhiteBalanceBracket
    {
        BracketMode BracketMode;
        WhiteBalanceShift WhiteBalanceShift;
    }
}
