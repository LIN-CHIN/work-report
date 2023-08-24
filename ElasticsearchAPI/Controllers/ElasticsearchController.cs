using ElasticsearchAPI.Documents;
using ElasticsearchAPI.DTOs;
using ElasticsearchAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticsearchAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElasticsearchController : ControllerBase
    {
        private readonly IElasticsearchService _elasticsearchService;

        public ElasticsearchController(IElasticsearchService elasticsearchService) 
        {
            _elasticsearchService = elasticsearchService;
        }

        [HttpGet()]
        public IActionResult GetByEventId([FromQuery] QueryDocumentDTO queryDocumentDTO,
                                          [FromQuery] QueryBaseDTO queryBaseDTO)
        {
            return Ok(new 
            { 
                items = _elasticsearchService.Get(queryDocumentDTO, queryBaseDTO) 
            });
        }

        /// <summary>
        /// 根據EventId取得Log
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="fromIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        [HttpGet("{eventId}")]
        public IActionResult GetByEventId( [FromRoute] string eventId,
                                           [FromQuery] QueryBaseDTO queryBaseDTO) 
        {
            return Ok( new 
            {
                items = _elasticsearchService.GetByEventId(eventId, queryBaseDTO)
            });
        }
    }
}
