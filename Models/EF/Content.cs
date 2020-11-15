namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Content")]
    public partial class Content
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string ContentName { get; set; }

        [StringLength(500)]
        public string ContentDescription { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        public long? CategoryId { get; set; }

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

        public bool Status { get; set; }

        public DateTime? TopHot { get; set; }

        public int? ViewCount { get; set; }

        [StringLength(500)]
        public string Tags { get; set; }

        [StringLength(2)]
        public string Language { get; set; }
    }
}
