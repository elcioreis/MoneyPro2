using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MoneyPro2.Domain.Entities;

namespace MoneyPro2.API.Data.Mappings;

public class UserLoginMap : IEntityTypeConfiguration<UserLogin>
{
    public void Configure(EntityTypeBuilder<UserLogin> builder)
    {
        // Tabela
        builder.ToTable("UserLogin");

        // Chave primaria
        builder.HasKey(x => x.Id);

        // Identidade
        builder.Property(x => x.Id).ValueGeneratedOnAdd().UseIdentityColumn();

        // Campos
        builder
            .Property(x => x.UserId)
            .IsRequired(true)
            .HasColumnName("UserId")
            .HasColumnType("INT");

        builder
            .Property(x => x.LoginTime)
            .IsRequired(true)
            .HasColumnName("LoginTime")
            .HasColumnType("DATETIME")
            .ValueGeneratedOnAdd()
            .HasDefaultValueSql("GETUTCDATE()");

        // Criando indices com chave multipla
        builder
            .HasIndex(x => new { x.UserId, x.LoginTime }, "IX_UserLogin_UserId_LoginTime")
            .IsClustered(false);
    }
}
