using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.DBModel;
using MobileTest.Core;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MobileTest.Api
{
    public class CourseTest
    {
        private IMemberHandler member;
        private ICourseHandler courseHandler;
        private readonly Random rnd = new Random();

        [SetUp]
        public async Task SetUp()
        {
            LogEngine.IsDetailed = true;
            var http = new Mobile.Core.Worker.HttpWorker(TestConstants.WebAddress);
            member = new MemberEngine(http);
            courseHandler = new CourseEngine(http);
            await member.Login("181909", "qbQ890ZC");
        }

        [Test]
        public async Task AddCourse()
        {
            var semesters = await courseHandler.GetCurrentSemester();
            Assert.IsTrue(semesters.Count > 0);

            var course = semesters.SelectMany(x => x.Courses).FirstOrDefault();
            var file = Path.GetTempFileName();
            var data = "This is a test File\n";
            for (var i = 0; i < 1000; i++)
            {
                File.AppendAllText(file, data);
            }

            var db = new DBFile()
            {
                FilePath = file,
                FileName = Path.GetFileName(file)
            };
            var res = await courseHandler.AddMaterial(course.Id, new System.Collections.Generic.List<DBFile> { db });
            Assert.IsTrue(res.Actionstatus);
        }
    }
}
