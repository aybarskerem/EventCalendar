namespace EventCalendar
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Table")]
    public partial class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int EventID { get; set; }

        [Required]
        [StringLength(30)]
        public string Subject { get; set; }

        [StringLength(300)]
        public string Description { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime Start { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime End { get; set; }

        [StringLength(10)]
        public string EventColor { get; set; }
    }
}
