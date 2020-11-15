namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string Description { get; set; }
    }
}
