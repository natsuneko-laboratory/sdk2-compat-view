using System;
using System.Collections.Generic;

namespace Mochizuki.VRChat.VRC2CompatView.Internal
{
    public class CompositeDisposables : IDisposable
    {
        private readonly List<IDisposable> _disposables;

        public CompositeDisposables()
        {
            _disposables = new List<IDisposable>();
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
                disposable?.Dispose();
        }

        public void Add(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }
    }
}