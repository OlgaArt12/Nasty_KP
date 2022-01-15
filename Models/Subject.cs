using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Uspevaemost.Models
{
    [Table("Subject")]
    [Index(nameof(NameSubject), Name = "C_Subject_NameSubject", IsUnique = true)]
    public partial class Subject
    {
        public Subject()
        {
            StudPlans = new HashSet<StudPlan>();
            Vedomosts = new HashSet<Vedomost>();
        }

        [Key]
        [Column("IDNameSubject")]
        public int IdnameSubject { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Предмет:")]
        [StringLength(100)]
        public string NameSubject { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("ФИО преподавателя:")]
        [Column("FIOPrepod")]
        [StringLength(100)]
        public string Fioprepod { get; set; }

        [InverseProperty(nameof(StudPlan.NameSubjectNavigation))]
        public virtual ICollection<StudPlan> StudPlans { get; set; }
        [InverseProperty(nameof(Vedomost.NameSubject))]
        public virtual ICollection<Vedomost> Vedomosts { get; set; }
    }
}
