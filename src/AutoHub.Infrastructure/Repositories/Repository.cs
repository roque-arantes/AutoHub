using System.Linq.Expressions;
using AutoHub.Application.Interfaces;
using AutoHub.Domain.Common;
using AutoHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AutoHub.Infrastructure.Repositories;

public class Repository<T>(ApplicationDbContext context) : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context = context;
    protected readonly DbSet<T> _dbSet = context.Set<T>();

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).AsNoTracking().ToListAsync();
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _dbSet.AnyAsync(e => e.Id == id);
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task<(IEnumerable<T> Items, int TotalCount)> GetPagedAsync(int page, int pageSize)
    {
        var query = _dbSet.AsNoTracking();

        var totalCount = await query.CountAsync();
        var offset = ((long)page - 1) * pageSize;

        // Uma página além do total é vazia, inclusive quando o offset excede int.MaxValue.
        if (offset >= totalCount)
            return (Array.Empty<T>(), totalCount);

        var items = await query
            .OrderBy(e => e.Id)
            .Skip((int)offset)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
