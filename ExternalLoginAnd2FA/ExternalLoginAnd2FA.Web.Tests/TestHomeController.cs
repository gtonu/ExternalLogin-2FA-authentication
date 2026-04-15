using Autofac.Extras.Moq;
using ExternalLoginAnd2FA.Domain.Entities;
using ExternalLoginAnd2FA.Infrastructure.Data;
using ExternalLoginAnd2FA.Web.Controllers;
using Moq;

namespace ExternalLoginAnd2FA.Web.Tests;

public class TestHomeController
{
    private AutoMock _moq;
    private HomeController _homeController;
    private Mock<ApplicationDbContext> _dbContext;
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        _moq = AutoMock.GetLoose();
    }
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _moq?.Dispose();
    }
    [SetUp]
    public void Setup()
    {
        _homeController = _moq.Create<HomeController>();
        _dbContext = _moq.Mock<ApplicationDbContext>();
    }
    [TearDown]
    public void TearDown()
    {
        _homeController?.Dispose();
        _dbContext?.Reset();
    }
    

    [Test]
    public async Task Test1()
    {
        //Arrange
        var store1 = new Store
        {
            Id = Guid.NewGuid(),
            StoreName = "Apple",
            ItemCount = 10
        };

        _dbContext.Setup(x => x.Stores.AddAsync(
            It.Is<Store>(y => y.StoreName == store1.StoreName && y.ItemCount == store1.ItemCount)
            ))
            .Verifiable();
        //Act
        var View =await _homeController.Index();
        //Assert


    }
}
