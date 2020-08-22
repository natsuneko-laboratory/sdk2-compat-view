/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Mochizuki.VRChat.SDK2CompatView.Internal
{
    public class StringHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public StringHandle() : base(IntPtr.Zero, true) { }

        public string AsString()
        {
            var len = 0;
            while (Marshal.ReadByte(handle, len) != 0)
                len++;

            var buffer = new byte[len];
            Marshal.Copy(handle, buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer);
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            NativeMethods.FreeString(this);
            return true;
        }
    }
}