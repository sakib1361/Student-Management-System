using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.EntityFrameworkCore;
using Student.Infrastructure.DBModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Student.Infrastructure.Services
{
    public class BatchService : BaseService
    {
        private readonly StudentDBContext _db;

        public BatchService(StudentDBContext studentDBContext)
        {
            _db = studentDBContext;
        }
        public async Task<List<Batch>> GetBatchesAsync(int currentPage, int pageCount = 20)
        {
            var start = (currentPage - 1) * pageCount;
            return await _db.Batches.OrderByDescending(x => x.StartsOn)
                                    .Skip(start)
                                    .Take(pageCount)
                                    .ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return await _db.Batches.CountAsync();
        }

        public async Task<ActionResponse> AddBatch(Batch batch)
        {
            batch.Name = batch.Name.ToUpper();
            if (await _db.Batches.AnyAsync(x => x.Name == batch.Name))
            {
                return new ActionResponse(false, "Batch Name Already exists");
            }
            for (int counter = 0; counter < batch.NumberOfSemester; counter++)
            {
                var sem = new Semester()
                {
                    Duration = batch.SemesterDuration,
                    Name = "Semester " + (counter + 1).ToString(),
                    StartsOn = batch.StartsOn.AddMonths(counter * batch.SemesterDuration),
                    EndsOn = batch.StartsOn.AddMonths((counter + 1) * batch.SemesterDuration).AddMinutes(-1)
                };
                batch.Semesters.Add(sem);
            }
            batch.EndsOn = batch.StartsOn
                                .AddMonths(batch.SemesterDuration * batch.NumberOfSemester)
                                .AddDays(7);
            _db.Batches.Add(batch);
            await _db.SaveChangesAsync();
            return new ActionResponse(true)
            {
                Data = batch.Id
            };
        }

        public async Task<Batch> GetBatchAsync(int id)
        {
            var res = await _db.Batches
                               .Include(x => x.Students)
                               .Include(x => x.Semesters)
                               .ThenInclude(x => x.Courses)
                               .FirstOrDefaultAsync(x => x.Id == id);
            return res;
        }

        public async Task<ActionResponse> UpdateBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.Include(x=>x.Semesters)
                                            .FirstOrDefaultAsync(x=>x.Id == batch.Id);
            if (oldBatch == null)
            {
                return new ActionResponse(false, "The batch information was not found");
            }
            else
            {
                oldBatch.StartsOn = batch.StartsOn;
                oldBatch.Name = batch.Name;
                oldBatch.SemesterDuration = batch.SemesterDuration;

                oldBatch.EndsOn = oldBatch.StartsOn
                               .AddMonths(oldBatch.SemesterDuration * oldBatch.NumberOfSemester)
                               .AddDays(7);


                var counter = 0;
                foreach(var sem in oldBatch.Semesters.OrderBy(x=>x.StartsOn))
                {
                    sem.Duration = oldBatch.SemesterDuration;
                    sem.StartsOn = oldBatch.StartsOn.AddMonths(counter * oldBatch.SemesterDuration);
                    sem.EndsOn = oldBatch.StartsOn.AddMonths((counter + 1) * oldBatch.SemesterDuration).AddMinutes(-1);
                    _db.Entry(sem).State = EntityState.Modified;
                    counter++;
                }
                _db.Entry(oldBatch).State = EntityState.Modified;
                await _db.SaveChangesAsync();
                return new ActionResponse(true, "Batch Information Updated Successfully");
            }
        }

        public async Task<ActionResponse> DeleteBatch(Batch batch)
        {
            var oldBatch = await _db.Batches.Include(x => x.Students)
                                            .Include(x => x.Semesters)
                                            .FirstOrDefaultAsync(x => x.Id == batch.Id);
            if (oldBatch == null)
            {
                return new ActionResponse(false, "The batch information was not found");
            }
            else
            {
                foreach (var student in oldBatch.Students)
                {
                    _db.Entry(student).State = EntityState.Deleted;
                }

                foreach (var semester in oldBatch.Students)
                {
                    _db.Entry(semester).State = EntityState.Deleted;
                }

                _db.Entry(oldBatch).State = EntityState.Deleted;
                await _db.SaveChangesAsync();
                return new ActionResponse(true, "Batch Information Deleted Successfully");
            }
        }

        public async Task<List<DBUser>> GetBatchStudents(int batchId)
        {
            var batch = await _db.Batches
                                 .Include(x => x.Students)
                                 .FirstOrDefaultAsync(x => x.Id == batchId);
            return batch.Students.ToList();
        }

        public async Task<List<Batch>> SearchBatch(string key)
        {
            return await _db.Batches.OrderByDescending(x => x.StartsOn)
                                    .Where(x => EF.Functions.Like(x.Name, $"%{key}%"))
                                    .Take(30)
                                    .ToListAsync();
        }
    }
}
