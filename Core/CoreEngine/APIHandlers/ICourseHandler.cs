using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface ICourseHandler
    {
        Task<ActionResponse> CreateCourse(int semesterId, Course course, List<DBFile> dBFiles, List<IFormFile> formFiles = null);
        Task<List<Course>> GetSemesterCourses(int semesterId);
        Task<ActionResponse> UpdateCourse(Course course);
        Task<ActionResponse> DeleteCourse(Course course);
        Task<ActionResponse> DeleteCouseMaterial(DBFile dBFile);
        Task<ActionResponse> AddMaterial(int courseId, List<DBFile> dbFiles, List<IFormFile> formFiles = null);
        Task<Course> GetCourse(int courseId);
        Task<List<Semester>> GetCurrentSemesters(string userId);
        Task<List<Semester>> GetStudentCurrentSemesters();
        Task<ActionResponse> UploadCourseResult(int courseId, DBFile dBFile, IFormFile formFile);
        Task<List<SemesterData>> GetResult();
        Task<List<SemesterData>> GetStudentResult(string userId);
        Task<List<Course>> SearchCourse(string search);
    }
}
