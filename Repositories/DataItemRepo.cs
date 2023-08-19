using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using WebAppThursdayTask.Data;
using WebAppThursdayTask.Models;
using WebAppThursdayTask.Services;
using Microsoft.AspNetCore.Identity;
using static System.Reflection.Metadata.BlobBuilder;
using Azure;

namespace WebAppThursdayTask.Repositories
{
    public class DataItemRepo:ControllerBase,IDataItem
    {
      
        private readonly DataItemDbContext _dataItemDbContext;
        

        //Dependency Injection of DBContest object
        public DataItemRepo(DataItemDbContext dataItemDbContext)
        {
            this._dataItemDbContext = dataItemDbContext ?? throw new ArgumentNullException(nameof(_dataItemDbContext));
        }


        /// <summary>
        /// Asynchronous Delete Method finds the record and deletes
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Deleted Affirmation</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<ActionResult> DeleteDataItemByTitleAsync(string title,string strName)
        {
            try
            {
                User user= _dataItemDbContext.Users.FirstOrDefault(x=>x.UserName==strName);
                DataItem dataItem = _dataItemDbContext.DataItems.FirstOrDefault(x=>x.Title==title) ?? throw new ArgumentNullException(nameof(dataItem));
                if (dataItem == null)
                {
                    return NotFound();
                }
                     
                else
                {
                    if (user.UserId == dataItem.UserId)
                    {
                        _dataItemDbContext.DataItems.Remove(dataItem);
                        await _dataItemDbContext.SaveChangesAsync(true);

                        return Ok("DataItem Deleted Successfully");
                    }
                    else 
                    {
                        return BadRequest("User Is not allowed to delete");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }



       

        /// <summary>
        /// This method is used to add a DataItem
        /// </summary>
        /// <param name="dataItem">DataItem information to add</param>
        /// <returns>Added Affirmation</returns>
        public async Task<ActionResult> AddDataItemAsync(DataItemView dataItemView, string strAuth)
        {
            try
            {
                
                User authorisedUser = _dataItemDbContext.Users.FirstOrDefault(x=>x.UserName == strAuth);

                DataItem dataItem = new DataItem();
                dataItem.Id = Guid.NewGuid();
                dataItem.Title = dataItemView.title;
                dataItem.Description = dataItemView.Description;
                dataItem.CreatedAt = dataItemView.CreatedAt;
                dataItem.UserId = authorisedUser.UserId;

                await _dataItemDbContext.DataItems.AddAsync(dataItem);

                await _dataItemDbContext.SaveChangesAsync();
              
                return Ok("DataItem Created.");

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        /// <summary>
        /// This method is used to update a DataItem details
        /// </summary>
        /// <param name="dataItem">DataItem details to update</param>
        /// <returns>DataItem details Updated Affirmation</returns>
        public async Task<ActionResult> UpdateDataItemAsync(string title,DataItemView dataItemView,string authUserName)
        {
            try
            {
                if (dataItemView == null)
                {
                    return BadRequest("DataItem not found . Data sent is invalid.");
                    
                }
                var dataItem1 = await _dataItemDbContext.DataItems.FirstOrDefaultAsync(x=>x.Title==title);
                User authorisedUser = _dataItemDbContext.Users.FirstOrDefault(x => x.UserName == authUserName);

                if (dataItem1 == null)
                {
                    return BadRequest("DataItem Not Found");
                }
                if (authorisedUser.UserId == dataItem1.UserId)
                {
                    dataItem1.Title = dataItemView.title;
                    dataItem1.Description = dataItemView.Description;
                    dataItem1.CreatedAt = dataItemView.CreatedAt;
                    dataItem1.UserId = authorisedUser.UserId;

                    await _dataItemDbContext.SaveChangesAsync();

                    return Ok("DataItem Details Updated");
                }
                else
                {
                    return BadRequest("User Is not allowed to update");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

      
       

    }
}
