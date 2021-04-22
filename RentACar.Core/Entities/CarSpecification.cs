using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RentACar.Core.Exceptions;


namespace RentACar.Core.Entities
{
    /*
     * 
     * In real life, we'd need to sometimes update list of valid car makes and models 
     * in the system. Also, if we change "VW" to "Volkswagen" in the system, it makes sense to 
     * propagate the change to all car specification objects.
     * For this reason, car make + car model make sense to be modelled as entities 
     * and CarSpecification contains references to them. 
     * DDD allows for value objects to reference entities although it's not typical.
     * 
     *
     * value equality 
     *      implemented via inheriting from ValueObject + GetEqualityComponents
     *
     * immutability 
     *      "private set" remains as it's needed for serialization per https://docs.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/implement-value-objects 
     *      
     * self-validation at creation
     *      implemented here in factory methods + data annotations help as well
     *
     *      real-life and more "S"olid: we could have i.e. acrissvalidator as a service implemented + registered
     *      in the infrastructure project, then injected *as a parameter* into factory method below
     * 
     */
    [ComplexType]
    public class CarSpecification : ValueObject
    {
        [Required]
        public CarModel Model { get; private set; } 
        public int CarModelId { get; private set; }
        public string AcrissCode { get; private set; }

        public CarMake Make => Model.CarMake;

        #region creation methods
        private CarSpecification() { }
        private CarSpecification(CarModel model, string acrissCode)
        {
            Model = model;
            
            AcrissCode = acrissCode ?? string.Empty;
        }

        public static CarSpecification FromModel(CarModel model)
        {
            // car model now contains a valid reference to car make
            if (model == null)
                throw new InvalidDomainValueException("invalid car make/model combination");
            
            return new CarSpecification(model, acrissCode: string.Empty);
            
        }

        public static CarSpecification FromModelAndAcriss(CarModel model, string acrissCode)
        {
            var makeModelSpec = FromModel(model);

            var acode = (acrissCode ?? string.Empty).ToUpper();

            if (!IsValidAcrissCode(acode))
                throw new InvalidDomainValueException("invalid acriss code");

            // immutability / private set is only for the serializer
            return new CarSpecification(makeModelSpec.Model, acode);

        }

        #endregion

        #region static validation used by factory methods
  
        private static bool IsValidAcrissCode(string code) =>
            (code.Length == 4);

        #endregion

        #region ValueObject
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Make;
            yield return Model;
            yield return AcrissCode;
        }
        #endregion

    }
}
