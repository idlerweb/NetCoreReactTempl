namespace NetCoreReactTempl.DAL.Entities
{
    public class BaseDataEntity : BaseEntity
    {
        public long UserId { get; set; }
        public virtual User User { get; set; }
    }
}
