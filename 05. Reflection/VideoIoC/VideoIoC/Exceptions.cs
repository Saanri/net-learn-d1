using System;

namespace VideoIoC
{
    public class TypeNotSupported : Exception
    {
        public TypeNotSupported() : base() { }
        public TypeNotSupported(string message) : base(message) { }
    }

    public class CanNotCreateAbstractType : Exception
    {
        public CanNotCreateAbstractType() : base() { }
        public CanNotCreateAbstractType(string message) : base(message) { }
    }

    public class TypeMismatchObjectAndProperty : Exception
    {
        public TypeMismatchObjectAndProperty() : base() { }
        public TypeMismatchObjectAndProperty(string message) : base(message) { }
    }

    public class ConstructorNotFound : Exception
    {
        public ConstructorNotFound() : base() { }
        public ConstructorNotFound(string message) : base(message) { }
    }
    
}
