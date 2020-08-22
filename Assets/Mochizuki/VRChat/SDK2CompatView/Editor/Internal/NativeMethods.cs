/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Runtime.InteropServices;

namespace Mochizuki.VRChat.SDK2CompatView.Internal
{
    public static class NativeMethods
    {
        [DllImport("sdk2_compat", EntryPoint = "create_reader")]
        public static extern YamlReaderHandle CreateReader(string source);

        [DllImport("sdk2_compat", EntryPoint = "find_property")]
        public static extern YamlDocsHandle FindProperty(YamlReaderHandle handle, string path, ulong index);

        [DllImport("sdk2_compat", EntryPoint = "find_by_1st_key")]
        public static extern ulong FindBy1stKey(YamlReaderHandle handle, string key, IntPtr[] buffer, ulong bufferSize);

        [DllImport("sdk2_compat", EntryPoint = "document_size")]
        public static extern ulong DocumentSize(YamlReaderHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "destroy_reader")]
        public static extern void DestroyReader(YamlReaderHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "destroy_docs")]
        public static extern void DestroyDocs(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "find_relative")]
        public static extern YamlDocsHandle FindRelative(YamlDocsHandle handle, string path);

        [DllImport("sdk2_compat", EntryPoint = "as_bool")]
        public static extern bool AsBool(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "as_i64")]
        public static extern long AsLong(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "as_f64")]
        public static extern double AsDouble(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "as_string")]
        public static extern StringHandle AsString(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "is_bad_value")]
        public static extern bool IsBadValue(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "is_null")]
        public static extern bool IsNull(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "is_array")]
        public static extern bool IsArray(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "array_size")]
        public static extern ulong ArraySize(YamlDocsHandle handle);

        [DllImport("sdk2_compat", EntryPoint = "free_string")]
        public static extern void FreeString(StringHandle handle);
    }
}