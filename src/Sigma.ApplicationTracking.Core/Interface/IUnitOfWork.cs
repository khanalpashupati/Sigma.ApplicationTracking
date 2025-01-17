using Sigma.ApplicationTracking.Domain.Entities;


namespace Sigma.ApplicationTracking.Domain.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Applicant> ApplicantRepository { get; }
        Task<int> SaveAsync();
    }
}
