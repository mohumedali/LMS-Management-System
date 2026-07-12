using CourseManagment.DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static CourseManagment.DAL.Repositories.GenaricRepo;

namespace CourseManagment.DAL.Repositories
{
    public class GenaricRepo
    {
        public class GenaricRepository<Tentity> : IGenaricRepo<Tentity> where Tentity : class
        {
            private readonly CourseDbContext _dbcontext;
            private readonly DbSet<Tentity> _set;

            public GenaricRepository(CourseDbContext dbContext)
            {
                _dbcontext = dbContext;
                _set = _dbcontext.Set<Tentity>();
            }

            public async Task<int> AddAsync(Tentity entity)
            {
                await _set.AddAsync(entity);
                return await _dbcontext.SaveChangesAsync();
            }

            // DELETE
            public async Task<int> DeleteAsync(Tentity entity)
            {
                _set.Remove(entity);
                return await _dbcontext.SaveChangesAsync();
            }

            public async Task<int> UpdateAsync(Tentity entity)
            {
                _set.Update(entity);
                return await _dbcontext.SaveChangesAsync();
            }

            public async Task<IEnumerable<Tentity>> GetAll(CancellationToken ct, bool tracking = false)
            {
                IQueryable<Tentity> query = tracking ? _set : _set.AsNoTracking();

                return await query.ToListAsync(ct);
            }

            public async Task<Tentity?> GetByIdAsync(int id, CancellationToken ct)
            {
                return await _set.FindAsync(new object[] { id }, ct);
            }

            public async Task<bool> AnyAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken ct)
            {
                return await _set.AsNoTracking().AnyAsync(predicate, ct);
            }

 
            public async Task<Tentity?> FirstOrDefault(
                Expression<Func<Tentity, bool>> predicate,
                CancellationToken ct,
                bool tracking = false)
            {
                IQueryable<Tentity> query = tracking ? _set : _set.AsNoTracking();

                return await query.FirstOrDefaultAsync(predicate, ct);
            }
        }
    }
}
