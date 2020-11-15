namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Permission")]
    public partial class Permission
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool CanCreate { get; set; }

        [Key]
        [Column(Order = 2)]
        public bool CanDelete { get; set; }

        [Key]
        [Column(Order = 3)]
        public bool CanRead { get; set; }

        [Key]
        [Column(Order = 4)]
        public bool CanUpdate { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string FunctionId { get; set; }

        [Key]
        [Column(Order = 6)]
        public Guid RoleId { get; set; }
    }
}
