using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MuseMusic.Models.Tables;

namespace MuseMusic.Models.ManagerModels
{
     public partial class RecordPlayerViewModel
    {
        public List<Product> Products { get; set; }

        [Required]
        public Product SelectedProduct { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }

        public List<Brand> AllBrands { get; set; }

        public List<ImageUrl> AllImages { get; set; }
        public int SelectedImageId { get; set; }

    }

    public partial class AddVinylModel
    {
        [Required]
        public Product SelectedProduct { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }
    }

    public partial class EditVinylModel
    {
        [Required]
        public Product SelectedProduct { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }
    }

    public partial class ListRecordPlayerView
    {
        public string Id;
        public int? ProductId;
        public string ProductName;
        public string ProductDescription;
        public int? ProductQuantity;
        public decimal Price;
        public int? Years;
        public string BrandName;
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Price { get; set; }
    }
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
