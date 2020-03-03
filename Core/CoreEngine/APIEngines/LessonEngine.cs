using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    internal class LessonEngine : BaseApiEngine, ILessonHandler
    {
        private const string Controller = "Lessons";
        public LessonEngine(HttpWorker httpWorker) : base(httpWorker, Controller)
        {
        }

        public Task<ActionResponse> AddLesson(int courseId, Lesson lesson)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { courseId, lesson });
        }

        public Task<ActionResponse> DeleteLesson(int lessonId, int courseId)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, new { lessonId, courseId });
        }

        public Task<List<Lesson>> GetCourseLessons(int courseId)
        {
            return SendRequest<List<Lesson>>(HttpMethod.Get, new { courseId });
        }

        public Task<List<Lesson>> GetUserLessons()
        {
            return SendRequest<List<Lesson>>(HttpMethod.Get, null);
        }

        public Task<List<Lesson>> GetLessons(DateTime dateTime)
        {
            return SendRequest<List<Lesson>>(HttpMethod.Post, dateTime);
        }

        public Task<List<Lesson>> GetTodaysLessons()
        {
            return SendRequest<List<Lesson>>(HttpMethod.Get, null);
        }

        public Task<ActionResponse> UpdateLesson(Lesson lesson)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, lesson);
        }
    }
}
