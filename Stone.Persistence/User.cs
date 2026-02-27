using Microsoft.AspNetCore.Identity;
using Stone.Entities.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Stone.Persistence
{
    public class User : IdentityUser
    {
        [StringLength(100)]
        public string FirstName { get; set; } = default!;
        [StringLength(100)]
        public string LastName { get; set; } = default!;
        [Column("persona_id")]
        public int? PersonId { get; set; }
        public virtual Person? Person { get; set; }
    }

    //public enum DocumentTypeEnum : short
    //{
    //    Dni,
    //    Passport
    //}
}
