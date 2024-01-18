namespace AuditLog.API.AuditTrail.Interfaces
{
    public interface IAuditTrail<T>
    {
        void Insert(T entity);
        void Update(T oldEntity, T newEntity);
        void Delete(T oldEntity);
    }
}
