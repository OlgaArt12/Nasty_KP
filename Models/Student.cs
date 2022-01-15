using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Uspevaemost.Models
{
    [Table("Student")]
    [Index(nameof(Zachetka), Name = "C_Student_Zachetka", IsUnique = true)]
    [Index(nameof(Fio), Name = "ix_stud_fio")]
    public partial class Student
    {
        public Student()
        {
            Vedomosts = new HashSet<Vedomost>();
        }

        [Key]
        [Column("ZachetkaID")]
        public int ZachetkaId { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Номер зачетки:")]
        [Range(18000000, 22000000, ErrorMessage = "Вы вышли за диапозон!")]
        public int Zachetka { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("ФИО:")]
        [Column("FIO")]
        [StringLength(100)]
        public string Fio { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Дата рождения:")]
        [DataType(DataType.Date, ErrorMessage = "Некорректный ввод")]
        [Column(TypeName = "date")]
        public DateTime DateStud { get; set; }

        [Required(ErrorMessage = "Поле является обязательным")]
        [DisplayName("Номер группы:")]
        [Column("IDGroup")]
        public int? Idgroup { get; set; }

        [ForeignKey(nameof(Idgroup))]
        [InverseProperty(nameof(Academgroup.Students))]
        [DisplayName("Номер группы:")]
        public virtual Academgroup IdgroupNavigation { get; set; }

        [InverseProperty(nameof(Vedomost.IdZachNavigation))]
        public virtual ICollection<Vedomost> Vedomosts { get; set; }
    }
}
