namespace ItemStore.WebApi.csproj.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Item not found.") { }
    }
}