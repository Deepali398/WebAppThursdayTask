using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebAppThursdayTask.Models;
using WebAppThursdayTask.Services;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebAppThursdayTask.Controllers
{

    /// <summary>
    /// This class is used as controller for DataItem entity
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DataItemController : ControllerBase
    {


        private readonly IDataItem _dataItem;

        //Dependeny Injection
        public DataItemController(IDataItem dataItem)
        {
            _dataItem = dataItem;
        }

        [Authorize]
        //Http delete method implementation
        [HttpDelete]
        public async Task<ActionResult> DeleteDataItemAsync(string title)
        {
            string strName;

            var identity = HttpContext.User.Identity as ClaimsIdentity;

            strName = identity.FindFirst("Name").Value;

            var action = await _dataItem.DeleteDataItemByTitleAsync(title,strName);
            if (action == null)
                return BadRequest(action);
            else
                return Ok(action);
        }
            

        [Authorize]
        //Http Post method implementation
        [HttpPost]
        public async Task<ActionResult> AddDateItemAsync(DataItemView dataItemView)
        {
            string strName;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
                         
            strName = identity.FindFirst("Name").Value;

            var action = await _dataItem.AddDataItemAsync(dataItemView ,strName);
            return Ok(action);

        }

        [Authorize]
        //Http Put method implementation
        [HttpPut]
        public async Task<ActionResult> UpdateDataItemAsync(string title ,DataItemView dataItemView)
        {
            string strName;

            var identity = HttpContext.User.Identity as ClaimsIdentity;
          
            strName = identity.FindFirst("Name").Value;

            var action = await _dataItem.UpdateDataItemAsync(title,dataItemView,strName);
            return Ok(action);

        }
    }

}
