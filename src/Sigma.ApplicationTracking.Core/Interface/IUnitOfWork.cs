using Sigma.ApplicationTracking.Core.Entities;


namespace Sigma.ApplicationTracking.Core.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<Applicant> ApplicantRepository { get; }
        Task<int> SaveAsync();
    }
}
