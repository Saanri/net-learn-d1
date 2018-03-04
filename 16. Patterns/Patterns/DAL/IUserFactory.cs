using ORM;
using System.Collections.Generic;

namespace DAL
{
    public interface IUserFactory
    {
        IEnumerable<User> CreateUserForDb(List<UserExternal> listUserExternal);
    }
}
