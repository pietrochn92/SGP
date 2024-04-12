using Application.DTOs.Pratica;
using Application.Repository.Pratica;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using PraticaApi.Controllers;

namespace PraticaApi.Tests
{
    [TestClass]
    public class PraticaControllerTest
    {
        private readonly PraticaController _controller;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PraticaController> _logger;
        private readonly List<PraticaDto> pratiche =
    [
        new()
        {
            Id = 1,
            IdStatoPratica = 1,
            IdRisultatoPratica = null,
            NomeUtente = "Francesco",
            CognomeUtente = "Verdi",
            CodiceFiscaleUtente = "VRDFNC80A01H501X",
            DataNascitaUtente = new DateTime(1980, 01, 01),
            Allegato = null,
            AllegatoName = "test.pdf"
        },
        new()
        {
            Id = 2,
            IdStatoPratica = 1,
            IdRisultatoPratica = null,
            NomeUtente = "Maria",
            CognomeUtente = "Bianchi",
            CodiceFiscaleUtente = "BNCMRA80A01H501A",
            DataNascitaUtente = new DateTime(1980, 01, 01),
            Allegato = null,
            AllegatoName = "test2.pdf"
        }
    ];

        public PraticaControllerTest()
        {
            var mockRepo = new Mock<IPraticaRepository>();

            mockRepo.Setup(repo => repo.GetPratiche()).Returns(pratiche.AsEnumerable<PraticaDto>);
            mockRepo.Setup(repo => repo.GetPratica(It.IsAny<int>()))
                .Returns<int>(id => Task.FromResult(pratiche.FirstOrDefault(i => i.Id == id)));
            mockRepo.Setup(repo => repo.CreatePratica(It.IsAny<PraticaDto>()))
                .Callback<PraticaDto>(pratiche.Add);
            mockRepo.Setup(repo => repo.UpdatePratica(It.IsAny<PraticaDto>()))
                .Callback<PraticaDto>(i =>
                {
                    var item = pratiche.FirstOrDefault(x => x.Id == i.Id);
                    if (item != null)
                    {
                        item.NomeUtente = i.NomeUtente;
                        item.CognomeUtente = i.CognomeUtente;
                        item.CodiceFiscaleUtente = i.CodiceFiscaleUtente;
                    }
                });
            mockRepo.Setup(repo => repo.DeletePratica(It.IsAny<int>()))
                .Callback<int>(id => pratiche.RemoveAll(i => i.Id == id));

            _controller = new PraticaController(mockRepo.Object, _configuration, _logger);
        }

        [TestMethod]
        public void Test()
        {

        }

        [Fact]
        public void GetPraticheTest()
        {
            var okObjectResult = _controller.GetPratiche();
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult);
            var items = Assert.IsType<List<PraticaDto>>(okResult.Value);
            Assert.Equal(2, items.Count);
        }

        [Fact]
        public void GetPraticaTest()
        {
            var id = 2;
            var okObjectResult = _controller.GetPratica(id);
            var okResult = Assert.IsType<OkObjectResult>(okObjectResult);
            var item = Assert.IsType<PraticaDto>(okResult.Value);
            Assert.Equal(id, item.Id);
        }

        [Fact]
        public void CreatePraticaTest()
        {
            var createdResponse = _controller.CreatePratica(
                new PraticaDto
                {
                    Id = 3,
                    IdStatoPratica = 1,
                    IdRisultatoPratica = null,
                    NomeUtente = "Vittoria",
                    CognomeUtente = "Gialli",
                    CodiceFiscaleUtente = "GLLVTR80A41H501W",
                    DataNascitaUtente = new DateTime(1980, 01, 01),
                    Allegato = null,
                    AllegatoName = "test3.pdf"
                }
            );
            var response = Assert.IsType<CreatedAtActionResult>(createdResponse);
            var item = Assert.IsType<PraticaDto>(response.Value);
            Assert.Equal("Vittoria", item.NomeUtente);
        }

        [Fact]
        public void UpdatePraticaTest()
        {
            var id = 2;
            var okObjectResult = _controller.UpdatePratica(id,
                new PraticaDto
                {
                    IdStatoPratica = 1,
                    NomeUtente = "Federica",
                    CodiceFiscaleUtente = "BNCFRC80A41H501Z"
                });
            Assert.IsType<OkResult>(okObjectResult);
            var pratica = pratiche.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(pratica);
            Assert.Equal("Federica", pratica.NomeUtente);
            okObjectResult = _controller.UpdatePratica(id, null);
            Assert.IsType<NoContentResult>(okObjectResult);
        }

        [Fact]
        public void DeleteCatalogItemTest()
        {
            var id = 1;
            var pratica = pratiche.FirstOrDefault(i => i.Id == id);
            Assert.NotNull(pratica);
            var okObjectResult = _controller.DeletePratica(id);
            Assert.IsType<OkResult>(okObjectResult);
            pratica = pratiche.FirstOrDefault(i => i.Id == id);
            Assert.Null(pratica);
        }
    }
}
