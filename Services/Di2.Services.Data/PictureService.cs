using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Di2.Data.Common.Repositories;
using Di2.Data.Models;
using Di2.Data;
using Microsoft.AspNetCore.Http;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Di2.Services.Mapping;

namespace Di2.Services.Data
{
    public class PictureService : IPictureService
    {

        private readonly IDeletableEntityRepository<Picture> picturesRepository;
        private readonly Cloudinary cloudinary;

        public PictureService(
            IDeletableEntityRepository<Picture> picturesRepository,
            Cloudinary cloudinary)
        {
            this.picturesRepository = picturesRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<IEnumerable<UploadResult>> Upload(ICollection<IFormFile> pictures, string orderId)
        {
            var uploadResults = new ConcurrentBag<ImageUploadResult>();
            Parallel.ForEach(pictures, (picture) =>
            {
                var guid = Guid.NewGuid().ToString();
                var uploadParams = new ImageUploadParams
                {
                    PublicId = guid,
                    File = new FileDescription(guid, picture.OpenReadStream()),
                    Folder = $"{orderId}",
                };
                var uploadResult = this.cloudinary.UploadLarge(uploadParams);
                uploadResults.Add(uploadResult);
            });

            foreach (var picture in uploadResults)
            {
                var pictureToAdd = new Picture
                {
                    Id = picture.PublicId.Substring(picture.PublicId.LastIndexOf('/') + 1),
                    OrderId = orderId,
                    Url = picture.SecureUri.AbsoluteUri,
                };
                await this.picturesRepository.AddAsync(pictureToAdd);
                await this.picturesRepository.SaveChangesAsync();
            }
            return uploadResults;
        }

        public void Delete(string orderId)
            => this.cloudinary.DeleteResourcesByPrefix($"{orderId}/");

        

        public async Task<T> GetPictureById<T>(string pictureId)
            => await this.picturesRepository
                .All()
                .Where(p => p.Id == pictureId)
                .To<T>()
                .FirstOrDefaultAsync();

        public async Task Delete(string orderId, string pictureId)
        {
            this.cloudinary.DeleteResourcesByPrefix($"{orderId}/{pictureId}");

            var pictureToRemove = this.picturesRepository
                .All()
                .Where(x=>x.Id==pictureId).FirstOrDefault();

            if (pictureToRemove == null)
            {
                return;
            }

            this.picturesRepository.Delete(pictureToRemove);
            await this.picturesRepository.SaveChangesAsync();
        }
    }
}
