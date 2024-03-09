using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace userManagerApplication.Repository.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        IEnumerable<TEntity> Find(Expression<Func<TEntity,bool>> filter, string? include);
        void Add(TEntity data);
        void AddList(IEnumerable<TEntity> data);
        void Delete(int id);
        void DeleteList(IEnumerable<TEntity> dataList);
        void Update(TEntity data);
        void UpdateList(IEnumerable<TEntity> data);
        void Save();

    }
}
