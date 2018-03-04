using ORM;

namespace DAL
{
    public interface IAddressFactory
    {
        Address CreateAddressForDb(AddressExternal AddressExternal);
    }
}
