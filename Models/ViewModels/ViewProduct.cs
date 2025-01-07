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
    }

    public class Product
    {
        public int ProductId { get; set; }
        public string DiskId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public string ArtistNames { get; set; }
        public string ProductDescription { get; set; }
        public string Price { get; set; }
        public string Tracklist { get; set; }
        public string Status { get; set; } // For preorder or other statuses
        public int Years { get; set; }
    }
}