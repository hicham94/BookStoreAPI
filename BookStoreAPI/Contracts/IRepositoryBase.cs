﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreAPI.Contracts
{
    public interface IRepositoryBase<T> where T:class
    {

        Task<IList<T>> FindAll();
        Task<T> FindById(int Id);
        Task<bool> IsExist(int Id);
        Task<bool> Create(T entity);
        Task<bool> Update(T entity);
        Task<bool> Delete(T entity);
        Task<bool> Save();


    }
}
