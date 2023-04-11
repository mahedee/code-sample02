using System.ComponentModel.DataAnnotations;

namespace IValidatebleObjectDemo
{
    public class Employee : IValidatableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
        public int DesignationId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(Id <= 0)
            {
                yield return new ValidationResult("Employee Id is a required filed!", new[] {"Id"});
            }

            if (String.IsNullOrEmpty(Name))
            {
                yield return new ValidationResult("Employee Name is a required field!", new[] { "Name" });
            }
        }
    }
}
