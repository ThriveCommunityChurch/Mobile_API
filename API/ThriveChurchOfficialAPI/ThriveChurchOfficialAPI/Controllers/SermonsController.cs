﻿using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using ThriveChurchOfficialAPI.Core;
using ThriveChurchOfficialAPI.Services;

namespace ThriveChurchOfficialAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SermonsController : ControllerBase
    {
        private readonly ISermonsService _sermonsService;

        public SermonsController(ISermonsService sermonsService)
        {
            // delay the init of the repo for when we go to the service, we will grab the connection 
            // string from the IConfiguration object there instead of init-ing the repo here
            _sermonsService = sermonsService;
        }

        // GET api/sermons
        [HttpGet]
        public async Task<ActionResult<AllSermonsSummaryResponse>> GetAllSermons()
        {
            var response = await _sermonsService.GetAllSermons();

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        /// <summary>
        /// Recieve Sermon Series in a paged format
        /// </summary>
        /// <remarks>
        /// This will return the sermon series' in a paged format. 
        /// <br />
        /// NOTE: 
        /// <br />
        /// &#8901; The first page will contain the 5 first messagess.
        /// &#8901; Every subsequent page will contain up to 10 messages.
        /// &#8901; The response will contain the total number of pages.
        /// </remarks>
        /// <param name="PageNumber"></param>
        /// <returns>Paged Sermon Data</returns>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        [Produces("application/json")]
        [HttpGet("paged")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<SermonsSummaryPagedResponse>> GetPagedSermons([BindRequired] int PageNumber)
        {
            var response = await _sermonsService.GetPagedSermons(PageNumber);

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        [HttpPost("series")]
        public async Task<ActionResult<SermonSeries>> CreateNewSermonSeries([FromBody] SermonSeries request)
        {
            var response = await _sermonsService.CreateNewSermonSeries(request);

            if (response == null)
            {
                return StatusCode(400);
            }

            if (response.SuccessMessage == "202")
            {
                // Return a 202 here because this is valid, however there is something else active and nothing was done
                // "The request has been received but not yet acted upon" is what I would expect to be a correct response
                // More on that here https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/202
                return StatusCode(202, response.Result);
            }

            return response.Result;
        }

        // this query string should contain an Id
        [HttpGet("series/{SeriesId}")]
        public async Task<ActionResult<SermonSeries>> GetSeriesForId([BindRequired] string SeriesId)
        {
            var response = await _sermonsService.GetSeriesForId(SeriesId);

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        // this query string should contain an Id
        [HttpPut("series/{SeriesId}")]
        public async Task<ActionResult<SermonSeries>> ModifySermonSeries([BindRequired] string SeriesId, [FromBody] SermonSeriesUpdateRequest request)
        {
            var response = await _sermonsService.ModifySermonSeries(SeriesId, request);

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        [HttpPost("series/{SeriesId}/message")]
        public async Task<ActionResult<SermonSeries>> AddMessagesToSermonSeries([BindRequired] string SeriesId, [FromBody] AddMessagesToSeriesRequest request)
        {
            var response = await _sermonsService.AddMessageToSermonSeries(SeriesId, request);

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        [HttpPut("series/message/{MessageId}")]
        public async Task<ActionResult<SermonMessage>> UpdateMessagesInSermonSeries([BindRequired] string MessageId, [FromBody] UpdateMessagesInSermonSeriesRequest request)
        {
            var response = await _sermonsService.UpdateMessageInSermonSeries(MessageId, request);

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        [HttpGet("live")]
        public async Task<ActionResult<LiveStreamingResponse>> GetLiveSermons()
        {
            var response = await _sermonsService.GetLiveSermons();

            if (response == null)
            {
                return StatusCode(400);
            }

            var value = new ActionResult<LiveStreamingResponse>(response).Value;

            return value;
        }

        [HttpPost("live")]
        public async Task<ActionResult<LiveStreamingResponse>> GoLive([FromBody] LiveSermonsUpdateRequest request)
        {
            var response = await _sermonsService.GoLive(request);

            if (response.HasErrors)
            {
                return StatusCode(400, response.ErrorMessage);
            }

            return response.Result;
        }

        [HttpPut("live/special")]
        public async Task<ActionResult<LiveStreamingResponse>> UpdateLiveForSpecialEvents([FromBody] LiveSermonsSpecialEventUpdateRequest request)
        {
            var response = await _sermonsService.UpdateLiveForSpecialEvents(request);

            if (response == null)
            {
                return StatusCode(400);
            }

            return response;
        }

        [HttpGet("live/poll")]
        public async Task<ActionResult<LiveSermonsPollingResponse>> PollForLiveEventData()
        {
            var response = await _sermonsService.PollForLiveEventData();

            if (response == null)
            {
                return StatusCode(400, "");
            }

            return response;
        }

        [HttpDelete("live")]
        public async Task<ActionResult<LiveSermons>> UpdateLiveSermonsInactive()
        {
            var response = await _sermonsService.UpdateLiveSermonsInactive();

            if (response == null)
            {
                return StatusCode(400);
            }

            return response;
        }
    }
}