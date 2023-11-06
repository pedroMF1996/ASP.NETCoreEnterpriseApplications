using Dapper;
using Microsoft.EntityFrameworkCore;
using NSE.Catalogo.API.Models;
using NSE.Core.Data;

namespace NSE.Catalogo.API.Data.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly CatalogoContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public ProdutoRepository(CatalogoContext context)
        {
            _context = context;
        }

        public async Task<Produto> ObterPorId(Guid id)
        {
            return await _context.Produtos.FindAsync(id);
        }

        public async Task<PagedResult<Produto>> ObterTodos(int pageSize, int pageIndex, string query = null)
        {

            var sql = @"SELECT * FROM Produtos 
                        WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%') 
                        ORDER BY [Nome]
                        OFFSET @PageIndex ROWS
                        FETCH NEXT @PageSize ROWS ONLY
                        SELECT COUNT(Id) FROM Produtos WHERE (@Nome IS NULL OR Nome LIKE '%' + @Nome + '%')";

            var multi = await _context.Database.GetDbConnection()
                .QueryMultipleAsync(sql, new {  
                                                Nome = query, 
                                                PageIndex = pageSize * (pageIndex -1), 
                                                PageSize = pageSize 
                                             } 
                                   );

            var produtos = multi.Read<Produto>();

            var total = multi.Read<int>().FirstOrDefault();

            PagedResult<Produto> result = new()
            {
                List = produtos,
                TotalResults = total,
                PageIndex = pageIndex,
                PageSize = pageSize,
                Query = query
            };

            return result;
        }

        public void Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
        }

        public void Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
        }
        public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids)
        {
            var idsGuid = ids.Split(',')
                .Select(id => (Ok: Guid.TryParse(id, out var x), Value: x));

            if (!idsGuid.All(nid => nid.Ok)) return new List<Produto>();

            var idsValue = idsGuid.Select(id => id.Value);

            return await _context.Produtos.AsNoTracking()
                .Where(p => idsValue.Contains(p.Id) && p.Ativo).ToListAsync();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

    }
}
