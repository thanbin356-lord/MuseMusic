using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MuseMusic.Models.Tables;

namespace MuseMusic.Models.ViewModels
{
    public class VinylViewModel
    {
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
        public List<Artist> AllArtists { get; set; }
        public List<Mood> AllMoods { get; set; }
        public List<Brand> AllBrands { get; set; }
        public List<string> SelectedArtistNames { get; set; }
        public List<string> SelectedMoodNames { get; set; }
        public List<Category> AllCategories { get; set; }
        public List<string> SelectedCategoryNames { get; set; }
        public List<int> SelectedCategoryIds { get; set; } // Thêm thuộc tính này
        public List<int> SelectedArtistIds { get; set; } // Thêm thuộc tính này
        public List<int> SelectedMoodIds { get; set; } // Thêm thuộc tính này
        public List<string> SelectedEraIds { get; set; } // Added property for selected eras
        public List<string> AllEras { get; set; } // Added property to list all available eras
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);
         public List<string> MusicFiles { get; set; } 
        public VinylViewModel()
        {
            AllEras = new List<string> { "1980s", "1990s", "2000s", "2010s", "2020s" }; // Predefined list of eras
        }
    }

    public class RecordPlayerViewModel()
    {
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
        public List<Brand> AllBrands { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);

}
public class AccessoriesViewModel()
    {
        public List<Product> Products { get; set; }
        public Product SelectedProduct { get; set; }
        public List<Brand> AllBrands { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalProducts { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalProducts / PageSize);

}
public class Product
{
    public int ProductId { get; set; }
    public string DiskId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public string ArtistNames { get; set; }
    public string ProductDescription { get; set; }
    public decimal Price { get; set; }
    public string Tracklist { get; set; }
    public string Status { get; set; } // For preorder or other statuses
    public int Years { get; set; }
    public string CategoriesNames { get; set; }
    public string MoodNames { get; set; }
    public int? ProductQuantity { get; set; }
    public string SelectedBrandName { get; set; }
    public List<string> ImageUrls { get; set; }
    public string? PrimaryImageUrl { get; set; }
    public string ArtistImageUrl { get; set; }
    public Vinyl Vinyl { get; set; }
    public RecordPlayer RecordPlayer { get; set; }
}
public class Vinyl
{
    public int VinylId { get; set; }
    public string DiskId { get; set; }
    public int ProductId { get; set; }

    // Navigation properties to related entities
    public List<CategoriesVinyl> CategoriesVinyls { get; set; }
    public List<ArtistVinyl> ArtistVinyls { get; set; }
    public List<MoodVinyl> MoodVinyls { get; set; }

}
public class RecordPlayer
{
    public string Motor { get; set; }
    public string Speed { get; set; }
}
}