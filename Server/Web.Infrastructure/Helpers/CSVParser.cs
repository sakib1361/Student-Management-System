using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrasructure.Helpers
{
    class CSVParser
    {
        private const string NameColumn = "name";
        private const string PhoneColumn = "phone";
        private const string EmailColumn = "email";
        private const string RollColumn = "roll";
        private const string PointColumn = "point";
        private const string GradeColumn = "grade";

        public async Task<List<StudentCourse>> ParseResult(string filePath)
        {
            int rollCol = 0, pointCol = 1, gradeCol = 2;
            var lineNo = 0;
            var allData = new List<StudentCourse>();
            using var stream = File.OpenRead(filePath);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lineNo++;
                if (lineNo == 1)
                {
                    var colData = line.ToLower().Split(",").ToList();
                    rollCol = GetColumns(colData, RollColumn, rollCol);
                    gradeCol = GetColumns(colData, GradeColumn, gradeCol);
                    pointCol = GetColumns(colData, PointColumn, pointCol);
                }
                else
                {
                    var lineData = line.Split(",");
                    var student = new StudentCourse()
                    {
                        StudentRoll = GetData(lineData, rollCol),
                        Grade = GetData(lineData, gradeCol),
                        GradePoint = GetDataDouble(lineData, pointCol)
                    };
                    allData.Add(student);
                }
            }

            return allData;
        }
        public async Task<List<DBUser>> ParseUser(string filePath)
        {
            int lineNo = 0, rollCol = 0, nameCol = 1, emailCol = 2, phoneCol = 3;
            var studentList = new List<DBUser>();
            using var stream = File.OpenRead(filePath);
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                lineNo++;
               

                if (lineNo == 1)
                {
                    var splitter = line.ToLower().Split(',');
                    var colData = splitter.ToList();
                    rollCol = GetColumns(colData, RollColumn, rollCol);
                    nameCol = GetColumns(colData, NameColumn, nameCol);
                    emailCol = GetColumns(colData, EmailColumn, emailCol);
                    phoneCol = GetColumns(colData, PhoneColumn, phoneCol);
                }
                else
                {
                    var splitter = line.Split(',');
                    var dbUser = new DBUser()
                    {
                        Roll = GetDataint(splitter, rollCol),
                        Name = GetData(splitter, nameCol),
                        Email = GetData(splitter, emailCol),
                        PhoneNumber = GetData(splitter, phoneCol)
                    };
                    studentList.Add(dbUser);
                }
            }

            return studentList;
        }

        private int GetColumns(List<string> data, string name, int value)
        {
            var index = data.IndexOf(name.ToLower());
            return index >= 0 ? index : value;
        }
        private string GetData(string[] line, int posittion)
        {
            if (line.Length > posittion) return line[posittion];
            else return string.Empty;
        }
        private decimal GetDataDouble(string[] line,int position)
        {
            string actualData = GetData(line, position);
            decimal.TryParse(actualData, out decimal _data);
            return _data;
        }
        private int GetDataint(string[] line, int position)
        {
            string actualData = GetData(line, position);
            int.TryParse(actualData, out int _data);
            return _data;
        }
    }
}
