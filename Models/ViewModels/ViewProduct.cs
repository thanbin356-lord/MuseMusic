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
        public List<Models.ViewModels.Product> Products{ get; set; }
        public Product SelectedProduct { get; set; }
        public List<Artist> AllArtists { get;  set; }
        public List<Mood> AllMoods { get; set; }
        public List<Brand> AllBrands { get; set; }
        public List<string> SelectedArtistNames { get; set; }
        public List<string> SelectedMoodNames { get; set; }
        public List<Category> AllCategories { get; set; }
        public List<string> SelectedCategoryNames { get; set; }

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
        public string SelectedBrandName { get;set; }
        public List<string> ImageUrls { get; set; }
        public string? PrimaryImageUrl { get; set; }
    }
}