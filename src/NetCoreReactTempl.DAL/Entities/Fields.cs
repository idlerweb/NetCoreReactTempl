namespace NetCoreReactTempl.DAL.Entities
{
    public class Field : BaseEntity
    {
        public string Name { get; set; }
        public string Value { get; set; }

        public long DataId { get; set; }
        public virtual Data Data { get; set; }
    }
}
