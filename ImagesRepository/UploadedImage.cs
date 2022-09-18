using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace ImagesRepository
{
    public class UploadedImage
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }
        public string Password { get; set; }
        public int Views { get; set; }
    }
}
