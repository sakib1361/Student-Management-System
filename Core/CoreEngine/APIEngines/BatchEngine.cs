using CoreEngine.APIHandlers;
using CoreEngine.Engine;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CoreEngine.APIEngines
{
    class BatchEngine : BaseApiEngine, IBatchHandler
    {
        private const string Controller = "batches";
        public BatchEngine(HttpWorker httpWorker) : base(httpWorker, Controller)
        {
        }

        public Task<List<Batch>> GetBatches(int page = 1)
        {
            return SendRequest<List<Batch>>(HttpMethod.Get, new { page });
        }

        public Task<ActionResponse> CreateBatch(Batch batch)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, batch);
        }

        public Task<ActionResponse> UpdateBatch(Batch batch)
        {
            return SendRequest<ActionResponse>(HttpMethod.Post, batch);
        }

        public Task<Batch> GetBatch(int batchId)
        {
            return SendRequest<Batch>(HttpMethod.Get, new { batchId });
        }

        public Task<List<Batch>> SearchBatch(string key)
        {
            return SendRequest<List<Batch>>(HttpMethod.Get, new { key });
        }
    }
}
