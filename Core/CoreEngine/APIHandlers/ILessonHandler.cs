using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ILessonHandler
    {
        Task<List<Lesson>> GetUserLessons();
        Task<ActionResponse> AddLesson(int courseId, Lesson lesson);
        Task<ActionResponse> UpdateLesson(Lesson lesson);
        Task<List<Lesson>> GetCourseLessons(int courseId);
        Task<List<Lesson>> GetTodaysLessons();
        Task<ActionResponse> DeleteLesson(int lessonId, int courseId);
    }
}
