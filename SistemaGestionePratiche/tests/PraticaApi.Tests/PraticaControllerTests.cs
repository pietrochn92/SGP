using Application.DTOs.Pratica;
using Application.Repository.Pratica;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PraticaApi.Controllers;

namespace PraticaApi.Tests
{
    public class PraticaControllerTests
    {
        private readonly Mock<IPraticaRepository> _praticaRepositoryMock;
        private Fixture _fixture;
        private PraticaController _controller;

        public PraticaControllerTests()
        {
            _fixture = new Fixture();
            _praticaRepositoryMock = new Mock<IPraticaRepository>();
        }

        [TestMethod]
        public async Task TestGetPraticheMock()
        {
            var praticheMock = _fixture.CreateMany<PraticaDto>(5);
            _praticaRepositoryMock.Setup(repo => repo.GetPratiche()).Returns(praticheMock.AsEnumerable<PraticaDto>);

            var result = await _controller.GetPratiche();
            var obj = result as ObjectResult;

            Assert.AreEqual(200, obj?.StatusCode);
        }
    }
}
