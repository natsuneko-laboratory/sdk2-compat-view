/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;

namespace Mochizuki.VRChat.SDK2CompatView.Internal
{
    public class YamlReaderHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public YamlReaderHandle() : base(IntPtr.Zero, true) { }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            NativeMethods.DestroyReader(this);
            return true;
        }
    }
}