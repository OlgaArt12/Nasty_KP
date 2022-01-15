using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Uspevaemost.Models
{
    [Table("Academgroup")]
    public partial class Academgroup
    {
        public Academgroup()
        {
            StudPlans = new HashSet<StudPlan>();
            Students = new HashSet<Student>();
        }

        [Key]
        [Column("GroupID")]
        public int GroupId { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Шифр:")]
        [StringLength(4)]
        public string Shifr { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Год поступления:")]
        [Range(14, 21, ErrorMessage = "Вы вышли за диапозон!")]
        public int Godpostup { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Номер группы:")]
        [Range(1, 6, ErrorMessage = "Вы вышли за диапозон!")]
        public int Number { get; set; }

        [InverseProperty(nameof(StudPlan.IdGroupNavigation))]
        public virtual ICollection<StudPlan> StudPlans { get; set; }
        [InverseProperty(nameof(Student.IdgroupNavigation))]
        public virtual ICollection<Student> Students { get; set; }
    }
}
