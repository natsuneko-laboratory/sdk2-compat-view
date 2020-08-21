using System;

namespace Mochizuki.VRChat.VRC2CompatView.Internal
{
    public class YamlDocs : IDisposable
    {
        private readonly YamlDocsHandle _handle;

        public bool BoolValue => NativeMethods.AsBool(_handle);

        public long LongValue => NativeMethods.AsLong(_handle);

        public double DoubleValue => NativeMethods.AsDouble(_handle);

        public string StringValue => NativeMethods.AsString(_handle).AsString();

        public bool IsBadValue => NativeMethods.IsBadValue(_handle);

        public bool IsNull => NativeMethods.IsNull(_handle);

        public bool IsArray => NativeMethods.IsArray(_handle);

        public ulong ArraySize => NativeMethods.ArraySize(_handle);

        public YamlDocs(YamlDocsHandle handle)
        {
            _handle = handle;
        }

        public void Dispose()
        {
            NativeMethods.DestroyDocs(_handle);
        }

        public YamlDocs FindRelative(string path)
        {
            return new YamlDocs(NativeMethods.FindRelative(_handle, path));
        }

        public T GetRelativeValueAs<T>(string path)
        {
            var @default = typeof(T);
            using (var value = FindRelative(path))
            {
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
            }

            return default;
        }
    }
}