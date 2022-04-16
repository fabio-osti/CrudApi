using CrudApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudApi.Contexts
{
	public class PersonContext : DbContext
	{
		public PersonContext(DbContextOptions<PersonContext> options) : base(options)
		{ }

		public DbSet<Person>? Persons { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			var person = modelBuilder.Entity<Person>();
			person.Metadata.SetTableName("PERSONS");
			person.HasKey(e => e.Id).HasName("ID");
			person.Property(e => e.Name).HasColumnName("NAME").HasMaxLength(64).IsRequired();
			person.Property(e => e.Sex).HasColumnName("SEX").IsRequired();
			person.Property(e => e.Age).HasColumnName("AGE").HasConversion<byte>();
			person.Property(e => e.HairColor).HasColumnName("HAIR_COLOR").HasConversion(hc => (byte)hc!, hc => (HairColor)hc);
		}
	}
}