using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Moni.Models
{
    public class Category : ModelBase, IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        public ICollection<Category> SubCategories { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var className = GetType().Name;

            if (ParentId == 0)
            {
                yield return new ValidationResult
                  ("The ParentId can't be 0", new[] { className, "ParentId" });
            }

            if(Parent.Parent != null)
            {
                yield return new ValidationResult
                  ("Categories can't go deeper than 2 levels", new[] { className, "ParentId" });
            }
        }
    }
}
