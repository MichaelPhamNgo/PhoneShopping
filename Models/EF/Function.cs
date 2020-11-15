namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Function")]
    public partial class Function
    {
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(100)]
        public string IconCss { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(50)]
        public string ParentId { get; set; }

        public int? SortOrder { get; set; }

        public bool? Status { get; set; }

        [StringLength(250)]
        public string URL { get; set; }
    }
}
