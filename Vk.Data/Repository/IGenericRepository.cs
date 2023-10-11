﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vk.Base.Model;

namespace Vk.Data.Repository;

public interface IGenericRepository<TEntity> where TEntity : BaseModel
{
    Task<TEntity> GetByIdAsync(int id,CancellationToken cancellationToken, params string[] includes);
    TEntity GetById(int id, params string[] includes);
    List<TEntity> GetAll(params string[] includes);
    void DeleteById(int id);
    void Delete(TEntity entity);
    void RemoveById(int id);  
    void Remove(TEntity entity);
    void Update(TEntity entity);
    void Insert(TEntity entity, CancellationToken cancellationToken);
    void InsertAsync(TEntity entity, CancellationToken cancellationToken);
    void InsertRange(List<TEntity> entities);
    IQueryable<TEntity> GetAsQueryable(params string[] includes);
    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression, params string[] includes);
}
