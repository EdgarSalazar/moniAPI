using Moni.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moni.Models
{
    public abstract class Balance : ModelBase, IValidatableObject
    {
        [Required]
        [DefaultField(1)]
        public string Name { get; set; }

        [Required]
        [DefaultField(2)]
        public decimal Value { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var className = GetType().Name;

            if(Value == 0)
            {
                yield return new ValidationResult
                  ("The value is required", new[] { className, "Value" });
            }

            if(CategoryId == 0)
            {
                yield return new ValidationResult
                  ("The category is required", new[] { className, "CategoryId" });
            }
        }
    }
}
