namespace Sistema_Produccion_3_Backend.Services.RequestLock
{
    public interface IRequestLockService
    {
        Task<IDisposable> AcquireLockAsync(object key);
    }
}
