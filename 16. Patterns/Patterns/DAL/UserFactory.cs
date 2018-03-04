using ORM;
using System.Collections.Generic;

namespace DAL
{
    public class UserFactory : IUserFactory
    {
        private IAddressFactory _addressFactory;

        public UserFactory(IAddressFactory addressFactory)
        {
            _addressFactory = addressFactory;
        }

        public IEnumerable<User> CreateUserForDb(List<UserExternal> listUserExternal)
        {
            foreach (UserExternal ue in listUserExternal)
                yield return new User()
                {
                    Id = ue.id,
                    Name = ue.name,
                    Username = ue.username,
                    Email = ue.email,
                    Address = _addressFactory.CreateAddressForDb(ue.address)
                };
        }
    }


}
