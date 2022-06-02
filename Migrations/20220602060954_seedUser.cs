using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apprazor.Migrations
{
    public partial class seedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            for (int i = 1; i < 150; i++)
            {
                migrationBuilder.InsertData(
                    table : "Users",
                    columns: new string[] {
                            "Id",
                            "UserName",
                            "Email",
                            "SecurityStamp",
                            "EmailConfirmed",
                            "PhoneNumberConfirmed",
                            "TwoFactorEnabled",
                            "LockoutEnabled",
                            "AccessFailedCount",
                            "EmailAddress",




                            
                    },
                    values: new object[] {
                        Guid.NewGuid().ToString(),
                        "User" + i.ToString("D3"),
                        $"email{i.ToString("D3")}.@gmail.com",
                        Guid.NewGuid().ToString(),
                        true,
                        false,
                        false,
                        false,
                        0,
                        "...@#%...",
                        
                        








                    }

                );
            }

        }



//       ,[NormalizedUserName]

//       ,[NormalizedEmail]

//       ,[PasswordHash]

//       ,[ConcurrencyStamp]
//       ,[PhoneNumber]
//       ,[PhoneNumberConfirmed]

//       ,[LockoutEnd]
//       ,[LockoutEnabled]
//       ,[AccessFailedCount]
//       ,[EmailAddress]
//       ,[BirthDay]
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
