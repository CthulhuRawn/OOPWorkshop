namespace Domain
{
    public class Treatment : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Dose{ get; set; }
        public virtual TreatmentTypeDecode Type { get; set; }
        public virtual TreatmentReport ContainingTreatment { get; set; }
    }
}
