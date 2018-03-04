using ORM;

namespace DAL
{
    public class AddressFactory : IAddressFactory
    {
        public Address CreateAddressForDb(AddressExternal AddressExternal)
        {
            return new Address()
            {
                Street = AddressExternal.street,
                Suite = AddressExternal.suite,
                City = AddressExternal.city
            };
        }
    }
}
