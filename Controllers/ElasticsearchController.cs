using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using NetCoreAPI.Models;
using Serilog;

namespace NetCoreAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [AllowAnonymous]
    public class ElasticsearchController: ControllerBase
    {
        private readonly IElasticClient _esClient;
        private readonly ILogger _logger;

        public ElasticsearchController(IElasticClient esClient, ILogger logger)
        {
            _esClient = esClient;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var response = _esClient.Get<School>("2");

            return Ok(response.Source);
        }

        [HttpGet]
        [Route("search/{schoolName}")]
        public IActionResult GetSchool(string schoolName)
        {
            // var response = _esClient.Search<School>(x => x
            //         .Query( q => q.
            //             SimpleQueryString(qs => qs.Query(schoolName))
            //             )
            //         );

            _logger.Information($"Get SchoolName {schoolName}");

            var responsedata = _esClient.Search<School>(s => s
                                .Query(q => q
                                    .Match(m => m
                                        .Field(f => f.Name)
                                        .Query(schoolName)
                                    )  
                                )  
                            ); 

            return Ok(responsedata.Documents);
        }
    }
}