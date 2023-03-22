using IValidatebleObjectDemo;
using System.ComponentModel.DataAnnotations;

var result = new List<ValidationResult>();
bool validateAllProperties = false;
bool isValid = false;

var employee = new Employee()
{
    Id = 1,
    Name = "Mahedee Hasan",
    Status = true,
    DesignationId = 1
};


isValid = Validator.TryValidateObject(employee, new ValidationContext(employee, null, null), 
    result, validateAllProperties);

var employee2 = new Employee()
{
    Id = -1,
    Name = null,
    Status = true,
    DesignationId = 1
};

isValid = Validator.TryValidateObject(employee2, new ValidationContext(employee2, null, null), 
    result, validateAllProperties);   