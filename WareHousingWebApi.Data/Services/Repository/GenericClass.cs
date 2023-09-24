using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WareHousingWebApi.Data.DbContext;

namespace WareHousingWebApi.Data.Services.Repository;

public class GenericClass<Tentity> where Tentity : class
{
    private readonly ApplicationDbContext _context;
    private DbSet<Tentity> _table;
    public GenericClass(ApplicationDbContext context)
    {
        _context = context;
        _table = context.Set<Tentity>();
    }

    public virtual async Task Create(Tentity entity) => await _table.AddAsync(entity);


    public virtual void Update(Tentity entity) => _table.Update(entity);


    public virtual async Task<Tentity> GetById(object Id) { return await _table.FindAsync(Id); }

    public virtual void Delete(Tentity entity) => _table.Remove(entity);

    public virtual async void DeleteById(object Id)
    {
        var entity = await GetById(Id);
        Delete(entity);
    }

    public virtual void DeleteByRange(IEnumerable<Tentity> entities) => _table.RemoveRange(entities);

    public virtual IEnumerable<Tentity> Get(Expression<Func<Tentity, bool>>? whereVariable = null, string JoinStrig = "")
    {
        IQueryable<Tentity> query = _table;

        if (whereVariable != null)
        {
            query = query.Where(whereVariable);
        }

        if (JoinStrig != "")
        {
            foreach (string item in JoinStrig.Split(','))
            {
                query = query.Include(item);
            }
        }

        return query;
        
    }


    public IQueryable<Tentity> GetEn => _table;
    public IQueryable<Tentity> GetEnNoTraking => _table.AsNoTracking();
}
