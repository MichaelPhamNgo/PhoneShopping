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
        public int Id { get; set; }

        public bool? CanCreate { get; set; }

        public bool? CanDelete { get; set; }

        public bool? CanRead { get; set; }

        public bool? CanUpdate { get; set; }

        [StringLength(50)]
        public string FunctionId { get; set; }

        public Guid? RoleId { get; set; }
    }
}
