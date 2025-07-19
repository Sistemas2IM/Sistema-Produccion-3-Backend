using System.Collections.Concurrent;

namespace Sistema_Produccion_3_Backend.Services.RequestLock
{
    public class RequestLockService : IRequestLockService
    {
        private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks = new();

        public async Task<IDisposable> AcquireLockAsync(object key)
        {
            var sem = _locks.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            await sem.WaitAsync();

            return new Releaser(key, sem, _locks);
        }

        private class Releaser : IDisposable
        {
            private readonly object _key;
            private readonly SemaphoreSlim _sem;
            private readonly ConcurrentDictionary<object, SemaphoreSlim> _locks;

            public Releaser(object key, SemaphoreSlim sem, ConcurrentDictionary<object, SemaphoreSlim> locks)
            {
                _key = key;
                _sem = sem;
                _locks = locks;
            }

            public void Dispose()
            {
                _sem.Release();
                if (_sem.CurrentCount == 1) // No más esperas
                {
                    _locks.TryRemove(_key, out _);
                }
            }
        }
    }
}
