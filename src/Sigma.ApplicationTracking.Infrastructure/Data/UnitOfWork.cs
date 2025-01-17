using Microsoft.EntityFrameworkCore.Storage;
using Sigma.ApplicationTracking.Domain.Entities;
using Sigma.ApplicationTracking.Domain.Interface;
using Sigma.ApplicationTracking.Infrastructure.Data.Context;
using Sigma.ApplicationTracking.Infrastructure.Repositories;

namespace Sigma.ApplicationTracking.Infrastructure.Data
{

    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicantTrackerDbContext _context;
        public UnitOfWork(ApplicantTrackerDbContext context)
        {
            _context = context;
            ApplicantRepository = new Repository<Applicant>(context);

        }

        public IRepository<Applicant> ApplicantRepository { get; }



        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
