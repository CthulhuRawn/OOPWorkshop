namespace EZVet.DTOs
{
    public abstract class Entity<TDTO, TDomain> : EntityBase where TDTO: EntityBase
    {
        public abstract TDTO Initialize(TDomain domain);

        public virtual TDTO ShallowInitialize(TDomain domain)
        {
            return null;
        }
    }

    public abstract class EntityBase
    {
        public int? Id;
    }
}
