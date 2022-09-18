using ImagePassword.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ImagesRepository;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace ImagePassword.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=Pictures; Integrated Security=true";
        private readonly IWebHostEnvironment _webHostEnvironment;
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Upload(IFormFile imageFile, string password)
        {

            string fileName = $"{Guid.NewGuid()}-{imageFile.FileName}";
            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            imageFile.CopyTo(fs);

            var repo = new ImageRepository(_connectionString);
            var id = repo.Add(filePath, password, fileName);
            var imagePassword = repo.GetImagePasswordById(id);
            ViewModel vm = new ViewModel
            {
                Id = id,
                Password = imagePassword
            };
            return View(vm);
        }
        public IActionResult EnterPassword(int id,string password)

        {
            
            var vi = new ViewImage();
            vi.Id = id;
            List<int> IdList = HttpContext.Session.Get<List<int>>("IdList");
            if (IdList == null)
            {
                IdList = new List<int>();
            }
            if (IdList.Contains(id))
            {
                vi.AlreadyHasAccess = true;
            }


            if (password != null || vi.AlreadyHasAccess)
            {
                var repo = new ImageRepository(_connectionString);
                var correctpassword = repo.GetImagePasswordById(id);

                if (correctpassword == password || vi.AlreadyHasAccess)
                {
                    IdList.Add(id);
                    HttpContext.Session.Set("IdList", IdList);

                    repo.IncrementView(id);
                    UploadedImage image = repo.GetimageById(id);
                    vi.Image = image;
                    vi.SubmittedCorrectPassword = true;
                }

                else
                {
                    TempData["Message"] = "Incorrect";
                    vi.Message = (string)TempData["Message"];
                    vi.SubmittedCorrectPassword = false;

                }
            }
            else
            {
                vi.SubmittedCorrectPassword = false;
            }

            return View(vi);

        }

    }
}
