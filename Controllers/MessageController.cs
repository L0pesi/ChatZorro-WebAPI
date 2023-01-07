using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;

namespace ChatZorro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class MessageController : Controller
    {
        private Services.MessageService _message = new Services.MessageService();
        #region Message
        [HttpPut]
        [Route("Send")]
        public IActionResult Send([FromBody] Models.NewMessageModel msg)
        {
            try
            {
                return Ok(_message.Send(msg));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("Sincronize")]
        public IActionResult Sincronize()
        {
            try
            {
                return Ok(Services.MessageService.Sincronize());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Authorize]
        [Route("Delete")]
        public IActionResult Delete([FromHeader] int code)
        {
            try
            {
                return Ok(Services.MessageService.Delete(code));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
