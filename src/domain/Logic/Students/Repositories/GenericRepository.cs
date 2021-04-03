﻿using CSharpFunctionalExtensions;
using Logic.Utils;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Students.Repositories
{
    public interface IGenericRepository<T> where T : Entity
    {
        Task<IReadOnlyList<T>> GetAll();
        Task<Maybe<T>> Get(long id);
        Task Insert(T obj);
        Task Update(T obj);
        Task Delete(long id);
        Task Save();
    }

    public class GenericRepository<T> : IGenericRepository<T> where T : Entity
    {
        protected readonly EfDbContext _context;

        public GenericRepository(EfDbContext context)
        {
            _context = context;
        }


        public async Task<IReadOnlyList<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<Maybe<T>> Get(long id)
        {
            var entity = await _context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity ?? Maybe<T>.None;
        }

        public async Task Insert(T obj)
        {
            await _context.Set<T>().AddAsync(obj);
        }

        public Task Update(T obj)
        {
            _context.Set<T>().Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            return Task.CompletedTask;
        }

        public async Task Delete(long id)
        {
            T existing = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            _context.Set<T>().Remove(existing);
        }

        public Task Save()
        {
            return _context.SaveChangesAsync();
        }
    }
}