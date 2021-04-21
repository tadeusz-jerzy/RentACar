using System;

namespace RentACar.Core.Exceptions
{
    [Serializable]
    public class EntityNotFoundException : BusinessException
    {
        public EntityNotFoundException() : base() { }
        public EntityNotFoundException(string message) : base(message) { }
        
        public EntityNotFoundException(string message, string entityType, int entityId) : base(message, entityType, entityId) { }
        public EntityNotFoundException(string message, Exception inner) : base(message, inner) { }

        // "A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client" (MS docs)
        protected EntityNotFoundException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }

}
