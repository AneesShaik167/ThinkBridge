using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.Entity;
using System.Threading.Tasks;
using ThinkBridge.Models;

namespace ThinkBridge.Controllers
{
    public class ItemController : ApiController
    {
        IProductRepository _repository;

        public ItemController(IProductRepository repository)
        {
            _repository = repository;
        }
        List<Item> Items = new List<Item>();

        public ItemController() { }

        public ItemController(List<Item> Items)
        {
            this.Items = Items;
        }
        public IEnumerable<Item> GetAllItems()
        {
            return Items;
        }
        //GetAllItems
        public async Task<IEnumerable<ItemViewModel>> GetAllItemsAsync()
        {
            IList<ItemViewModel> items = null;
            try
            {
                using (var ctx = new thinkbridgeEntities())
                {
                    items = await ctx.Items.Select(i => new ItemViewModel()
                    {
                        Id = i.Id,
                        Name = i.Name,
                        Price = i.Price,
                        Description = i.Description
                    }).ToListAsync<ItemViewModel>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.Message);
            }

            return items;
        }

        //Post an Item
        public async Task<IHttpActionResult> CreateItemAsync(Item item)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data.");

            using (var ctx = new thinkbridgeEntities())
            {
                ctx.Items.Add(new Item()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Description = item.Description
                });

                ctx.SaveChanges();
            }

            return Ok();
        }

        //Update an Item
        [HttpPut]
        public async Task<IHttpActionResult> UpdateItemAsync(Item item)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            using (var ctx = new thinkbridgeEntities())
            {
                var existingItem = ctx.Items.Where(i => i.Id == item.Id)
                                                        .FirstOrDefault<Item>();

                if (existingItem != null)
                {
                    existingItem.Price = item.Price;
                    existingItem.Description = item.Description;

                    ctx.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }

            return Ok();
        }

        //Delete an item
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteItemAsync(string id)
        {
            if (Convert.ToInt32(id) <= 0)
                return BadRequest("Not a valid student id");

            using (var ctx = new thinkbridgeEntities())
            {
                var item = ctx.Items
                    .Where(i => i.Id == id)
                    .FirstOrDefault();

                ctx.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }

            return Ok();
        }

        public interface IProductRepository
        {
            IEnumerable<Item> Items { get; }
        }
    }
}
