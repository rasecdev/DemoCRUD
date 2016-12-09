using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoCRUD.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int AnoEdicao { get; set; }
        public decimal Valor { get; set; }

        public Genero Genero { get; set; }
        public int GeneroId { get; set; }
    }
}