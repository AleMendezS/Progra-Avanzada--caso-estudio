using Microsoft.EntityFrameworkCore;
using APW2.Data.Models;

namespace AB.Data.Repository;

public interface IRepositoryBase<T> where T : class
{
    Task<bool> CreateAsync(T entity);
    Task<bool> UpdateAsync(T entity);
    Task<bool> DeleteAsync(T entity);
    Task<IEnumerable<T>> ReadAsync();
    Task<bool> ExistsAsync(T entity);
}

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly ProcessdbContext _context;

    public RepositoryBase()
    {
        _context = new ProcessdbContext(); 
    }

    public async Task<bool> CreateAsync(T entity)
    {
        _context.Set<T>().Add(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(T entity)
    {
        _context.Set<T>().Update(entity);
        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(T entity)
    {
        _context.Set<T>().Remove(entity);
        return await SaveChangesAsync();
    }

    public async Task<IEnumerable<T>> ReadAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<bool> ExistsAsync(T entity)
    {
        return await _context.Set<T>().AnyAsync(e => e == entity);
    }

    protected async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}