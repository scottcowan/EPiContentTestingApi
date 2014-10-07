namespace EpiContentTestingApi.Services
{
    public class CastingHelper
    {
        public static T CastToType<T>(object Object) where T : class
        {
            return Object as T;
        }
    }
}