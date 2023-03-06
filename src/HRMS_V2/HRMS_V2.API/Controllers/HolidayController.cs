using HRMS_V2.API.Requests;
using HRMS_V2.Application.Interfaces;
using HRMS_V2.Application.Models;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HRMS_V2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HolidayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IHolidayService _holidayService;

        public HolidayController(IMediator mediator, IHolidayService holidayService)
        {
            _mediator = mediator;
            _holidayService = holidayService;
        }

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HolidayModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<HolidayModel>>> GetHolidaysAsync()
        {
            var holidays = await _holidayService.GetHolidayListAsync();

            return Ok(holidays);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<HolidayModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<HolidayModel>>> SearchHolidaysAsync(SearchPageRequest request)
        {
            var holidayList = await _holidayService.SearchHolidaysAsync(request.Args);

            return Ok(holidayList);
        }

        [Route("[action]")]
        [HttpGet]
        [ProducesResponseType(typeof(HolidayModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<HolidayModel>> GetHolidayByIdAsync(int Id)
        {
            var holiday = await _holidayService.GetHolidayByIdAsync(Id);

            return Ok(holiday);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType(typeof(HolidayModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<HolidayModel>> CreateHolidayAsync(AddHolidayRequest request)
        {
            var commandResult = await _mediator.Send(request);

            return Ok(commandResult);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> UpdateHolidayAsync(UpdateHolidayRequest request)
        {
            var commandResult = await _mediator.Send(request);

            return Ok(commandResult);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> DeleteHolidayByIdAsync(DeleteHolidayByIdRequest request)
        {
            var commandResult = await _mediator.Send(request);

            return Ok(commandResult);
        }
    }
}
