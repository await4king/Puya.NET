using System;
using System.Collections.Generic;
using System.Text;

namespace Puya.Data
{
    public class DbNullException : Exception
    {
        public DbNullException() : base("given db is null")
        { }
    }
    public class ConnectionNullException : Exception
    {
        public ConnectionNullException() : base("given connection is null")
        { }
    }
    public class CommandNullException : Exception
    {
        public CommandNullException() : base("given command is null")
        { }
    }
    public class CommandConnectionNullException : Exception
    {
        public CommandConnectionNullException() : base("given command's connection is null")
        { }
    }
    public class ListNullException : Exception
    {
        public ListNullException() : base("given list is null")
        { }
    }
}
