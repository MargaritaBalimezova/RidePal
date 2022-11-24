using System;

namespace RidePal.Services.Exceptions
{
    public class EntityNotFoundException : Exception
    {
        public EntityNotFoundException(string msg)
        : base(msg)
        {
        }
    }
}