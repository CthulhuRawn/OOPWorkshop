namespace Domain
{
    public class Field : Entity
    {
        public virtual string Name { get; set; }

        public virtual FieldTypeDecode Type { get; set; }

        public virtual FieldSizeDecode Size { get; set; }
    }
}
