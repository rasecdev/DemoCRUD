using DemoCRUD.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace DemoCRUD.AcessoDados
{
    //Classe para mapear as informações com o Entity.
    public class LivroContexto : DbContext
    {
        public DbSet<Genero> Generos { get; set; }
        public DbSet<Livro> Livros { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {   
            //Remover a convenção de nomes em inglês no Plural
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Properties<String>().Configure(c => c.HasMaxLength(100));
        }
    }
}