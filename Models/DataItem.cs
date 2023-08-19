using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace WebAppThursdayTask.Models
{
    public class DataItem
    {
        public Guid Id { get; set; }    
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } 

        //Navigation Propery
        public int UserId { get; set; }
        public User User { get; set; }
    }
}

