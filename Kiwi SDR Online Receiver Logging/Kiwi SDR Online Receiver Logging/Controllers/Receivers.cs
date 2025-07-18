﻿using MonsterLlama.KiwiSDR.Web.Logger.Model;
using Microsoft.AspNetCore.Mvc;

namespace MonsterLlama.KiwiSDR.Web.Logger.Controllers
{
    [Route("api/[controller]")]
    public class Receivers : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllServers()
        {
            var receiver = new Receiver()
            {
                ReceiverId = 1,
                URL        = "http://kb6c.proxy.kiwisdr.com:8073/",
                Antenna    = "Wellbrooke Loop",
                Name       = "KB6C/6",
                Grid       = "DM04kr",
                Location   = "Stauffer, California",
                ASL        = 1585
            };

            // Test Return value
            return Ok(receiver);
        }

        [HttpGet("id={id}")]
        public IActionResult GetReceiverById(int id)
        {
            var receiver = new Receiver()
            {
                ReceiverId = id,
                URL        = "http://kb6c.proxy.kiwisdr.com:8073/",
                Antenna    = "Wellbrooke Loop",
                Name       = "KB6C/6",
                Grid       = "DM04kr",
                Location   = "Stauffer, California",
                ASL        = 1585
            };

            // Test Return value
            return Ok(receiver);
        }
    }
}
