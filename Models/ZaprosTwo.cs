using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Uspevaemost.Models
{
    [Keyless]
    public partial class ZaprosTwo
    {
        [Required]
        [Column("FIO")]
        [StringLength(100)]
        public string Fio { get; set; }
        [StringLength(10)]
        public string Mark { get; set; }
        [StringLength(4)]
        public string Shifr { get; set; }
        public int? Godpostup { get; set; }
        public int? Number { get; set; }
        [StringLength(100)]
        public string NameSubject { get; set; }
        public int? Debtor { get; set; }
    }
}
