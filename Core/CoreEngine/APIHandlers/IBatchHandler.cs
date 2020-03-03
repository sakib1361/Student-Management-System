using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreEngine.APIHandlers
{
    public interface IBatchHandler
    {
        Task<List<Batch>> GetBatches(int page = 1);
        Task<ActionResponse> CreateBatch(Batch batch);
        Task<ActionResponse> UpdateBatch(Batch batch);
        Task<Batch> GetBatch(int batchId);
        Task<List<Batch>> SearchBatch(string key);
    }
}
