using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class NoticesController : Controller, INoticeHandler
    {
        private readonly NoticeService _noticeService;
        private readonly FileService _fileService;

        public NoticesController(NoticeService noticeService, FileService fileService)
        {
            _noticeService = noticeService;
            _fileService = fileService;
        }

        public async Task<ActionResponse> AddPost(Notice post, List<DBFile> dBFiles, List<IFormFile> formFiles = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (formFiles != null)
            {
                post.DBFiles = await _fileService.UploadFiles(formFiles);
            }
            var res = await _noticeService.AddUpdateNotice(post, userId);
            return res;
        }

        public async Task<ActionResponse> DeletePost(Notice post)
        {
            return new ActionResponse(await _noticeService.Delete(post.Id));
        }

        public async Task<Notice> GetNotice(int noticeId)
        {
            return await _noticeService.GetNotice(noticeId);
        }

        public async Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resp =  await _noticeService.GetRecentNotice(page, userId);
            return resp;
        }

        public async Task<List<Notice>> SearchNotice(string key)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _noticeService.SearchNotice(userId, key);
        }

        public async Task<List<Activity>> GetActivities(DateTime startTime, DateTime endTime)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _noticeService.GetActivitiesAsync(userId, startTime, endTime);
            return res;
        }

        public async Task<List<Notice>> GetUpcomingEvents(int page, PostType all)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var resp = await _noticeService.GetUpcomingEvents(userId);
            return resp;
        }

        public async Task<ActionResponse> UpdatePost(Notice post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var res = await _noticeService.AddUpdateNotice(post, userId);
            return res;
        }
    }
}