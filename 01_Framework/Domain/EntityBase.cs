namespace _01_Framework.Domain
{
    public class EntityBase
    {
        public long Id { get; private set; }
        public DateTime CreationDate { get; private set; }
        public EntityBase()
        {
            Id = Random.Shared.NextInt64(111111111, 999999999);
            CreationDate = DateTime.Now;
        }
    }
}