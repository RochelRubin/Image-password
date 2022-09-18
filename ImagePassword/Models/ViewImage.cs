using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using  ImagesRepository;


namespace ImagePassword.Models
{
    public class ViewImage
    {
        public UploadedImage Image { get; set; }
        public int Id { get; set; }
        public string Message { get; set; }
        public bool SubmittedCorrectPassword { get; set; }
        public bool AlreadyHasAccess { get; set; }
    }
}
