using System;

namespace RentACar.Core.Exceptions
{
    [Serializable]
    public class InvalidDomainValueException : BusinessException
    {
        public InvalidDomainValueException() : base() { }
        public InvalidDomainValueException(string message) : base(message) { }
        public InvalidDomainValueException(string message, Exception inner) : base(message, inner) { }

        // "A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client" (MS docs)
        protected InvalidDomainValueException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }

}
