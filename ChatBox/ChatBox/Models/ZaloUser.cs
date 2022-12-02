namespace ChatBox.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ZaloUser")]
    public partial class ZaloUser
    {
        [Key]
        public int UniqueID { get; set; }

        [StringLength(50)]
        public string User_ID { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(500)]
        public string Avatar { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int? Gender { get; set; }
    }
}
