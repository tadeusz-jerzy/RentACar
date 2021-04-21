using System;
using System.Collections.Generic;
using RentACar.Core.Exceptions;

namespace RentACar.Core.Entities
{
    public class Vin : ValueObject
    {
        [MinLength(17)]
        [MaxLength(17)]
        public string Code { get; private set; }

        public Vin() { }
        public Vin(string vinCode)
        {
            if (!IsValidVinCode(vinCode))
                throw new InvalidDomainValueException(nameof(vinCode));

            Code = vinCode;
        }

        private static bool IsValidVinCode(string vinCode)
        {
            return vinCode.Length == 17;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}
