using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Curso.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Curso.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Conversor> Conversores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string strConection = "Data source=(localdb)\\mssqllocaldb;Initial Catalog=DevIO-02;Integrated Security=true;pooling=true";
            optionsBuilder.UseSqlServer(strConection)//Seria as tentativas de refazer a query apos o erro, 1 parametro: Numero de tentativas, 2 parametro: Tempo da retentativa, 3 parametro: Array de Erros, passar null par usr padroes
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)
            .EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AI");//Troca as regras de busca do banco de dados de forma global
            // //RAFAEL --> rafael -- Vai encontrar
            // //João --> Joao -- Vai encontrar
            
            // modelBuilder.Entity<Departamento>().Property(p => p.Descricao).UseCollation("SQL_Latin1_General_CP1_CS_AS");//Troca as regras de busca do banco de dados por entidade

            // modelBuilder.HasSequence<int>("MinhaSequencia", "sequencias")
            //     .StartsAt(1)
            //     .IncrementsBy(2)
            //     .HasMin(1)
            //     .HasMax(10)
            //     .IsCyclic();
            
            // modelBuilder.Entity<Departamento>().Property(p => p.Id).HasDefaultValueSql("NEXT VALUE FOR sequencias.MinhaSequencia");

            // modelBuilder
            //     .Entity<Departamento>()
            //     .HasIndex(p => new { p.Descricao, p.Ativo })//Indices Composto
            //     .HasDatabaseName("idx_meu_indice_composto")//Dar um nome ao indice
            //     .HasFilter("Descricao IS NOT NULL")//Filtrar os campos
            //     .HasFillFactor(80)
            //     .IsUnique();//Para criar ele unico

            // modelBuilder.Entity<Estado>()
            //     .HasData(new []
            //         {
            //             new Estado{Id = 1, Nome = "São Paulo"},
            //             new Estado{Id = 2, Nome = "Sergipe"}
            //         }
            //     );

            // modelBuilder.HasDefaultSchema("cadastros"); //Criacao de Schemas

            // modelBuilder.Entity<Estado>().ToTable("Estados", "SegundoEsquema");
            var conversao = new ValueConverter<Versao, string>(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));
            
            var conversao1 = new EnumToStringConverter<Versao>();

            modelBuilder.Entity<Conversor>()
            .Property(p => p.Versao)
            .HasConversion(conversao1);
            //.HasConversion(conversao);
            //.HasConversion(p => p.ToString(), p => (Versao)Enum.Parse(typeof(Versao), p));
            //.HasConversion<string>();
        }
    }
}