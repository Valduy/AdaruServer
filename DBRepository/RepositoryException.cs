using System;
using System.Collections.Generic;
using System.Text;

namespace DBRepository
{
    public class RepositoryException : Exception
    {
        public RepositoryException(string message) 
            : base(message)
        {}
    }
}
