namespace Models.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Product")]
    public partial class Product
    {
        public long Id { get; set; }

        [StringLength(10)]
        public string ProductCode { get; set; }

        [StringLength(250)]
        public string ProductName { get; set; }

        [StringLength(500)]
        public string ProductDescription { get; set; }

        [StringLength(50)]
        public string Image { get; set; }

        [Column(TypeName = "xml")]
        public string MoreImages { get; set; }

        public decimal? Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        public bool? IncludeVAT { get; set; }

        public int? Quantity { get; set; }

        public long? CategoryId { get; set; }

        [Column(TypeName = "ntext")]
        public string Detail { get; set; }

        public int? Warranty { get; set; }

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
    }
}
