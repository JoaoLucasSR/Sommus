using Microsoft.AspNetCore.Mvc;
using Serilog;
using Sommus.Api.Repository;
using Sommus.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sommus.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PopulateDatabaseController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfirmedRepository _confirmedRepository;
        private readonly IDeathsRepository _deathsRepository;
        public PopulateDatabaseController(IHttpClientFactory clientFactory, IConfirmedRepository confirmedRepository, IDeathsRepository deathsRepository)
        {
            _clientFactory = clientFactory;
            _confirmedRepository = confirmedRepository;
            _deathsRepository = deathsRepository;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var date = DateTime.UtcNow.Date.AddMonths(-6);
            var startOfWeek = date.AddDays(0 - (int)date.DayOfWeek);
            var client = _clientFactory.CreateClient();
            var requestConfirmed = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.covid19api.com/country/brazil/status/confirmed?from={startOfWeek.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}&to={DateTime.UtcNow.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}");
            var requestDeaths = new HttpRequestMessage(HttpMethod.Get,
            $"https://api.covid19api.com/country/brazil/status/deaths?from={startOfWeek.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}&to={DateTime.UtcNow.Date.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff'Z'")}");
            requestConfirmed.Headers.Add("Accept", "application/json");
            requestDeaths.Headers.Add("Accept", "application/json");

            var responseConfirmed = await client.SendAsync(requestConfirmed);
            var responseDeaths = await client.SendAsync(requestDeaths);

            if (responseConfirmed.IsSuccessStatusCode && responseDeaths.IsSuccessStatusCode)
            {
                using var responseConfirmedStream = await responseConfirmed.Content.ReadAsStreamAsync();
                using var responseDeathsStream = await responseDeaths.Content.ReadAsStreamAsync();
                var confirmeds = await JsonSerializer.DeserializeAsync<IEnumerable<Confirmed>>(responseConfirmedStream);
                var deaths = await JsonSerializer.DeserializeAsync<IEnumerable<Deaths>>(responseDeathsStream);
                int errorConfirmed = 0;
                int errorDeaths = 0;

                errorConfirmed = await _confirmedRepository.AddAll(confirmeds.ToList());
                errorDeaths = await _deathsRepository.AddAll(deaths.ToList());

                if ((errorConfirmed != 0) && (errorDeaths != 0))
                    return Ok();

                return BadRequest();
            }
            else
            {
                Log.Error(responseConfirmed.StatusCode.ToString());
                return BadRequest();
            }
        }
    }
}
