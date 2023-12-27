using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace POS_System_DotNet.Models.Product
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

       
        public string Barcode { get; set; }

        public string ProductName { get; set; }
        public double ImportPrice { get; set; }
        public double RetailPrice { get; set; }
        public string Category { get; set; }
        public DateTime CreationDate { get; set; }

        [Column(TypeName = "varbinary(max)")]
        public byte[] Picture { get; set; }
    }
}
