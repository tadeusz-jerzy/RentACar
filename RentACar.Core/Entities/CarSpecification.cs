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
     * (which I don't do here) 
     * and CarSpecification would contain references to them. 
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
        public string Make { get; private set; }
        [Required]
        public string Model { get; private set; }
        public string AcrissCode { get; private set; } 

        #region creation methods
        private CarSpecification(string make, string model, string acrissCode)
        {
            Make = make;
            Model = model;
            AcrissCode = acrissCode ?? string.Empty;
        }

        public static CarSpecification FromMakeAndModel(string makeName, string modelName)
        {
            if (makeName == null || modelName == null)
                throw new InvalidDomainValueException("invalid car make/model combination");
            
            var make = makeName.ToUpper();
            var model = modelName.ToUpper();
            
            if (!IsValidMakeAndModel(make, model))
                throw new InvalidDomainValueException("invalid car make/model combination");

            return new CarSpecification(make, model, acrissCode: string.Empty);
            
        }

        public static CarSpecification FromMakeModelAndAcriss(string makeName, string modelName, string acrissCode)
        {
            var makeModelSpec = FromMakeAndModel(makeName, modelName);

            var acode = (acrissCode ?? string.Empty).ToUpper();

            if (!IsValidAcrissCode(acode))
                throw new InvalidDomainValueException("invalid acriss code");

            // immutability / private set is only for the serializer
            return new CarSpecification(makeModelSpec.Make, makeModelSpec.Model, acode);

        }

        #endregion

        #region static validation used by factory methods
        private static Dictionary<string, List<string>> _makesAndModels = new Dictionary<string, List<string>>()
            {
                { "FSO", new List<string>() { "POLONEZ"  , "WARSZAWA" }
                },
                { "POLSKI FIAT", new List<string>() { "125P", "126P" } },
                { "FIAT", new List<string>() { "PUNTO", "PANDA", "DUCATO" } }
            };

        private static bool IsValidMakeAndModel(string make, string model) =>
            _makesAndModels.ContainsKey(make)
                 && _makesAndModels[make].Contains(model);

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
