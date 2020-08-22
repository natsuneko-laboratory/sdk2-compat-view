/*-------------------------------------------------------------------------------------------
 * Copyright (c) Fuyuno Mikazuki / Natsuneko. All rights reserved.
 * Licensed under the MIT License. See LICENSE in the project root for license information.
 *------------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;

namespace Mochizuki.VRChat.Extensions.Convenience
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