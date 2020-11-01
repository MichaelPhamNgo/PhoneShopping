namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [Key]
        public long Id { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(250)]
        public string Password { get; set; }

        [StringLength(50)]
        public string SecurityStamp { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Dob { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public bool? EmailConfirm { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthday { get; set; }

        [Column("Address 1")]
        [StringLength(250)]
        public string Address_1 { get; set; }

        [Column("Address 2")]
        [StringLength(250)]
        public string Address_2 { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        [StringLength(50)]
        public string ZipCode { get; set; }

        public long? StateId { get; set; }

        public DateTime? RegisteredDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool? Status { get; set; }

        [StringLength(20)]
        public string GroupId { get; set; }
    }
}
