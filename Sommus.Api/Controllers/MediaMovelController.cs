using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Sommus.Api.DTO;
using Sommus.Api.Repository;
using Sommus.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sommus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaMovelController : ControllerBase
    {
        private readonly IConfirmedRepository _confirmedRepository;
        private readonly IDeathsRepository _deathsRepository;
        public MediaMovelController(IConfirmedRepository confirmedRepository, IDeathsRepository deathsRepository)
        {
            _confirmedRepository = confirmedRepository;
            _deathsRepository = deathsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<MediaMovel>> Get([FromQuery] DateTime date)
        {
            if (date.ToUniversalTime() < DateTime.UtcNow.Date.AddMonths(-6))
                return BadRequest();
            if (date.ToUniversalTime() > DateTime.UtcNow)
                return BadRequest();

            DateTime startOfWeek = date.AddDays(0 - (int)date.DayOfWeek);
            double minConfirmed = (await _confirmedRepository?.Get(startOfWeek.ToString("yyyy'-'MM'-'dd HH':'mm':'ss")) ?? new Confirmed()).Cases;
            double minDeaths = (await _deathsRepository?.Get(startOfWeek.ToString("yyyy'-'MM'-'dd HH':'mm':'ss")) ?? new Deaths()).Cases;

            double maxConfirmed = (await _confirmedRepository?.Get(startOfWeek.AddDays(6).ToString("yyyy'-'MM'-'dd HH':'mm':'ss")) ?? await _confirmedRepository.GetLast()).Cases;
            double maxDeaths = (await _deathsRepository?.Get(startOfWeek.AddDays(6).ToString("yyyy'-'MM'-'dd HH':'mm':'ss")) ?? await _deathsRepository.GetLast()).Cases;

            return new MediaMovel
            {
                Confirmed = (maxConfirmed - minConfirmed) / 7,
                Deaths = (maxDeaths - minDeaths) / 7
            };
        }
    }
}
