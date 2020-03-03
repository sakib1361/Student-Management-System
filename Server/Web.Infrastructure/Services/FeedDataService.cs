using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class FeedDataService : BaseService
    {
        private int BatchStart = 18;
        private int BatchCount = 5;
        private string _userId;
        private readonly BatchService _batchService;
        private readonly NoticeService _noticeService;
        private readonly CourseService _courseService;
        private readonly UserService _userService;
        private readonly FeedData _feed;
        private readonly Random rnd = new Random();
        public FeedDataService(BatchService batchService,
            NoticeService noticeService,
            CourseService courseService,
            UserService userService)
        {
            _batchService = batchService;
            _noticeService = noticeService;
            _courseService = courseService;
            _userService = userService;
            _feed = new FeedData();
        }

        public async Task StartAsync(string userId)
        {
            _userId = userId;
            var start = BatchStart;
            while (BatchCount > 0)
            {
                await CreateBatch("MIT", start, 4);
                start--;
                BatchCount--;
            }
            await CreateNotices();
        }

        private async Task CreateNotices()
        {
            var allCourse = await _courseService.SearchCourse("");
            foreach (var course in allCourse)
            {
                var cNotices = _feed.CreateNotice(course, course.Semester.Batch);
                var courseInfo = await _courseService.GetCourseAsync(course.Id);
                var lNotices = _feed.CreateNotice(course, courseInfo.Lessons.ToList());

                cNotices.AddRange(lNotices);
                foreach (var item in cNotices)
                {
                    await _noticeService.AddUpdateNotice(item, _userId);
                }
            }

        }

        private async Task CreateBatch(string batch, int batchNo, int semester)
        {
            var batchName = batch + batchNo.ToString();
            var resp = await _batchService.AddBatch(new Batch()
            {
                StartsOn = new DateTime(2000 + batchNo, 1, 1),
                Name = batchName,
                NumberOfSemester = semester,
                SemesterDuration = 4
            });
            if (resp.Actionstatus && resp.Data is int batchId)
            {
                await CreateStudent(batchId, batchNo);
                Console.WriteLine("Batch Created: " + batchName);
                await CreateCourse(batchId);
            }
        }

        private async Task CreateCourse(int batchId)
        {
            var batch = await _batchService.GetBatchAsync(batchId);
            var allLesson = new List<Lesson>();
            foreach (var semester in batch.Semesters)
            {
                var allCourse = _feed.Courses();
                var counter = 3;
                while (counter > 0)
                {
                    var course = allCourse[rnd.Next(0, allCourse.Count)];
                    var resp = await _courseService.AddCourse(course, semester.Id, batchId);
                    if (resp.Actionstatus)
                    {
                        Console.WriteLine(" Course Created: " + course.CourseName);
                        counter--;
                        for (var lessonCount = 0; lessonCount < 3; lessonCount++)
                        {
                            var lesson = _feed.CreateLesson(allLesson);
                            resp = await _courseService.AddUpdateLesson(course.Id, lesson);
                            if (resp.Actionstatus)
                            {
                                Console.WriteLine("  Lesson Created: " + lesson.DayOfWeek.ToString() + " : " + lesson.TeacherName);
                            }
                        }
                    }
                }
            }
        }

        private async Task CreateStudent(int batchId, int batchNumber)
        {
            for (int counter = 1; counter < 40; counter++)
            {
                var name = _feed.GetStudentName();
                var roll = (batchNumber * 100 + counter).ToString();
                var email = name.Replace(" ", ".").ToLower() + "@email.com";
                var phoneNumber = rnd.Next(01501100000, 01988990000).ToString();
                await _userService.AddStudent(batchId, roll, name, email, phoneNumber);
            }
        }
    }

    class FeedData
    {
        private readonly Random rnd = new Random();
        internal List<Course> Courses()
        {
            var allCourse = new List<Course>
            {
                Create("MITM301", "Project Management and Business Info System", 2),
                Create("MITM302", "Computer Programming", 4),
                Create("MITM304", "Database Architecture and Administration", 4),
                Create("MITM306", "Advanced Computer Networks & Internetworking", 4),

                Create("MITM303", "Client Server Technology and System Programming", 4),
                Create("MITM305", "Internet Computing", 4),
                Create("MITP421", "Project for MIT", 6),

                Create("MITE401", "Data Mining and Warehousing", 3),
                Create("MITE402", "E-Commerce Technologies in E-Business", 3),
                Create("MITE403", "Computer, Data, Network Security/E-Security", 3),
                Create("MITE404", "Parallel and Distributed Processing 3", 2),
                Create("MITE405", "Computer Graphics and Multimedia", 3),
                Create("MITE406", "Simulation and Modeling", 3),
                Create("MITE412", "Advanced Object Oriented Programming", 3),
                Create("MITE414", "Software Testing", 3)
            };
            return allCourse;
        }

        internal Lesson CreateLesson(List<Lesson> lessons)
        {
            var day = (DayOfWeek)rnd.Next(0, 6);
            var time = TimeSpan.FromHours(rnd.Next(17, 21)).Add(TimeSpan.FromMinutes(15 * rnd.Next(0, 4)));
            var oldlesson = lessons.FirstOrDefault(x => x.DayOfWeek == day && Math.Abs((x.TimeOfDay - time).Minutes) < 60);
            if (oldlesson != null)
            {
                return CreateLesson(lessons);
            }
            else
            {
                return new Lesson()
                {
                    DayOfWeek = day,
                    RoomNo = "R" + rnd.Next(100, 300),
                    TeacherName = GetTeacherName(),
                    TimeOfDay = time,
                    Description = "Short description"
                };
            }
        }

        private Course Create(string id, string name, decimal credit)
        {
            return new Course()
            {
                CourseId = id.Trim(),
                CourseName = name.Trim(),
                CourseCredit = credit,
                Description = "This sample course was generated from feed service.\nOptional Information",
            };
        }

        internal string GetTeacherName()
        {
            return TeacherNames[rnd.Next(0, TeacherNames.Count)].Trim();
        }

        internal string GetStudentName()
        {
            return UserNames[rnd.Next(0, UserNames.Count)].Trim();
        }

        internal List<Notice> CreateNotice(Course course, Batch batch)
        {
            var noticeList = new List<Notice>();
            var end = course.Semester.EndsOn.AddDays(-1 * rnd.Next(1, 15)).Date.AddHours(17);
            var exam = new Notice()
            {
                CourseId = course.Id,
                Batch = batch,
                EventDate = end,
                PostType = PostType.Examination,
                Title = "Final Exam of : " + course.CourseId,
                Message = string.Format("Term final exam of {0} of batch {1} will be held on {2}",
                course.CourseName, batch.Name, end.ToLongDateString()),
                FutureNotification = true
            };
            noticeList.Add(exam);
            return noticeList;
        }

        internal List<Notice> CreateNotice(Course course, List<Lesson> list)
        {
            var noticeList = new List<Notice>();
            foreach (var item in list)
            {
                var cancel = RandomDate(course.Semester.StartsOn, course.Semester.EndsOn, item.DayOfWeek);
                var cancelNotice = new Notice()
                {
                    CourseId = course.Id,
                    Batch = course.Semester.Batch,
                    EventDate = cancel.Date.Add(item.TimeOfDay),
                    PostType = PostType.ClassCancel,
                    Title = "Class has been Cancelled for : " + course.CourseId,
                    Message = string.Format("Class Cancelled for {0} of batch {1} intended to be held on {2}",
                    course.CourseName, course.Semester.Batch.Name, cancel.ToLongDateString()),
                    FutureNotification = true
                };

                var cancel2 = RandomDate(course.Semester.StartsOn, course.Semester.EndsOn, item.DayOfWeek);
                var cancel2Notice = new Notice()
                {
                    CourseId = course.Id,
                    Batch = course.Semester.Batch,
                    EventDate = cancel2.Date.Add(item.TimeOfDay),
                    PostType = PostType.ClassCancel,
                    Title = "Class has been Cancelled for : " + course.CourseId,
                    Message = string.Format("Class Cancelled for {0} of batch {1} intended to be held on {2}",
                    course.CourseName, course.Semester.Batch.Name, cancel2.ToLongDateString()),
                    FutureNotification = true
                };

                var examDay = RandomDate(course.Semester.StartsOn.AddMonths(1), course.Semester.EndsOn, item.DayOfWeek);
                var examNotice = new Notice()
                {
                    CourseId = course.Id,
                    Batch = course.Semester.Batch,
                    EventDate = cancel.Date.Add(item.TimeOfDay),
                    PostType = PostType.ClassCancel,
                    Title = "Mid Term Exam for : " + course.CourseId,
                    Message = string.Format("Mid Term for {0} of batch {1} intended to be held on {2}",
                    course.CourseName, course.Semester.Batch.Name, examDay.ToLongDateString()),
                    FutureNotification = true
                };
                noticeList.Add(cancelNotice);
                noticeList.Add(cancel2Notice);
                noticeList.Add(examNotice);
            }
            return noticeList;
        }

        private DateTime RandomDate(DateTime start, DateTime end, DayOfWeek dayOfWeek)
        {
            int range = rnd.Next((end.AddDays(-15) - start).Days);
            var date = start.AddDays(range);
            while (date.DayOfWeek != dayOfWeek)
            {
                date = date.AddDays(1);
            }
            return date;
        }

        private readonly List<string> TeacherNames = new List<string>()
        {
            "Rina Cho  ",
            "Kathryn Southwell  ",
            "Marvel Roquemore  ",
            "Lynetta Sedlak  ",
            "Cheryle Giorgio  ",
            "Noma Topham  ",
            "Lasonya Cumbee  ",
            "Julieta Aichele  ",
            "Adeline Engman  ",
            "Ernestine Gerling  ",
            "Coralee Goldsberry  ",
            "Brittanie Click  ",
            "Danna Penfold  ",
            "Arlene Ramey  ",
            "Kate Pedrick  ",
            "Numbers Hellyer  ",
            "Neely Fitzgibbon  ",
            "Janette Erving  ",
            "Loriann Simonson  ",
            "Chan Scola  ",
            "Zachary Mahony  ",
            "Renae Amell  ",
            "Les Arner  ",
            "Zack Pinkard  ",
            "Jamie Manis  ",
            "Diane Ye  ",
            "Quinn Boulden  ",
            "Henrietta Huson  ",
            "Johnette Funaro  ",
            "Roma Derrick  ",
            "Rachal Lowy  ",
            "Deeann Villicana  ",
            "Wallace Petri  ",
            "Giselle Hames  ",
            "Pinkie Pettyjohn  ",
            "Vickey Moreles  ",
            "Emily Lieber  ",
            "Sondra Mak  ",
            "Florencia Deford  ",
            "Florence Shi  ",
            "Reginald Bertolino  ",
            "Leighann Silvey  ",
            "Orval Reese  ",
            "Dominick Fox  ",
            "Kelsey Quast  ",
            "Jarred Santini  ",
            "Sang Fallon  ",
            "Kaci Kwiatkowski  ",
            "Roselyn Molnar  ",
            "Erika Whitehouse "
        };
        private readonly List<string> UserNames = new List<string>()
        {
            "Lucie Gatson ",
            "Nohemi Blackledge  ",
            "Nella Melillo  ",
            "Bonnie Coder  ",
            "Cherryl Swigart  ",
            "Lester Burner  ",
            "Laveta Minnich  ",
            "Catharine Madore  ",
            "Marline Schow  ",
            "Lakesha Harmon  ",
            "Contessa Buckingham  ",
            "Marguerita Philipps  ",
            "Sylvia Boren  ",
            "Tandra Greene  ",
            "Cecily Ridlon  ",
            "Barabara Stringer  ",
            "Annamarie Ancona  ",
            "Ilse Liberto  ",
            "Su Stonge  ",
            "Juliette Rhea  ",
            "Percy Frankum  ",
            "Lyndsay Keith  ",
            "Odessa Tidwell  ",
            "Howard Welty  ",
            "Celina Ardoin  ",
            "Sergio Stamps  ",
            "Rosaura Zahler  ",
            "Christena Kuss  ",
            "Dwight Fessenden  ",
            "Alissa Baskerville  ",
            "Paula Gaydos  ",
            "Dewitt Maiorano  ",
            "Elena Trevarthen  ",
            "Camelia Lukach  ",
            "Elwanda Ota  ",
            "Renaldo Adkins  ",
            "Darrell Mcniel  ",
            "Angie Mullings  ",
            "Latoyia Alegria  ",
            "Sanda Vanburen  ",
            "Jina Trotter  ",
            "Jeanette Ivers  ",
            "Booker Veasley  ",
            "Kelsie Burger  ",
            "Jeremy Mcmullin  ",
            "Colleen Pinkerton  ",
            "Afton Caul  ",
            "Meryl Lytle  ",
            "Amos Suriel  ",
            "Meghan Dahn  ",
            "Harley Shippee  ",
            "Phung Vallecillo  ",
            "Patience Shemwell  ",
            "Miss Overcast  ",
            "Marchelle Benda  ",
            "Maxie Bedell  ",
            "Kristeen Garlington  ",
            "Pia Keltz  ",
            "Sanda Taing  ",
            "Masako Ferro  ",
            "Ashanti Bieniek  ",
            "Bernadine Mehler  ",
            "Shella Roemer  ",
            "Soila Torpey  ",
            "Cheyenne Hawks  ",
            "Lashawna Winberg  ",
            "Gabriela Goltz  ",
            "Lady Currey  ",
            "Kristian Venable  ",
            "Emmitt Bronner  ",
            "Daisy Cassell  ",
            "Rosina Chesnutt  ",
            "Carroll Maroon  ",
            "Jay Peabody  ",
            "Bennett Alban  ",
            "Mohammad Philippi  ",
            "Aleta Kwiatkowski  ",
            "Celinda Blow  ",
            "Larry Hostetter  ",
            "Silva Bendixen  ",
            "Lakia Sapien  ",
            "Regine Litteral  ",
            "Emely Hang  ",
            "Lael Grove  ",
            "Kacey Maddux  ",
            "Cher Parten  ",
            "Nickie Altamirano  ",
            "Bella Benzing  ",
            "Jere Esperanza  ",
            "Lashay Lukes  ",
            "Rogelio Poisson  ",
            "Anja Amerine  ",
            "Janette Freer  ",
            "Dominic Rowlands  ",
            "Zoe Harada  ",
            "Donovan Lazar  ",
            "Devon Lisi  ",
            "Lyman Butterfield  ",
            "Hyun Wilczynski  ",
            "Colene Yohe  ",
            "Elda Stadler  ",
            "Melva Copley  ",
            "Stanley Marney  ",
            "Shanika Quimby  ",
            "Jacklyn Needles  ",
            "Eneida Robson  ",
            "Clelia Rexford  ",
            "Bianca Shafer  ",
            "Ingeborg Burwell  ",
            "Hank Voliva  ",
            "Chantal Schleifer  ",
            "Eddy Baade  ",
            "Jodee Tynes  ",
            "Samella Zhao  ",
            "Easter Freudenthal  ",
            "Camellia Debartolo  ",
            "Zita Landsman  ",
            "Jenell Thackston  ",
            "Lamonica Madding  ",
            "Maurine Zeng  ",
            "Leah Emigh  ",
            "Rachael Hoskinson  ",
            "Sylvie Grimsley  ",
            "Neva Mcchesney  ",
            "Michiko Tapscott  ",
            "Gillian Rocco  ",
            "Cinda Fortino  ",
            "Jessia Frame  ",
            "Kala Zynda  ",
            "Kemberly Arzate  ",
            "Eboni Bartz  ",
            "Jonell Plummer  ",
            "Agatha Strackbein  ",
            "Tyrone Kesselman  ",
            "Belkis Resendez  ",
            "Ivey Ratledge  ",
            "Milan Ursery  ",
            "Tena Eastman  ",
            "Demarcus Apodaca  ",
            "Linsey Polinsky  ",
            "Roselia Radke  ",
            "Maisie Coffee  ",
            "Quiana Chun  ",
            "Earlean Almond  ",
            "Jacelyn Rolph  ",
            "Fawn Hafner  ",
            "Janice Leopold  ",
            "Cherry Wroten  ",
            "Lottie Arebalo  ",
            "Larae Macpherson  ",
            "Scott Moscoso  ",
            "Nicolle Linney  ",
            "Myrta Junge  ",
            "Pedro Esh  ",
            "Murray Fulks  ",
            "Jacquelynn Uhlman  ",
            "Classie Samples  ",
            "Renaldo Viviani  ",
            "Thao Spoon  ",
            "Mallory Mcnulty  ",
            "Danuta Hoying  ",
            "Sau Fishburn  ",
            "Jacqulyn Bish  ",
            "Vance Bissette  ",
            "Olen Fulcher  ",
            "Elvia Ashline  ",
            "Janet Hillier  ",
            "Daina Colas  ",
            "Crystle Munch  ",
            "Rickie Gatto  ",
            "Sang Shouse  ",
            "Setsuko Beacham  ",
            "Wilburn Pillsbury  ",
            "Lizeth Seip  ",
            "Carmelita Zeng  ",
            "Sharonda Parsley  ",
            "Clarence Ness  ",
            "Margit Levay  ",
            "Rory Lasseter  ",
            "Hassan Klee  ",
            "Winter Stretch  ",
            "Alisia Olive  ",
            "Yael Hotaling  ",
            "Kiersten Kiddy  ",
            "Violeta Rivenburg  ",
            "Basilia Colosimo  ",
            "Marcelle Stayer  ",
            "Analisa Mahi  ",
            "Cira Hammons  ",
            "Tessie Schier  ",
            "Iesha Winningham  ",
            "Angelia Ferro  ",
            "Fatimah Marini  ",
            "Lazaro Rakow  ",
            "Jacinta Key  ",
            "Eldridge Geib  ",
            "Ione Squillante  ",
            "Francie Shalash  ",
            "Louis Miele  ",
            "Carlita Morein  "
        };
    }
}
