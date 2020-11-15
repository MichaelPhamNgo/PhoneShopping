namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Category")]
    public partial class Category
    {
        public long Id { get; set; }

        [StringLength(250)]
        public string CategoryName { get; set; }

        [StringLength(500)]
        public string CategoryDescription { get; set; }

        public long? ParentId { get; set; }

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

        public int? DisplayOrder { get; set; }

        public bool? ShowOnHome { get; set; }

        public bool Status { get; set; }

        [StringLength(2)]
        public string Language { get; set; }
    }
}
