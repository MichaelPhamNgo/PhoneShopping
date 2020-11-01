namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemConfig")]
    public partial class SystemConfig
    {
        [StringLength(50)]
        public string Id { get; set; }

        [StringLength(50)]
        public string SystemConfigName { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        [StringLength(250)]
        public string Value { get; set; }

        public bool Status { get; set; }
    }
}
