using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoMyrp.Models;
using ProjetoMyrp.Models.Data;

namespace ProjetoMyrp.Controllers;

public class VendasController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var applicationDbContext = context.Vendas.Include(v => v.Cliente).Include(v => v.Produto);
        return View(await applicationDbContext.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var venda = await context.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.Produto)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (venda == null)
        {
            return NotFound();
        }

        // Adicione o valor total à ViewData para ser utilizado na view
        ViewData["ValorTotal"] = venda.ValorTotal;

        return View(venda);
    }

    [HttpGet]
    public IActionResult Create()
    {
        ViewData["ClienteId"] = new SelectList(context.Clientes, "Id", "CPFouCNPJ");
        ViewData["ProdutoId"] = new SelectList(context.Produtos, "Id", "Nome");
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,ProdutoId,ClienteId,Quantidade,DataVenda")] Venda venda)
    {
        var produto = await context.Produtos.FindAsync(venda.ProdutoId);
        if (produto == null)
        {
            ModelState.AddModelError("", "Produto não encontrado.");
            // Recarregar os dados dos dropdowns e retornar a view com os erros
            ViewData["ClienteId"] = new SelectList(context.Clientes, "Id", "CPFouCNPJ", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(context.Produtos, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        produto.QuantidadeEmEstoque -= venda.Quantidade;
        venda.Produto = produto;
        
        context.Add(venda);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var venda = await context.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.Produto)
            .FirstOrDefaultAsync(m => m.Id == id);
        if (venda == null)
        {
            return NotFound();
        }

        // Adicionar o valor total à ViewData para ser utilizado na view
        ViewData["ValorTotal"] = venda.ValorTotal;
        ViewData["ClienteId"] = new SelectList(context.Clientes, "Id", "CPFouCNPJ", venda.ClienteId);
        ViewData["ProdutoId"] = new SelectList(context.Produtos, "Id", "Nome", venda.ProdutoId);
        return View(venda);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,ClienteId,Quantidade,DataVenda")] Venda venda)
    {
        if (id != venda.Id)
            return NotFound();
        
        if (!VendaExists(venda.Id))
            return NotFound();

        context.Update(venda);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();

        var venda = await context.Vendas
            .Include(v => v.Cliente)
            .Include(v => v.Produto)
            .AsSplitQuery()
            .FirstOrDefaultAsync(m => m.Id == id);
        
        if (venda == null)
            return NotFound();

        return View(venda);
    }
    
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var venda = await context.Vendas.Include(x => x.Produto).FirstOrDefaultAsync(x => x.Id == id);
        if (venda == null)
            return NotFound();
        
        venda.Produto.QuantidadeEmEstoque += venda.Quantidade;
        context.Vendas.Remove(venda);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool VendaExists(int id)
    {
        return context.Vendas.Any(e => e.Id == id);
    }
}