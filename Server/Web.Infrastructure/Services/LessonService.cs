using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.DBModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class LessonService : BaseService
    {
        private readonly StudentDBContext _db;
        public LessonService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
        }

        public async Task<Lesson> AddLesson(int courseId, Lesson lesson)
        {
            var course = await _db.Courses
                                  .FirstOrDefaultAsync(x => x.Id == courseId);
            if (course == null)
            {
                return null;
            }
            else
            {
                course.Lessons.Add(lesson);
                await _db.SaveChangesAsync();
                return lesson;
            }
        }

        public async Task<List<Lesson>> GetLesson(string userId)
        {
            var lessons = await _db.Lessons.Include(x => x.Course)
                                           .Where(m => m.Course.StudentCourses.Any(n => n.Student.Id == userId) &&
                                                       m.Course.Semester.StartsOn <= CurrentTime &&
                                                       m.Course.Semester.EndsOn >= CurrentTime)
                                           .ToListAsync();
            return lessons;
        }


        public async Task<List<Lesson>> GetCourseLesson(int courseId)
        {
            return await _db.Lessons.Where(x => x.Course.Id == courseId)
                                    .ToListAsync();
        }

        public async Task<ActionResponse> UpdateLesson(Lesson lesson)
        {
            var dbLesson = await _db.Lessons.FirstOrDefaultAsync(x => x.Id == lesson.Id);
            if (dbLesson == null)
            {
                return new ActionResponse(false, "Invalid Lesson");
            }
            else
            {
                _db.Entry(dbLesson).State = EntityState.Detached;
                _db.Entry(lesson).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return new ActionResponse(true);
            }
        }

        public async Task<List<Lesson>> UpcomingLessons()
        {
            var today = CurrentTime.DayOfWeek;
            var dayLessons = await _db.Lessons
                                      .Where(x => x.Course.Semester.EndsOn >= CurrentTime &&
                                                  x.DayOfWeek == today)
                                      .Include(x => x.Course)
                                      .ThenInclude(x => x.Semester)
                                      .ThenInclude(x => x.Batch)
                                      .ToListAsync();
            return dayLessons;
        }

        public async Task<ActionResponse> DeleteLesson(int lessonId, int courseId)
        {
            var lesson = await _db.Lessons.Include(x => x.Course)
                                  .FirstOrDefaultAsync(x => x.Id == lessonId);
            if (lesson == null || lesson.Course.Id != courseId)
            {
                return new ActionResponse(false, "Invalid Lesson Information");
            }
            else
            {
                _db.Entry(lesson).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return new ActionResponse(true);
            }
        }
    }
}
