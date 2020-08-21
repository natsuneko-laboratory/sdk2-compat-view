using System;
using System.Runtime.InteropServices;

namespace Mochizuki.VRChat.VRC2CompatView.Internal
{
    public class YamlDocsHandle : SafeHandle
    {
        public override bool IsInvalid => handle == IntPtr.Zero;

        public YamlDocsHandle() : base(IntPtr.Zero, true) { }

        protected override bool ReleaseHandle()
        {
            if (IsInvalid)
                return true;

            NativeMethods.DestroyDocs(this);
            return true;
        }
    }
}