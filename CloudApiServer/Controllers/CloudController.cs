using CloudApiServer.Models;
using DataToCommunicate;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CloudApiServer.Controllers
{
    //[Authorize]
    public class CloudController : ApiController
    {
        readonly CloudDbContext db = new CloudDbContext();
        [AllowAnonymous]
        public void SignUp([FromBody] RegistrationData data)
        {
            if (String.IsNullOrEmpty(data.UserName) || String.IsNullOrEmpty(data.UserName))
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            UserManager<User> userManager = new UserManager<User>(new UserStore<User>(db));

            User user = new User() { UserName = data.UserName };

            IdentityResult result = userManager.CreateAsync(user, data.Password).Result;

            if (!result.Succeeded)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        public IEnumerable<DirectoryData> GetDirectories()
        {
            string Id = User.Identity.GetUserId();

            IEnumerable<DirectoryData> result = db.Directories.Where(d => d.OwnerId.ToString() == Id).Select(d => new DirectoryData
            {
                Id = d.Id,
                Name = d.Name,
                ParentDirectoryId = d.ParentDirectoryId,
                FilesCount = d.Files.Count
            });

            return result;
        }

        [HttpPost]
        public IEnumerable<FileData> GetFiles([FromBody]Guid parentDirectoryId)
        {
            return db.Files.Where(f => f.ParentDirectoryId == parentDirectoryId).Select(f => new FileData
            {
                Id = f.Id,
                Name = f.Name,
            });
        }

        public Guid CreateDirectory(CreateDirectoryData createDirectoryData)
        {
            Models.Directory directory = new Models.Directory()
            {
                Id = Guid.NewGuid(),
                Name = createDirectoryData.Name,
                OwnerId = new Guid(User.Identity.GetUserId()),
                ParentDirectoryId = createDirectoryData.ParentDirectoryId,
            };
            db.Directories.Add(directory);
            db.SaveChanges();
            return directory.Id;
        }

        public async Task<Guid> UploadFile([FromUri] string fileName, [FromUri] Guid? parentDirectoryId)
        {
            Models.File file = new Models.File()
            {
                Id = Guid.NewGuid(),
                Name = fileName,
                OwnerId = new Guid(User.Identity.GetUserId()),
                ParentDirectoryId = parentDirectoryId,
            };
            db.Files.Add(file);
            db.SaveChanges();

            using (FileStream fs = new FileStream(@"D:\Education 5 2018\ТРПЗ\ServerData\" + file.Id + "Server" + fileName, FileMode.CreateNew))
                await Request.Content.CopyToAsync(fs);
            return file.Id;
        }

        [HttpPost]
        public HttpResponseMessage DownloadFile([FromBody] Guid fileId)
        {
            string fileName = db.Files.First(f=>f.Id == fileId).Name;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StreamContent(new FileStream(@"D:\Education 5 2018\ТРПЗ\ServerData\" + fileId + "Server" + fileName, FileMode.Open));
            return response;
        }
    }
}
