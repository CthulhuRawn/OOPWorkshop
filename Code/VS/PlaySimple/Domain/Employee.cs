namespace Domain
{
    public class Employee : UserBase
    {
        public virtual int Salary { get; set; }

        public virtual string Email { get; set; }
    }
}
