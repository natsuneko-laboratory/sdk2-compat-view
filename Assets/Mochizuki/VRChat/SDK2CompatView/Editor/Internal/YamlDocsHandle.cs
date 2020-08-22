/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;

namespace Mochizuki.VRChat.VRC2CompatView.Internal
{
    public class YamlDocsHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public YamlDocsHandle() : base(IntPtr.Zero, true) { }

        public YamlDocsHandle(IntPtr ptr) : base(IntPtr.Zero, true)
        {
            SetHandle(ptr);
        }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            NativeMethods.DestroyDocs(this);
            return true;
        }
    }
}