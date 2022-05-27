using System;
using Bogus;
using Microsoft.EntityFrameworkCore.Migrations;
using razorweb.models;

#nullable disable

namespace apprazor.Migrations
{
    public partial class initdb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Content = table.Column<string>(type: "ntext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articles", x => x.Id);
                });
                // insert data 
                Randomizer.Seed = new Random(8675309);
                var fakerarticle = new Faker<Article>();
                fakerarticle.RuleFor(a => a.Title,f => f.Lorem.Sentence(5,5));
                fakerarticle.RuleFor(a => a.Created, f => f.Date.Between(new DateTime(2021,1,1),new DateTime(2021,7,30)));
                fakerarticle.RuleFor(a => a.Content,f => f.Lorem.Paragraphs(1,4));
              Article  article =   fakerarticle.Generate();




                for (int i = 0; i < 150; i++)
                {
                    migrationBuilder.InsertData(
                    table : "articles",
                    columns:  new[] {"Title","Created","Content"},
                    values : new Object[] {
                       article.Title,
                        article.Created,
                           article.Content
                    }
                );
                }
                  migrationBuilder.InsertData(
                    table : "articles",
                    columns:  new[] {"Title","Created","Content"},
                    values : new Object[] {
                        "bai viet 2",
                            new DateTime(2022,10,5),
                            "noi dung 2"
                    }
                );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "articles");
        }
    }
}
