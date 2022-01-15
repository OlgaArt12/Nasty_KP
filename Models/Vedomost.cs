using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Uspevaemost.Models
{
    [Table("Vedomost")]
    [Index(nameof(Marknumb), Name = "ix_ved_mark")]
    [Index(nameof(IdZach), nameof(Marknumb), Name = "ix_ved_mark_avg")]
    public partial class Vedomost
    {
        [Key]
        [Column("IDVedomost")]
        public int Idvedomost { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Оценка:")]
        [StringLength(10)]
        public string Mark { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Предмет:")]
        [Column("NameSubjectID")]
        public int? NameSubjectId { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Студент:")]
        public int? IdZach { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Оценка (число):")]
        [Range(0, 5, ErrorMessage = "Вы вышли за диапозон!")]
        public int? Marknumb { get; set; }

        [ForeignKey(nameof(IdZach))]
        [InverseProperty(nameof(Student.Vedomosts))]
        [DisplayName("Студент:")]
        public virtual Student IdZachNavigation { get; set; }

        [ForeignKey(nameof(NameSubjectId))]
        [InverseProperty(nameof(Subject.Vedomosts))]
        [DisplayName("Предмет:")]
        public virtual Subject NameSubject { get; set; }
    }
}
