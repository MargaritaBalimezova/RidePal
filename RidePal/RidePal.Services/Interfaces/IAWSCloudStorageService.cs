using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RidePal.Services.Interfaces
{
    public interface  IAWSCloudStorageService
    {
        Task Upload(IFormFile file);
    }
}
