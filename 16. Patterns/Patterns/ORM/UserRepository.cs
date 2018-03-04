using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace ORM
{
    public class UserRepository : IRepository<User>
    {
        private readonly MyDBUsers _db;

        public UserRepository(MyDBUsers db)
        {
            _db = db;
        }

        public IEnumerable<User> GetAll()
        {
            return _db.Users;
        }

        public User Get(int id)
        {
            try
            {
                return _db.Users.Where(u => u.Id == id).First();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public void Add(User item)
        {
            _db.Users.Add(item);
            _db.SaveChanges();
        }

        public void Remove(User item)
        {
            throw new NotImplementedException();
        }

        public void Update(User item)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
