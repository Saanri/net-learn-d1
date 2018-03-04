namespace ORM
{
    public class UserExternal
    {
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public AddressExternal address { get; set; }
    }
}
