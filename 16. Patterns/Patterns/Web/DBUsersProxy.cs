using System;
using System.Net;

namespace Web
{
	public class DBUsersProxy : IProxy
	{
		private readonly WebClient _client;

		public DBUsersProxy()
		{
			_client = new WebClient();
		}

		public string GetData(string url)
		{
			return _client.DownloadString(url);
		}

		#region IDisposable Support
		private bool disposedValue = false;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					_client.Dispose();
				}

				disposedValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DBUsersProxy()
        {
            Dispose(false);
        }
        #endregion
    }
}
