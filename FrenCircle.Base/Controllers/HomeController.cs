﻿using FrenCircle.Base.Attributes;
using FrenCircle.Infra;
using Microsoft.AspNetCore.Mvc;

namespace FrenCircle.Base.Controllers
{
    [Route("/api/common")]
    [ApiController]
    public class HomeController(ILogger<HomeController> logger,ITelegramService telegramService) : FcBaseController
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly ITelegramService _telegramService = telegramService;
        [HttpGet]
        [Persist($"server-info2")]
        public async Task<IActionResult> ServerStat()
        {
            _ = _telegramService.SendMessageAsync("asas");
            return RESP_Success("||", "Server is up!!");

        }

    }
}