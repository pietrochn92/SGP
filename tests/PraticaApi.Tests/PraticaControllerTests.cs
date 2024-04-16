using Application.DTOs.Pratica;
using Application.Repository.Pratica;
using AutoFixture;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using Moq;
using PraticaApi.Controllers;

namespace PraticaApi.Tests
{
    [TestClass]
    public class PraticaControllerTests
    {
        private readonly Mock<IPraticaRepository> _praticaRepositoryMock;
        private Fixture _fixture;
        private PraticaController _controller;
        private readonly Mock<Microsoft.Extensions.Configuration.IConfiguration> _configuration;
        private readonly Mock<ILogger<PraticaController>> _logger;

        public PraticaControllerTests()
        {
            _fixture = new Fixture();
            _praticaRepositoryMock = new Mock<IPraticaRepository>();
            _configuration = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            _logger = new Mock<ILogger<PraticaController>>();
        }

        [TestMethod]
        public async Task TestGetPraticheMock()
        {
            var praticheMock = _fixture.Build<PraticaDto>().Without(p => p.Allegato).CreateMany(5);
            _praticaRepositoryMock.Setup(repo => repo.GetPratiche()).ReturnsAsync(praticheMock);
            _controller = new PraticaController(_praticaRepositoryMock.Object, _configuration.Object, _logger.Object);
            var result = await _controller.GetPratiche();
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj?.StatusCode);
        }

        [TestMethod]
        public async Task TestGetPraticheThrowExMock()
        {
            _praticaRepositoryMock.Setup(repo => repo.GetPratiche()).ThrowsAsync(new Exception());
            _controller = new PraticaController(_praticaRepositoryMock.Object, _configuration.Object, _logger.Object);
            var result = await _controller.GetPratiche();
            var obj = result as ObjectResult;

            Assert.AreEqual(400, obj?.StatusCode);
        }
    }
}
