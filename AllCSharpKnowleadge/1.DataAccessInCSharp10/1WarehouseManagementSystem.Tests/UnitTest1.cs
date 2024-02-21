using _1WarehouseManagementSystem.Infrastructure;
using Moq;
using WarehouseManagementSystem.Infrastructure.Data;
using WarehouseManagementSystem.Web.Controllers;
using WarehouseManagementSystem.Web.Models;

namespace _1WarehouseManagementSystem.Tests
{
    [TestClass]
    public class OrderControllerTests
    {
        [TestMethod]
        public void CanCreateOrderWithCorrectModel()
        {
            #region test using some specified repository
            ////ARRANGE
            //var orderRepository = new Mock<IUnitOfWork>();
            //var itemRepository = new Mock<IRepository<Item>>();
            //var shippingProviderRepository = new Mock<IRepository<ShippingProvider>>();

            //shippingProviderRepository.Setup(repository => repository.All())
            //    .Returns(new[] { new ShippingProvider() });

            //var orderController = new OrderController(orderRepository.Object, shippingProviderRepository.Object, itemRepository.Object);

            //var createOrderModel = new CreateOrderModel
            //{
            //    Customer = new()
            //    {
            //        Address = "31 tho nhuom",
            //        Country = "Vietnam",
            //        Name = "Thu Thin",
            //        PhoneNumber = "1234567890",
            //        PostalCode = "123456"
            //    },
            //    LineItems = new[]
            //    {
            //        new LineItemModel
            //        {
            //            ItemId = Guid.NewGuid(),
            //            Quantity = 100
            //        }
            //    }
            //};

            ////ACT
            //orderController.Create(createOrderModel);

            ////ASSERT
            //orderRepository.Verify(
            //    repository => repository.Add(It.IsAny<Order>()),
            //    Times.AtMostOnce()
            //    );

            #endregion

        }
    }
}