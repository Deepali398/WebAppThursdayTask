using Microsoft.AspNetCore.Mvc;
using WebAppThursdayTask.Models;
using static System.Reflection.Metadata.BlobBuilder;

namespace WebAppThursdayTask.Services
{
    public interface IDataItem
    {
        //Delete DataItem by ID
        Task<ActionResult> DeleteDataItemByTitleAsync(string title, string strName);

        //Add DataItem
        Task<ActionResult> AddDataItemAsync(DataItemView dataItemView ,string str);

        //Update DataItem
        Task<ActionResult> UpdateDataItemAsync(string title, DataItemView dataItemView, string authUserName);
       
    }
}
