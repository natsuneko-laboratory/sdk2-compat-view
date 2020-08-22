/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;

namespace Mochizuki.VRChat.SDK2CompatView.Internal
{
    public class YamlDocs : IDisposable
    {
        private readonly YamlDocsHandle _handle;

        public bool BoolValue => SafeAccess(() => NativeMethods.AsBool(_handle));

        public long LongValue => SafeAccess(() => NativeMethods.AsLong(_handle));

        public double DoubleValue => SafeAccess(() => NativeMethods.AsDouble(_handle));

        public string StringValue => SafeAccess(() => NativeMethods.AsString(_handle).AsString());

        public bool IsBadValue => SafeAccess(() => NativeMethods.IsBadValue(_handle));

        public bool IsNull => SafeAccess(() => NativeMethods.IsNull(_handle));

        public bool IsArray => SafeAccess(() => NativeMethods.IsArray(_handle));

        public ulong ArraySize => SafeAccess(() => NativeMethods.ArraySize(_handle));

        public YamlDocs(YamlDocsHandle handle)
        {
            _handle = handle;
        }

        public void Dispose()
        {
            if (_handle != null && !_handle.IsInvalid)
                _handle.Dispose();
        }

        public YamlDocs FindRelative(string path)
        {
            if (_handle == null)
                throw new InvalidOperationException();
            return new YamlDocs(NativeMethods.FindRelative(_handle, path));
        }

        public T GetRelativeValueAs<T>(string path)
        {
            var @default = typeof(T);
            var value = FindRelative(path);

            switch (@default)
            {
                // ReSharper disable PatternAlwaysOfType

                case Type _ when @default == typeof(bool):
                    if (value.BoolValue is T boolValue) return boolValue;
                    break;

                case Type _ when @default == typeof(long):
                    if (value.LongValue is T longValue) return longValue;
                    break;

                case Type _ when @default == typeof(double):
                    if (value.DoubleValue is T doubleValue) return doubleValue;
                    break;

                case Type _ when @default == typeof(string):
                    if (value.StringValue is T stringValue) return stringValue;
                    break;

                // ReSharper restore PatternAlwaysOfType

                default:
                    return default;
            }

            return default;
        }

        private T SafeAccess<T>(Func<T> func)
        {
            if (_handle == null)
                throw new InvalidOperationException();
            return func.Invoke();
        }
    }
}