namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("State")]
    public partial class State
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }

        [StringLength(50)]
        public string StateName { get; set; }

        [StringLength(250)]
        public string StateDescription { get; set; }

        public DateTime? CreatedDate { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public long? ModifiedBy { get; set; }

        public bool? Status { get; set; }
    }
}
