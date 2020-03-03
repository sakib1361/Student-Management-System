using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.DBModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class FileService : BaseService
    {
        private readonly StudentDBContext _db;
        private const string UploadFolder = "Uploads";

        public FileService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
        }

        public async Task<List<DBFile>> UploadFiles(IEnumerable<IFormFile> formFiles)
        {
            var files = new List<DBFile>();
            foreach (var file in formFiles)
            {
                files.Add(await UploadFile(file));
            }
            return files;
        }

        private string GetTargetFolder()
        {
            var dir = AppConstants.DataPath;
            var target = Path.Combine(dir, UploadFolder);
            Directory.CreateDirectory(target);
            return target;
        }

        public async Task<DBFile> UploadFile(IFormFile formFile)
        {
            var target = GetTargetFolder();

            var filePath = Path.GetTempFileName();

            using (var stream = File.Create(filePath))
            {
                await formFile.CopyToAsync(stream);
            }


            var actualFileName = Path.GetFileName(filePath);
            var targetFile = Path.Combine(target, actualFileName);

            if (!string.IsNullOrEmpty(formFile.FileName))
            {
                actualFileName = formFile.FileName;
                Path.ChangeExtension(targetFile, actualFileName);
            }

            var hash = HashFile(filePath);

            var oldFile = await _db.DBFiles.FirstOrDefaultAsync(x => x.FileHash == hash);
            if (oldFile == null)
            {
                oldFile = new DBFile()
                {
                    Id = Guid.NewGuid().ToString().Replace("-", ""),
                    FileHash = hash,
                    FileName = actualFileName,
                    FilePath = Path.GetFileName(targetFile)
                };
                File.Copy(filePath, targetFile);
                _db.DBFiles.Add(oldFile);
                await _db.SaveChangesAsync();
            }
            File.Delete(filePath);
            return oldFile;
        }

        public async Task<DBFile> GetFile(string id)
        {
            var file = await _db.DBFiles.FindAsync(id);
            if (file == null)
            {
                return null;
            }
            else
            {
                var target = GetTargetFolder();
                file.FilePath = Path.Combine(target, file.FilePath);
                return file;
            }
        }

        private string HashFile(string fileName)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(fileName);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }

        public async Task<ActionResponse> Delete(DBFile obj)
        {
            var target = GetTargetFolder();

            var _dbFile = await _db.DBFiles.FindAsync(obj.Id);

            if (_dbFile != null)
            {
                _db.Entry(_dbFile).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                var targetFile = Path.Combine(target, _dbFile.FilePath);
                if (_dbFile != null && File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                    return new ActionResponse(true, "File Delete Successfull");
                }
            }
            return new ActionResponse(false, "Failed to Locate the target file");
        }
    }
}
