using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using WeatherForecast.Models;
using Moq;
using WeatherForecast.Data;
using WeatherForecast.Mvc.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace WeatherForecast.Test
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void IndexMethodShouldReturnProperNumberOfBugs()
        {
            var list = new List<Town>();
            list.Add(new Town());
            list.Add(new Town());

            var townRepoMOck = new Mock<IRepository<Town>>();
            townRepoMOck.Setup(x => x.All()).Returns(list.AsQueryable());

            var uofMock = new Mock<IUowData>();
            uofMock.Setup(x => x.Towns).Returns(townRepoMOck.Object);

            var controller = new HomeController(uofMock.Object);
            var viewResult = controller.Index();
            Assert.IsNotNull(viewResult, "Index action returns null.");
            var model = viewResult as IEnumerable<Town>;
            Assert.IsNotNull(model, "The model is null.");
            Assert.AreEqual(2, model.Count());
        }
    }
}
