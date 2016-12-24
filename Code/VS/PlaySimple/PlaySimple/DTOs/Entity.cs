namespace PlaySimple.DTOs
{
    public abstract class Entity<TDTO, TDomain> : EntityBase
    {
        public abstract TDTO Initialize(TDomain domain);
    }

    public abstract class EntityBase
    {
        public int? Id;
    }
}
