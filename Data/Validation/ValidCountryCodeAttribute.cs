using System.ComponentModel.DataAnnotations;

namespace WebAPI.Data.Validation
{
    public class ValidCountryCodeAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string code = value as string;

            return code.Length == 2;
        }
    }
}
