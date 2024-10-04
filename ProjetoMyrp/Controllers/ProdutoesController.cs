using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMyrp.Models;
using ProjetoMyrp.Models.Data;

namespace ProjetoMyrp.Controllers;

public class ProdutoesController(ApplicationDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View(await context.Produtos.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await context.Produtos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,Preco,QuantidadeEmEstoque")] Produto produto)
    {
        if (ModelState.IsValid)
        {
            context.Add(produto);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await context.Produtos.FindAsync(id);
        if (produto == null)
        {
            return NotFound();
        }
        return View(produto);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Preco,QuantidadeEmEstoque")] Produto produto)
    {
        if (id != produto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(produto);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var produto = await context.Produtos
            .FirstOrDefaultAsync(m => m.Id == id);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var produto = await context.Produtos.FindAsync(id);
        if (produto != null)
        {
            context.Produtos.Remove(produto);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(int id)
    {
        return context.Produtos.Any(e => e.Id == id);
    }
}