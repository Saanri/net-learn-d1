using ORM;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Web;

namespace DAL
{
	public class UserService : IService<UserExternal, User>
	{
		private readonly IProxy _proxy;
		private readonly IRepository<User> _repository;
		private readonly IUserFactory _userFactory;

		public UserService(IProxy proxy, IRepository<User> repository, IUserFactory userFactory)
		{
			_proxy = proxy;
			_repository = repository;
            _userFactory = userFactory;
		}

		public List<UserExternal> GetAllByUrl(string url)
		{
			string json = _proxy.GetData(url);

			var json_serializer = new JavaScriptSerializer();
			return json_serializer.Deserialize<List<UserExternal>>(json);
		}

		public List<User> ConvertForDb(List<UserExternal> list)
		{
            var res = new List<User>();
            foreach (var u in _userFactory.CreateUserForDb(list))
                res.Add(u);

            return res;
		}

		public void SaveAllToDb(List<User> list)
		{
			foreach (var u in list)
				_repository.Add(u);
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_proxy.Dispose();
					_repository.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UserService()
        {
            Dispose(false);
        }
        #endregion
    }
}
