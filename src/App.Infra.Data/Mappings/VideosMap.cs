using App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace App.Infra.Data.Mappings
{
    public class VideosMap : IEntityTypeConfiguration<VideoBD>
    {
        public VideosMap()
        {

        }
        public void Configure(EntityTypeBuilder<VideoBD> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(p => p.DataCadastro)
               .IsRequired();


            builder.Property(p => p.Status)
                .IsRequired();

            builder.ToTable("Videos");
        }
    }
}
