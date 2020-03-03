using System;

namespace CoreEngine.Model.Common
{
    public abstract class BaseService
    {
        public DateTime CurrentTime => DateTime.UtcNow.AddHours(6);
    }
}
