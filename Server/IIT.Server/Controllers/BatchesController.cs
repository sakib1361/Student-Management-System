using CoreEngine.APIHandlers;
using CoreEngine.Model.Common;
using CoreEngine.Model.DBModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Student.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IIT.Server.Controllers
{
    [Authorize]
    public class BatchesController : ControllerBase, IBatchHandler
    {
        private readonly BatchService _batchService;

        public BatchesController(BatchService batchService)
        {
            _batchService = batchService;
        }

        public async Task<List<Batch>> GetBatches(int page = 1)
        {
            return await _batchService.GetBatchesAsync(page);
        }

        public async Task<ActionResponse> CreateBatch(Batch batch)
        {
            return await _batchService.AddBatch(batch);
        }

        public string Test()
        {
            return "Batch Working";
        }

        public Task<ActionResponse> UpdateBatch(Batch batch)
        {
            return _batchService.UpdateBatch(batch);
        }

        public Task<Batch> GetBatch(int batchId)
        {
            return _batchService.GetBatchAsync(batchId);
        }

        public async Task<List<Batch>> SearchBatch(string key)
        {
            return await _batchService.SearchBatch(key);
        }
    }
}