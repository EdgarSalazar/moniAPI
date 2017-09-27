using Moni.Attributes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Moni.Models
{
    public abstract class ModelBase
    {
        public ModelBase()
        {
            Created = DateTime.Now; 
        }

        [Key]
        [DefaultField(0)]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
