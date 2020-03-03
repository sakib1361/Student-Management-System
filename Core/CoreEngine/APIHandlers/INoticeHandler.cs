using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface INoticeHandler
    {
        Task<List<Notice>> GetPosts(int page, PostType postType = PostType.All);
        Task<ActionResponse> AddPost(Notice notice, List<DBFile> dBFiles, List<IFormFile> formFiles = null);
        Task<ActionResponse> UpdatePost(Notice notice);
        Task<ActionResponse> DeletePost(Notice notice);
        Task<List<Notice>> GetUpcomingEvents(int page, PostType all);
        Task<List<Activity>> GetActivities(DateTime startTime, DateTime endTime);
        Task<Notice> GetNotice(int noticeId);
        Task<List<Notice>> SearchNotice(string key);
    }
}
