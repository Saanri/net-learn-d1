namespace ORM
{
	// можешь не исправлять, но обычно классы разбивают по папкам. по назначению (Entities,Repositories and etc) или по бизнес смыслу (Users,Addresses and etc.)
	public class Address
	{
		public int Id { get; set; }
		public string Street { get; set; }
		public string Suite { get; set; }
		public string City { get; set; }
	}
}
