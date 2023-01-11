using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace FilmesApi.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class FilmeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FilmeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("MostrarFilmes")]
        public IEnumerable<Filme> MostrarFilmes([FromQuery] int skip, [FromQuery] int take = 50)
        {
            List<Filme> filmes = new List<Filme>();
            string connect = _configuration.GetConnectionString("DefaultConnection").ToString();

            using (SqlConnection con = new SqlConnection(connect))
            {
                con.Open();
                string query = "SELECT * FROM Filme";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var filme = new Filme()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Titulo = reader["Titulo"].ToString(),
                            Genero = reader["Genero"].ToString(),
                            Duracao = Convert.ToInt32(reader["Duracao"])
                        };

                        filmes.Add(filme);
                    }
                }
                con.Close();
                return filmes;
            }
        }

        [HttpGet("MostrarFilme")]
        public List<Filme> MostrarFilme([FromQuery] int id)
        {
            List<Filme> filmes = new List<Filme>();
            string connect = _configuration.GetConnectionString("DefaultConnection").ToString();

            using (SqlConnection con = new SqlConnection(connect))
            {
                con.Open();
                string query = "SELECT * FROM Filme where id = @id";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        var filme = new Filme()
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Titulo = reader["Titulo"].ToString(),
                            Genero = reader["Genero"].ToString(),
                            Duracao = Convert.ToInt32(reader["Duracao"])
                        };

                        filmes.Add(filme);
                    }
                }
                con.Close();
                return filmes;
            }

        }

        [HttpPost("AdicionarFilme")]
        public Filme AdicionarFilme([FromBody] Filme filme)
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
            return filme;
        }

        [HttpPut("EditarFilme")]
        public void EditarFilme([FromQuery] int id, [FromBody] Filme filme)
        {
            string connect = _configuration.GetConnectionString("DefaultConnection").ToString();

            using (SqlConnection con = new SqlConnection(connect))
            {
                con.Open();
                string query = "UPDATE Filme SET titulo = @Titulo, genero = @Genero, duracao = @Duracao WHERE id = " + id;
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Titulo", filme.Titulo.ToString());
                    cmd.Parameters.AddWithValue("@Genero", filme.Genero.ToString());
                    cmd.Parameters.AddWithValue("@Duracao", int.Parse(filme.Duracao.ToString()));
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        [HttpDelete("ExcluirFilme")]
        public void ExcluirFilme ([FromQuery] int id)
        {
            string connect = _configuration.GetConnectionString("DefaultConnection").ToString();

            using (SqlConnection con = new SqlConnection(connect))
            {
                con.Open();
                string query = "DELETE FROM Filme WHERE id = @id";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@id", id);
                    var reader = cmd.ExecuteReader();
                }
                con.Close();
            }
        }
    }
}