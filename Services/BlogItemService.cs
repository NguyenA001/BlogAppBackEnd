using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogAppBackEnd.Models;
using BlogAppBackEnd.Services.Context;

namespace BlogAppBackEnd.Services
{
    public class BlogItemService
    {
        private readonly DataContext _context;
        public BlogItemService(DataContext DataFromContext)
        {
            _context = DataFromContext;
        }

        public bool AddBlogItem(BlogItemModel newBlogItem)
        {
            _context.Add(newBlogItem);
            return _context.SaveChanges() != 0;
        }

        public IEnumerable<BlogItemModel> GetAllBlogItems()
        {
            return _context.BlogInfo;
        }

        public IEnumerable<BlogItemModel> GetItemByUserId(int userID)
        {
            return _context.BlogInfo.Where(item => item.UserId == userID);
        }

        public IEnumerable<BlogItemModel> GetItemsByCategory(string Category)
        {
            return _context.BlogInfo.Where(item => item.Category == Category);
        }

        public List<BlogItemModel> GetItemsByTag(string Tag)
        {
            //"Tag1, Tag2, Tag3,Tag4"
            List<BlogItemModel> AllBlogsWithTag = new List<BlogItemModel>();//[]
            var allItems = GetAllBlogItems().ToList();//{Tag:"Tag1, Tag2",Tag:"Tag2",Tag:"tag3"}
            for(int i=0; i < allItems.Count; i++)
            {
                BlogItemModel Item = allItems[i];//{Tag:"Tag1, Tag2"}
                var itemArr = Item.Tags.Split(",");//["Tag1","Tag2"]
                for(int j = 0; j < itemArr.Length; j++)
                {   //Tag1 j = 0
                    //Tag2 j = 1
                    if(itemArr[j].Contains(Tag))
                    {// Tag1               Tag1
                        AllBlogsWithTag.Add(Item);//{Tag:"Tag1, Tag2"}
                    }
                }
            }
            return AllBlogsWithTag;
        }

        public IEnumerable<BlogItemModel> GetItemsByDate(string Date)
        {
            return _context.BlogInfo.Where(item => item.Date == Date);
        }

        public IEnumerable<BlogItemModel> GetPublishedItems()
        {
            return _context.BlogInfo.Where(item => item.IsPublished);
        }

        public bool UpdateBlogItem (BlogItemModel BlogUpdate)
        {
            _context.Update<BlogItemModel>(BlogUpdate);
            return _context.SaveChanges() !=0;
        }

        public bool DeleteBlogItem (BlogItemModel BlogDelete)
        {
             BlogDelete.IsDeleted = true;
            _context.Update<BlogItemModel>(BlogDelete);
            return _context.SaveChanges() !=0;
        }

        public BlogItemModel GetBlogItemById(int Id)
        {
            return _context.BlogInfo.SingleOrDefault(item => item.Id == Id);
        }
    }
}