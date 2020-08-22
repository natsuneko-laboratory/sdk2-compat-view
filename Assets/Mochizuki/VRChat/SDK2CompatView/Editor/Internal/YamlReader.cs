/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Mochizuki.VRChat.VRC2CompatView.Internal
{
    public class YamlReader : IDisposable
    {
        private readonly YamlReaderHandle _handle;

        public YamlReader(string source)
        {
            _handle = NativeMethods.CreateReader(source);
        }

        public void Dispose()
        {
            if (_handle != null && !_handle.IsInvalid)
                _handle.Dispose();
        }

        public YamlDocs FindProperty(string path, ulong index = 0)
        {
            if (_handle == null)
                throw new InvalidOperationException();

            return new YamlDocs(NativeMethods.FindProperty(_handle, path, index));
        }

        // ReSharper disable once InconsistentNaming
        public List<YamlDocs> FindBy1stKey(string key)
        {
            if (_handle == null)
                throw new InvalidOperationException();

            var size = NativeMethods.FindBy1stKey(_handle, key, null, 0);
            var buffer = new IntPtr[size];
            NativeMethods.FindBy1stKey(_handle, key, buffer, (ulong) buffer.Length);

            return buffer.Select(w => new YamlDocsHandle(w)).Select(w => new YamlDocs(w)).ToList();
        }

        public T GetValueAs<T>(string path, ulong index = 0) where T : class
        {
            var @default = typeof(T);
            var value = FindProperty(path, index);
            switch (@default)
            {
                // ReSharper disable PatternAlwaysOfType

                case Type _ when @default == typeof(bool):
                    return value.BoolValue as T;

                case Type _ when @default == typeof(long):
                    return value.LongValue as T;

                case Type _ when @default == typeof(double):
                    return value.DoubleValue as T;

                case Type _ when @default == typeof(string):
                    return value.StringValue as T;

                // ReSharper restore PatternAlwaysOfType

                default:
                    return null;
            }
        }
    }
}