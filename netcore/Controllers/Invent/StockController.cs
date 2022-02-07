﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

using WMS.Data;
using WMS.Models.Invent;

namespace WMS.Controllers.Invent
{
    [Authorize(Roles = "Stock")]
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}



namespace WMS.MVC
{
    public static partial class Pages
    {
        public static class Stock
        {
            public const string Controller = "Stock";
            public const string Action = "Index";
            public const string Role = "Stock";
            public const string Url = "/Stock/Index";
            public const string Name = "Stock";
        }
    }
}
namespace WMS.Models
{
    public partial class ApplicationUser
    {
        [Display(Name = "Stock")]
        public bool StockRole { get; set; } = false;
    }
}



