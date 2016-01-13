using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Framework.Utility.KeyMouseHook
{
    /// <summary>
    /// 声明一个Point的封送类型
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public class POINT
    {
        public int x;
        public int y;
    }
}
