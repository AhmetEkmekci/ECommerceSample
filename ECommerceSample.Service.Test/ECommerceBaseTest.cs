using ECommerceSample.Data;
using ECommerceSample.Data.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ECommerceSample.Service.Test
{
    [TestClass]
    public class ECommerceBaseTest
    {
        readonly InMemoryECommerceDataSource dataSource;
        readonly ProductRepository productRepository;
        readonly OrderRepository orderRepository;
        readonly CampaignRepository campaignRepository;
        readonly ECommerceBase eCommerceBase;
        DateTime SystemDate { get; set; }

        public ECommerceBaseTest()
        {
            dataSource = new Data.InMemoryECommerceDataSource();
            productRepository = new Data.Repository.ProductRepository(dataSource);
            orderRepository = new Data.Repository.OrderRepository(dataSource);
            campaignRepository = new Data.Repository.CampaignRepository(dataSource);

            SystemDate = DateTime.Now;

            eCommerceBase = new ECommerceSample.Service.ECommerceBase(
                    SampleData.LogParser, SystemDate, productRepository, orderRepository, campaignRepository
                );
        }

        [DataRow("CommandSet1", true)]
        [DataRow("CommandSet2", true)]
        [DataRow("CommandSet3", false)]
        [DataRow("CommandSet4", true)]
        [DataRow("CommandSet5", true)]
        [TestMethod]
        public void RunBatch_ShouldRunCommands(string fileName, bool expectedResult)
        {
            //Arrange
            var commandSet = SampleData.CommandDictionary[fileName].ToArray();

            //Act
            var result = eCommerceBase.RunBatch(commandSet);

            //Assert
            Assert.AreEqual(result, expectedResult);
        }

        [DataRow("P1", "5", "10")]
        [TestMethod]
        public void CreateProduct_ShouldReturn_True(string Code, string Price, string Stock)
        {
            //Arrange
            var parameter = new List<string>()
            { Code, Price, Stock };

            //Act
            var result = eCommerceBase.CreateProduct(parameter);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateCampaign_ShouldReturn_True()
        {
            //Arrange
            eCommerceBase.CreateProduct(new List<string>() { "P1", "5", "5" });
            var parameter = new List<string>()
            {
                "C1", "P1", "5", "5", "5"
            };

            //Act
            var result = eCommerceBase.CreateCampaign(parameter);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CreateOrder_ShouldReturn_True()
        {
            //Arrange
            eCommerceBase.CreateProduct(new List<string>() { "P1", "5", "5" });
            var parameter = new List<string>()
            {
                "P1", "5"
            };

            //Act
            var result = eCommerceBase.CreateOrder(parameter);

            //Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [DataRow("P1", "10", "5", true)]
        [DataRow("P1", "10", "10", true)]
        [DataRow("P1", "10", "15", false)]
        public void Order_ShouldReturn_ExpectedValue(string productCode, string stock, string quantity, bool expected)
        {
            //Arrange
            eCommerceBase.CreateProduct(new List<string>() { productCode, "1", stock });
            var parameter = new List<string>() { productCode, quantity };

            //Act
            var result = eCommerceBase.CreateOrder(parameter);

            //Assert
            Assert.AreEqual(result, expected);
        }
    }
}
