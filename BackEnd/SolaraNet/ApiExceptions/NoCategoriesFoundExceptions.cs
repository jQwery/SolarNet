using AbstractsExceptions;

namespace SolaraNet.ApiExceptions
{
    public class NoCategoriesFoundExceptions : NotFoundException
    {
        public NoCategoriesFoundExceptions(string message) : base(message)
        {
        }
    }
}