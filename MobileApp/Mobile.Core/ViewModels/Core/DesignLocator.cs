using Mobile.Core.Engines.Dependency;

namespace Mobile.Core.ViewModels
{
    public static class DesignLocator
    {
        public static LoginViewModel LoginViewModel => GetLocator<LoginViewModel>();
        public static ForgetPassViewModel ForgetPassViewModel => GetLocator<ForgetPassViewModel>();
        public static AddUpdateCourseViewModel AddUpdateCourseViewModel => GetLocator<AddUpdateCourseViewModel>();
        public static AddUpdateLessonViewModel AddUpdateLessonViewModel => GetLocator<AddUpdateLessonViewModel>();
        public static CourseDetailViewModel CourseDetailViewModel => GetLocator<CourseDetailViewModel>();
        public static CoursesViewModel CoursesViewModel => GetLocator<CoursesViewModel>();
        public static HomeViewModel HomeViewModel => GetLocator<HomeViewModel>();
        public static AddUpdateNoticeViewModel AddUpdateNoticeViewModel => GetLocator<AddUpdateNoticeViewModel>();
        public static NoticeDetailViewModel NoticeDetailViewModel => GetLocator<NoticeDetailViewModel>();
        public static NoticesViewModel NoticesViewModel => GetLocator<NoticesViewModel>();
        public static ProfileDetailViewModel ProfileDetailViewModel => GetLocator<ProfileDetailViewModel>();
        public static StudentsViewModel ProfilesViewModel => GetLocator<StudentsViewModel>();
        public static SplashViewModel SplashViewModel => GetLocator<SplashViewModel>();
        public static MainViewModel MainViewModel => Locator.GetInstance<MainViewModel>();

        private static T GetLocator<T>()
        {
            return default;
        }
    }
}
