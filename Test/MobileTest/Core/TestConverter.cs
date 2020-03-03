using CoreEngine.Engine;
using CoreEngine.Model.DBModel;
using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MobileTest.Core
{
    class TestConverter
    {
        [Test]
        public async Task COnvertObject()
        {
            var b = new Batch()
            {
                SemesterDuration = 1,
                Semesters = new List<Semester>()
                {
                    new Semester(),
                    new Semester()
                },
                Name = "Data",
                Students = new List<DBUser>()
                {
                    new DBUser(),
                    new DBUser(),
                    new DBUser()
                }
            };

            var st = new Stopwatch();
            st.Start();
            for (var counter = 1; counter < 1000; counter++)
            {
                var n = await FormHelper.GetPair(b);
            }
            st.Stop();
            Assert.Less(st.ElapsedMilliseconds, 1000);
        }
    }
}
