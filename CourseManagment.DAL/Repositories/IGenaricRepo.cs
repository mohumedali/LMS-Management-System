using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagment.DAL.Repositories
{
    public interface IGenaricRepo<Tentity>
    {
        Task<int> AddAsync(Tentity entity);

        Task<int> UpdateAsync(Tentity entity);

        Task<int> DeleteAsync(Tentity entity);

        Task<IEnumerable<Tentity>> GetAll(CancellationToken ct, bool tracking = false);

        Task<Tentity?> GetByIdAsync(int id, CancellationToken ct);

        Task<bool> AnyAsync(Expression<Func<Tentity, bool>> predicate, CancellationToken ct);

        Task<Tentity?> FirstOrDefault(
            Expression<Func<Tentity, bool>> predicate,
            CancellationToken ct,
            bool tracking = false);
    }
}

