namespace Domain
{
    public class Admin : Entity
    {
        public virtual string Username { get; set; }

        public virtual string Password { get; set; }
    }
}
