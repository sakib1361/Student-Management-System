using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.AppServices;
using Student.Infrastructure.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class NoticeService : BaseService
    {
        private readonly StudentDBContext _db;
        private readonly INotificationService _notificationService;

        public NoticeService(StudentDBContext studentDBContext, INotificationService notificationService)
        {
            _db = studentDBContext;
            _notificationService = notificationService;
        }

        public async Task<int> GetTotalNoticeAsync()
        {
            return await _db.Notices.CountAsync();
        }

        public async Task<ActionResponse> AddUpdateNotice(Notice post, string userId)
        {
            var dbUser = await _db.DBUsers
                                  .Include(m => m.Batch)
                                  .FirstOrDefaultAsync(x => x.Id == userId);
            if (dbUser == null)
            {
                return new ActionResponse(false, "Invalid User");
            }
            else
            {
                post.Owner = dbUser;
                if (dbUser.Role == AppConstants.Student)
                {
                    post.Batch = dbUser.Batch;
                }
                if (post.Id == 0)
                {
                    _db.Entry(post).State = EntityState.Added;
                }
                else
                {
                    _db.Entry(post).State = EntityState.Modified;
                }
                try
                {
                    await _db.SaveChangesAsync();
                    if (post.Batch != null)
                    {
                        _notificationService.SendNotification(post.Batch.Name, post.Title, post.Message);
                    }
                    else
                    {
                        var currentBatch = await _db.Semesters
                                                    .Where(x => x.EndsOn > DateTime.Now)
                                                    .Select(m => m.Batch)
                                                    .Distinct()
                                                    .ToListAsync();
                        foreach (var batch in currentBatch)
                        {
                            _notificationService.SendNotification(batch.Name, post.Title, post.Message);
                        }
                    }
                    return new ActionResponse(true, "Successfully created the notice");
                }
                catch (Exception ex)
                {
                    return new ActionResponse(false, ex.Message);
                }
            }
        }

        public async Task<List<Notice>> SearchNotice(string userId, string key)
        {
            key = key.ToUpper();
            var user = await _db.DBUsers
                               .Include(x => x.Batch)
                               .FirstOrDefaultAsync(x => x.Id == userId);
            var primaryQuery = _db.Notices
                                  .Include(x=>x.DBFiles)
                                  .Where(x => EF.Functions.Like(x.Title, $"%{key}%") ||
                                               EF.Functions.Like(x.Message, $"%{key}%") ||
                                               x.Batch.Name == key)
                                  .Take(40);
            if (user.Role == AppConstants.Admin)
            {
                return await primaryQuery.ToListAsync();
            }
            else
            {
                return await primaryQuery.Where(x => x.Batch == null || x.Batch == user.Batch)
                                         .ToListAsync();
            }
        }

        public async Task<List<Activity>> GetActivitiesAsync(string userId, DateTime start, DateTime end)
        {
            var activityList = new List<Activity>();
            var user = await _db.DBUsers
                                .Include(x => x.Batch)
                                .FirstOrDefaultAsync(x => x.Id == userId);
            var notices = await _db.Notices.Where(x => x.EventDate >= start && x.EventDate <= end &&
                                                     (x.Batch == null || x.Batch == user.Batch))
                                   .ToListAsync();

            var lessons = await _db.Lessons.Include(x => x.Course)
                                           .Where(m => m.Course.StudentCourses.Any(n => n.Student.Id == userId) &&
                                                       m.Course.Semester.StartsOn.Date <= start &&
                                                       m.Course.Semester.EndsOn.Date >= end)
                                           .ToListAsync();
            var workItems = await _db.ToDoItems
                                     .Include(x => x.Participents)
                                     .Where(m => (m.EventTime >= start && m.EventTime <= end) &&
                                                  m.Participents.Any(n => n.DBUser.Id == userId))
                                     .ToListAsync();

            var day = start.Date;
            while (day < end)
            {
                var todayLesson = lessons.Where(x => x.DayOfWeek == day.DayOfWeek);
                foreach (var lesson in todayLesson)
                {
                    var activity = new Activity()
                    {
                        Id = lesson.Id,
                        DateTime = day,
                        Description = "Regular Class on " + lesson.Course.CourseName,
                        ActivityType = ActivityType.Lesson,
                        Name = lesson.Course.CourseName,
                        DayOfWeek = day.DayOfWeek,
                        TimeOfDay = lesson.TimeOfLesson
                    };
                    activityList.Add(activity);
                }
                var todayNotices = notices.Where(x => x.EventDate.Date == day);
                foreach (var notice in todayNotices)
                {
                    var activity = new Activity()
                    {
                        ActivityType = ActivityType.Notice,
                        DateTime = notice.EventDate,
                        DayOfWeek = day.DayOfWeek,
                        Description = notice.Message,
                        Id = notice.Id,
                        Name = notice.Title,
                        TimeOfDay = notice.TimeOfEvent
                    };
                    activityList.Add(activity);
                }
                var todayWorks = workItems.Where(x => x.EventTime.Date == day);
                foreach (var todoItem in todayWorks)
                {
                    var activity = new Activity()
                    {
                        ActivityType = ActivityType.Todo,
                        DateTime = todoItem.EventTime,
                        DayOfWeek = day.DayOfWeek,
                        Description = todoItem.Message,
                        Id = todoItem.Id,
                        Name = todoItem.Title,
                        TimeOfDay = todoItem.EventTime.ToString("hh:mm tt")
                    };

                    activityList.Add(activity);
                }
                day = day.AddDays(1);
            }


            return activityList;

        }

        public async Task<bool> UpdateNotice(Notice notice)
        {
            var dbNotice = await _db.Notices.FindAsync(notice);
            if (dbNotice == null)
            {
                return false;
            }
            else
            {
                _db.Entry(notice).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return true;
            }
        }



        public async Task<List<Notice>> GetUpcomingEvents(string userId)
        {
            var user = await _db.DBUsers
                               .Include(x => x.Batch)
                               .FirstOrDefaultAsync(x => x.Id == userId);
            var nextWeek = CurrentTime.AddMonths(1);
            var primaryQuery = _db.Notices
                                  .Include(x=>x.DBFiles)
                                  .Where(x => x.EventDate > CurrentTime &&
                                              x.EventDate <= nextWeek)
                                  .Include(m => m.Batch)
                                  .OrderBy(x => x.EventDate);
            if (user.Role == AppConstants.Admin)
            {
                var notices = await primaryQuery.ToListAsync();
                return notices;
            }
            else
            {
                var notices = await primaryQuery.Where(x => x.Batch == null || x.Batch == user.Batch)
                                                .ToListAsync();
                return notices;
            }
        }

        public async Task<List<Notice>> GetRecentNotice(int page, string userId, int itemPerPage = 30)
        {
            var user = await _db.DBUsers
                              .Include(x => x.Batch)
                              .FirstOrDefaultAsync(x => x.Id == userId);

            var primaryQuery = _db.Notices
                                  .Include(x=>x.DBFiles)
                                  .OrderByDescending(m => m.EventDate)
                                  .Include(m => m.Batch);
            if (user.Role == AppConstants.Admin)
            {
                return await primaryQuery.Skip(page * itemPerPage)
                                         .Take(itemPerPage)
                                         .ToListAsync();
            }
            else
            {
                var res = await primaryQuery.Where(x => x.Batch == null || x.Batch.Id == user.Batch.Id)
                                         .Skip(page * itemPerPage)
                                         .Take(itemPerPage)
                                         .ToListAsync();
                return res;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var notice = await _db.Notices.Include(x => x.Batch)
                                          .FirstOrDefaultAsync(x => x.Id == id);
            if (notice == null)
            {
                return false;
            }
            else
            {
                _db.Entry(notice).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return true;
            }
        }

        public async Task<Notice> GetNotice(int id)
        {
            var notice = await _db.Notices
                                  .Include(x => x.Course)
                                  .Include(x => x.Batch)
                                  .Include(x => x.DBFiles)
                                  .Include(x => x.Owner)
                                  .FirstOrDefaultAsync(x => x.Id == id);
            return notice;
        }
    }
}
