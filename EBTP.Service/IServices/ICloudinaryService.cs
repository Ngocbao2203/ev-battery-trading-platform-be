using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBTP.Service.IServices
{
    public interface ICloudinaryService
    {
        Task<ImageUploadResult> UploadProductImage(IFormFile file, string folder);
        Task<DeletionResult> DeleteImageAsync(string publicId);
    }
}
