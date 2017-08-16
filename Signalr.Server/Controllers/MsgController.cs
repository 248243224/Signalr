using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Signalr.Server.Controllers
{
    public class MsgController : ApiController
    {
        [HttpGet]
        public IHttpActionResult SendMsg(string name, string msg)
        {
            Hub.SignalrHub.SendMsg(new Hub.MsgModel()
            {
                name = name,
                msg = msg
            });
            return Ok();
        }
        [HttpGet]
        public IHttpActionResult GetMsg()
        {
            return Ok("this is a test msg");
        }
    }
}
