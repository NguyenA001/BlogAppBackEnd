using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAppBackEnd.Models;
using BlogAppBackEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlogAppBackEnd.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlogItemController : ControllerBase
    {
        private readonly BlogItemService _data;
        public BlogItemController(BlogItemService dataFromService)
        {
            _data = dataFromService;
        }

        [HttpPost("AddBlogItem")]
        public bool AddBlogItem(BlogItemModel newBlogItem)
        {
            return _data.AddBlogItem(newBlogItem);
        }
        [HttpGet("GetAllBlogItem")]
        public IEnumerable<BlogItemModel> GetAllBlogItems()
        {
            return _data.GetAllBlogItems();
        }

        [HttpGet("GetBlogItemByUserId/{userID}")]
        public IEnumerable<BlogItemModel> GetItemByUserId(int userID)
        {
            return _data.GetItemByUserId(userID);
        }
        
        [HttpGet("GetItemsByCategory/{Category}")]
        public IEnumerable<BlogItemModel> GetItemsByCategory(string Category)
        {
            return _data.GetItemsByCategory(Category);
        }

        [HttpGet("GetItemsByTag/{Tag}")]
        public List<BlogItemModel> GetItemsByTag(string Tag)
        {
            return _data.GetItemsByTag(Tag);
        }

        [HttpGet("GetItemsByDate/{Date}")]
        public IEnumerable<BlogItemModel> GetItemsByDate(string Date)
        {
            return _data.GetItemsByDate(Date);
        }

        [HttpGet("GetPublishedItems")]
        public IEnumerable<BlogItemModel> GetPublishedItems()
        {
            return _data.GetPublishedItems();
        }

        [HttpPost("UpdateBlogItem")]

        public bool UpdateBlogItem (BlogItemModel BlogUpdate)
        {
            return _data.UpdateBlogItem(BlogUpdate);
        }

        [HttpPost("DeleteBlogItem")]
         public bool DeleteBlogItem (BlogItemModel BlogDelete)
        {
           return _data.DeleteBlogItem(BlogDelete);
        }
        
        [HttpGet("GetBlogItemById/{Id}")]
        public BlogItemModel GetBlogItemById(int Id)
        {
            return _data.GetBlogItemById(Id);
        }
    }
}