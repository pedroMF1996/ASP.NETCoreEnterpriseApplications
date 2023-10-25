using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NSE.Carrinho.API.Migrations
{
    public partial class CorrecaoNomeTabelaCarrinhoItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Items_CarrinhoCliente_CarrinhoId",
                table: "Items");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Items",
                table: "Items");

            migrationBuilder.RenameTable(
                name: "Items",
                newName: "CarrinhoItems");

            migrationBuilder.RenameIndex(
                name: "IX_Items_CarrinhoId",
                table: "CarrinhoItems",
                newName: "IX_CarrinhoItems_CarrinhoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarrinhoItems",
                table: "CarrinhoItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarrinhoItems_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItems",
                column: "CarrinhoId",
                principalTable: "CarrinhoCliente",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarrinhoItems_CarrinhoCliente_CarrinhoId",
                table: "CarrinhoItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarrinhoItems",
                table: "CarrinhoItems");

            migrationBuilder.RenameTable(
                name: "CarrinhoItems",
                newName: "Items");

            migrationBuilder.RenameIndex(
                name: "IX_CarrinhoItems_CarrinhoId",
                table: "Items",
                newName: "IX_Items_CarrinhoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Items",
                table: "Items",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Items_CarrinhoCliente_CarrinhoId",
                table: "Items",
                column: "CarrinhoId",
                principalTable: "CarrinhoCliente",
                principalColumn: "Id");
        }
    }
}
