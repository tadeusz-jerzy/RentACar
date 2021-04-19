using System;

namespace RentACar.Core.Exceptions
{
    [Serializable]
    public class BusinessException : Exception
    {
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public BusinessException() : base() { }
        public BusinessException(string message) : base(message) { }
        public BusinessException(string message, string entityType, int entityId) : base(message) 
        {
            EntityType = entityType;
            EntityId = entityId;
        }
        public BusinessException(string message, Exception inner) : base(message, inner) { }

        // "A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client" (MS docs)
        protected BusinessException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

    }

}
