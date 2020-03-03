using CoreEngine.APIEngines;
using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace CoreEngine.Helpers
{
    public static class ServiceHelper
    {
        public static void Register(IServiceCollection services)
        {
            services.AddSingleton<HttpWorker>();
            services.AddScoped<IBatchHandler, BatchEngine>();
            services.AddScoped<ICourseHandler, CourseEngine>();
            services.AddScoped<IMemberHandler, MemberEngine>();
            services.AddScoped<INoticeHandler, NoticeEngine>();
            services.AddScoped<ILessonHandler, LessonEngine>();
            services.AddScoped<ITodoItemHandler, ToDoItemEngine>();
        }
    }
}
