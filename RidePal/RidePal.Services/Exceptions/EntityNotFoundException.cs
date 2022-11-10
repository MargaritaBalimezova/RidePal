using System;
using System.Collections.Generic;
using System.Text;

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
