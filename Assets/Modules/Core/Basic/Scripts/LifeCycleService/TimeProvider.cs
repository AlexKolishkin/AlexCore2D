using System;

namespace Core
{
    public interface ITimeProvider
    {
        DateTime Get();
    }

    public class LocalTimeProvider : ITimeProvider
    {
        public DateTime Get() => DateTime.Now;
    }
}