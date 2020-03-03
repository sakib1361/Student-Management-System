using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class LessonsController : ControllerBase, ILessonHandler
    {
        private readonly LessonService _lessonService;
        private readonly UserService _userService;

        public LessonsController(LessonService lessonService, UserService userService)
        {
            _lessonService = lessonService;
            _userService = userService;
        }
        public async Task<ActionResponse> AddLesson(int courseId, Lesson lesson)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var access = await _userService.AuthorizeCourse(userId, courseId);
            if (access)
            {
                var res = await _lessonService.AddLesson(courseId, lesson);
                return new ActionResponse(res != null);
            }
            else
            {
                return new ActionResponse(false, "You do not have right to add lesson here");
            }
        }

        public async Task<List<Lesson>> GetUserLessons()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return await _lessonService.GetLesson(userId);
        }

        public async Task<List<Lesson>> GetCourseLessons(int courseId)
        {
            return await _lessonService.GetCourseLesson(courseId);
        }

        public async Task<ActionResponse> UpdateLesson(Lesson lesson)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var lessonAccess = await _userService.AuthorizeLesson(userId, lesson.Id);

            if (lessonAccess)
            {
                return await _lessonService.UpdateLesson(lesson);
            }
            else
            {
                return new ActionResponse(false, "Invalid User access");
            }
        }

        public Task<List<Lesson>> GetTodaysLessons()
        {
            return _lessonService.UpcomingLessons();
        }

        public async Task<ActionResponse> DeleteLesson(int lessonId, int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var access = await _userService.AuthorizeCourse(userId, courseId);
            if (access)
            {
                return await _lessonService.DeleteLesson(lessonId, courseId);
            }
            else
            {
                return new ActionResponse(false, "Invalid Lesson Information");
            }
        }
    }
}