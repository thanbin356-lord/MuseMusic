using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MuseMusic.Models.Tables;

namespace MuseMusic.Models.ManagerModels
{
    public partial class VinylViewModel
    {
        public List<Product> Products { get; set; }

        [Required]
        public Product SelectedProduct { get; set; }
        public List<Artist> AllArtists { get; set; } // All artists for dropdown

        [Required]
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        public List<Mood> AllMoods { get; set; }

        [Required]
        public List<int> SelectedMood { get; set; }

        public List<Categories> AllCategories { get; set; } // All artists for dropdown

        [Required]
        public List<int> SelectedCategories { get; set; }

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
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        [Required]
        public List<int> SelectedMood { get; set; }

        [Required]
        public List<int> SelectedCategories { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }
    }

    public partial class EditVinylModel
    {
        [Required]
        public Product SelectedProduct { get; set; }

        [Required]
        public List<int> SelectedArtistIds { get; set; } // IDs of associated artists

        [Required]
        public List<int> SelectedMood { get; set; }

        [Required]
        public List<int> SelectedCategories { get; set; }

        [Required]
        public int SelectedBrandId { get; set; }
    }

    public partial class ListVinylViewModel
    {
        public string DiskId;
        public int? ProductId;
        public string ProductName;
        public string ProductDescription;
        public int? ProductQuantity;
        public decimal Price;
        public int? Years;
        public string MoodNames;
        public string CategoriesName;
        public string ArtistNames;
        public string BrandName;
    }


    public class Vinyl
    {
        public int Id { get; set; }
        public string DiskId { get; set; }
        public int? Years { get; set; }
        public string Tracklist { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } // New property
        public decimal Price { get; set; }

        public int ProductQuantity { get; set; }
        public string ProductDescription { get; set; }

        public List<Mood> Moods { get; set; }
        public List<Category> Categories { get; set; }
        public List<string> Artists { get; set; }  // This will hold artist names for the view
        public Product Product { get; set; }
        public int BrandId { get; internal set; }
    }
    public class Product
    {
        // public List<Vinyl> Vinyls { get; set; }
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductQuantity { get; set; }
        public decimal Price { get; set; }
        public int Years { get; set; }
        public string Tracklist { get; set; }
        public int SelectedBrandId{get;set;}
    }
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Mood
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Categories
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}