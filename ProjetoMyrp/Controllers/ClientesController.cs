using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoMyrp.Models;
using ProjetoMyrp.Models.Data;

namespace ProjetoMyrp.Controllers;

public class ClientesController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await context.Clientes.ToListAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await context.Clientes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Nome,CPFouCNPJ")] Cliente cliente)
    {
        if (ModelState.IsValid)
        {
            context.Add(cliente);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cliente);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await context.Clientes.FindAsync(id);
        if (cliente == null)
        {
            return NotFound();
        }
        return View(cliente);
    }
        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,CPFouCNPJ")] Cliente cliente)
    {
        if (id != cliente.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                context.Update(cliente);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(cliente.Id))
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
        return View(cliente);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var cliente = await context.Clientes
            .FirstOrDefaultAsync(m => m.Id == id);
        if (cliente == null)
        {
            return NotFound();
        }

        return View(cliente);
    }
        
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var cliente = await context.Clientes.FindAsync(id);
        if (cliente != null)
        {
            context.Clientes.Remove(cliente);
        }

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ClienteExists(int id)
    {
        return context.Clientes.Any(e => e.Id == id);
    }
}