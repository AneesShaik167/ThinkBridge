using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThinkBridge.Models;
using ThinkBridge.Controllers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using static ThinkBridge.Controllers.ItemController;
using System.Web.Http;
using System.Web.Http.Results;

namespace ThinkBridge.Tests
{
    [TestClass]
    public class TestItemController
    {
        [TestMethod]
        public async Task GetAllItemsAsync_ShouldReturnAllItems()
        {
            var testItems = GetTestItems();
            var controller = new ItemController(testItems);

            var result = controller.GetAllItems() as List<Item>;
            Assert.AreEqual(testItems.Count, result.Count);
        }

        [TestMethod]
        public async Task DeleteReturnsOkAsync()
        {
            // Arrange
            var mockRepository = new Mock<IProductRepository>();
            var controller = new ItemController(mockRepository.Object);

            // Act
            IHttpActionResult actionResult = await controller.DeleteItemAsync("4");

            // Assert
            Assert.IsInstanceOfType(actionResult, typeof(OkResult));
        }
        private List<Item> GetTestItems()
        {
            var testItems = new List<Item>();
            testItems.Add(new Item { Id = "1", Name = "Demo1", Price = "1", Description = "Demo1Description" });
            testItems.Add(new Item { Id = "2", Name = "Demo2", Price = "3.75", Description = "Demo2Description" });
            testItems.Add(new Item { Id = "3", Name = "Demo3", Price = "16.99", Description = "Demo3Description" });
            testItems.Add(new Item { Id = "4", Name = "Demo4", Price = "11.00", Description = "Demo4Description" });

            return testItems;
        }

    }
}
