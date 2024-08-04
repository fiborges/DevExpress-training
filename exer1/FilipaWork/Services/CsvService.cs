using FilipaWork.Models;
using System.Globalization;

namespace FilipaWork.Services
{
    public class CsvService
    {
        public List<Aluno> ReadCsv(string filePath)
        {
            // Lógica para ler o arquivo CSV e retornar uma lista de alunos
            var alunos = new List<Aluno>();
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvHelper.CsvReader(reader, CultureInfo.InvariantCulture))
            {
                alunos = csv.GetRecords<Aluno>().ToList();
            }
            return alunos;
        }
    }
}
