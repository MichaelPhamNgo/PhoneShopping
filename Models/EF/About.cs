namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("About")]
    public partial class About
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string AboutName { get; set; }

        [StringLength(500)]
        public string AboutDescription { get; set; }

        [StringLength(250)]
        public string Image { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        [StringLength(250)]
        public string SeoKeywords { get; set; }

        [StringLength(250)]
        public string SeoAlias { get; set; }

        [StringLength(50)]
        public string MetaTitle { get; set; }

        [StringLength(250)]
        public string MetaDescription { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Guid? ModifiedBy { get; set; }

        public bool? Status { get; set; }
    }
}
