using DevExpress.XtraReports.UI;
using FilipaWork.Models;
using FilipaWork.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FilipaWork.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly CsvService _csvService;
        private readonly string _filePath;

        public AlunosController(CsvService csvService)
        {
            _csvService = csvService;
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "alunos.csv");
        }

        [HttpGet]
        public ActionResult<List<Aluno>> GetAlunos()
        {
            var alunos = _csvService.ReadCsv(_filePath);
            return Ok(alunos);
        }

        [HttpGet("media")]
        public ActionResult<double> GetMedia()
        {
            var alunos = _csvService.ReadCsv(_filePath);
            if (alunos.Count == 0) return Ok(0);

            double media = alunos.Average(a => a.Nota);
            return Ok(media);
        }

        [HttpGet("evolucao")]
        public ActionResult<IEnumerable<object>> GetEvolucao()
        {
            var alunos = _csvService.ReadCsv(_filePath);
            var evolucao = alunos
                .GroupBy(a => a.Data)
                .Select(g => new
                {
                    Data = g.Key,
                    Media = g.Average(a => a.Nota)
                })
                .OrderBy(e => e.Data);

            return Ok(evolucao);
        }

        [HttpGet("report")]
        public IActionResult GetReport()
        {
            var alunos = _csvService.ReadCsv(_filePath);
            var report = new AlunosReport();
            report.LoadData(alunos);

            using (var stream = new MemoryStream())
            {
                report.ExportToPdf(stream);
                stream.Position = 0;

                return new FileStreamResult(new MemoryStream(stream.ToArray()), "application/pdf")
                {
                    FileDownloadName = "report.pdf"
                };
            }
        }
    }
}