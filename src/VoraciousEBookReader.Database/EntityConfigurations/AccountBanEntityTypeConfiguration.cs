//using System.Reflection.Emit;
//using System.Reflection.Metadata;

//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;

//using NetGore.Core.Models;


//namespace NetGore.Data.EntityConfigurations;

///// <summary>
///// Accounts that are baned from the game
///// </summary>
//public class AccountBanEntityTypeConfiguration : IEntityTypeConfiguration<AccountBan>
//{
//    public void Configure(EntityTypeBuilder<AccountBan> builder)
//    {
//        builder?
//            .HasQueryFilter(p => !p.IsDeleted)
//            .HasKey(p => p.Id);

//        builder?
//            .Property(p => p.Id)
//            .IsRequired()
//            .HasComment("The primary Key");

//        builder?
//            .Property(p => p.Name)
//            .HasMaxLength(80)
//            .IsRequired()
//            .HasComment("The name");

//        builder?
//            .Property(p => p.Description)
//            .HasComment("The description");

//        builder?
//            .Property(p => p.UpdatedAt)
//            .IsRequired()
//            .HasComment("The date and time last updated");

//        builder?
//            .Property(p => p.CreatedAt)
//            .IsRequired()
//            .HasComment("The date and time created");

//        builder?
//            .Property(p => p.IsDeleted)
//            .IsRequired()
//            .HasComment("Is it marked for deletion?");

//        builder?
//            .Property(p => p.Account)
//            .IsRequired()
//            .HasComment("The account this ban is for.");

//        builder?
//            .Property(p => p.EndTime)
//            .IsRequired()
//            .HasComment("When this ban ends.");

//        builder?
//            .Property(p => p.Expired)
//            .IsRequired()
//            .HasComment("If the ban is expired.");

//        builder?
//            .Property(p => p.IssuedBy)
//            .IsRequired()
//            .HasMaxLength(50)
//            .HasComment("Name of the person or system that issued this ban (not strongly typed at all).");

//        builder?
//            .Property(p => p.Reason)
//            .IsRequired()
//            .HasComment("The reason why this account was banned.");

//        builder?
//            .Property(p => p.StartTime)
//            .IsRequired()
//            .HasComment("When this ban started.");
//    }
//}