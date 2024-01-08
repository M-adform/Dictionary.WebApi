namespace Domain.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Item not found.") { }
    }
}