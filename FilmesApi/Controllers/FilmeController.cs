using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FilmesApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private static List<Filme> filmes = new List<Filme>();
        private readonly IConfiguration _configuration;
        private static int id = 0;

        [HttpGet]
        public IEnumerable<Filme> MostrarFilmes([FromQuery] int skip, [FromQuery] int take = 50)
        {
            return filmes.Skip(skip).Take(take);
        }

        [HttpGet("{id}")]
        public IActionResult MostrarFilme(int id)
        {
            var filme = filmes.FirstOrDefault(filme => filme.Id == id);
            if (filme == null) return NotFound();
            return Ok(filme);
        }

        [HttpPost]
        public Filme AdicionaFilme([FromBody] Filme filme)
        {
            string connect = _configuration.GetConnectionString("DefaultConnection").ToString();


            using (SqlConnection con = new SqlConnection(connect))
            {
                string query = "INSERT INTO Filme VALUES (@Titulo, @Genero, @Duracao)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Titulo", filme.Titulo.ToString());
                    cmd.Parameters.AddWithValue("@Genero",filme.Genero.ToString());
                    cmd.Parameters.AddWithValue("@Duracao",int.Parse(filme.Duracao.ToString()));
                    cmd.ExecuteNonQuery();

                    con.Close();
                }
            };

            //filmes.Add(filme);
            return filme;
        }
    }
}