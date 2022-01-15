using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Uspevaemost.Models
{
    [Table("StudPlan")]
    public partial class StudPlan
    {
        [Key]
        [Column("PlanID")]
        public int PlanId { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Часы:")]
        [Range(10, 200, ErrorMessage = "Вы вышли за диапозон!")]
        public int Clock { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Вид аттестации:")]
        public string Attestation { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Предмет:")]
        public int? NameSubject { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Номер группы:")]
        public int? IdGroup { get; set; }

        [ForeignKey(nameof(IdGroup))]
        [InverseProperty(nameof(Academgroup.StudPlans))]
        [DisplayName("Номер группы:")]
        public virtual Academgroup IdGroupNavigation { get; set; }

        [ForeignKey(nameof(NameSubject))]
        [InverseProperty(nameof(Subject.StudPlans))]
        [DisplayName("Предмет:")]
        public virtual Subject NameSubjectNavigation { get; set; }
    }
}
