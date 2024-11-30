﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTier.Repositories
{
    internal interface IEntityRepository<T> where T : class
    {
        public T Create(T newEntity);

        public bool Update(T updatedEntity);

        public void Delete(Guid id);

        public T Read(Guid id);
    }
}