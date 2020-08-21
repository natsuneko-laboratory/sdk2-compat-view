using System;

namespace Mochizuki.VRChat.VRC2CompatView.Internal
{
    public class YamlReader : IDisposable
    {
        private readonly YamlReaderHandle _handle;
        private bool _isDisposed;

        public YamlReader(string source)
        {
            _isDisposed = false;
            _handle = NativeMethods.CreateReader(source);
        }

        public void Dispose()
        {
            if (!_isDisposed)
                NativeMethods.DestroyReader(_handle);
            _isDisposed = true;
        }

        public YamlDocs FindProperty(string path)
        {
            return new YamlDocs(NativeMethods.FindProperty(_handle, path));
        }

        public T GetValueAs<T>(string path) where T : class
        {
            var @default = typeof(T);
            using (var value = FindProperty(path))
            {
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
}