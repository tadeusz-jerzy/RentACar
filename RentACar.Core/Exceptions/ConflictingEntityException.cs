using System;

namespace RentACar.Core.Exceptions
{
    [Serializable]
    public class ConflictingEntityException : BusinessException
    {
        public ConflictingEntityException() : base() { }
        public ConflictingEntityException(string message) : base(message) { }
        
        public ConflictingEntityException(string message, string entityType, int entityId) : base(message, entityType, entityId) { }
        public ConflictingEntityException(string message, Exception inner) : base(message, inner) { }

        // "A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client" (MS docs)
        protected ConflictingEntityException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }

}
