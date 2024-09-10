using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjetoMyrp.Data;
using ProjetoMyrp.Models;

namespace ProjetoMyrp.Controllers
{
    public class VendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vendas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Vendas.Include(v => v.Cliente).Include(v => v.Produto);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Vendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
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

        // GET: Vendas/Create
        public IActionResult Create()
        {
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "CPFouCNPJ");
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome");
            return View();
        }

        // POST: Vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProdutoId,ClienteId,Quantidade,DataVenda")] Venda venda)
        {
            if (ModelState.IsValid)
            {
                // Verificar se o produto existe
                var produto = await _context.Produtos.FindAsync(venda.ProdutoId);
                if (produto == null)
                {
                    ModelState.AddModelError("", "Produto não encontrado.");
                    // Recarregar os dados dos dropdowns e retornar a view com os erros
                    ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "CPFouCNPJ", venda.ClienteId);
                    ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", venda.ProdutoId);
                    return View(venda);
                }

                // Atribuir o produto encontrado à venda
                venda.Produto = produto;

                // Adicionar a venda ao contexto e salvar as mudanças
                _context.Add(venda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Se o modelo não for válido, recarregar os dados dos dropdowns e retornar a view com erros
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "CPFouCNPJ", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }



        // GET: Vendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            // Adicionar o valor total à ViewData para ser utilizado na view
            ViewData["ValorTotal"] = venda.ValorTotal;

            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "CPFouCNPJ", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }


        // POST: Vendas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProdutoId,ClienteId,Quantidade,DataVenda")] Venda venda)
        {
            if (id != venda.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VendaExists(venda.Id))
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
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "Id", "CPFouCNPJ", venda.ClienteId);
            ViewData["ProdutoId"] = new SelectList(_context.Produtos, "Id", "Nome", venda.ProdutoId);
            return View(venda);
        }

        // GET: Vendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venda = await _context.Vendas
                .Include(v => v.Cliente)
                .Include(v => v.Produto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (venda == null)
            {
                return NotFound();
            }

            return View(venda);
        }

        // POST: Vendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venda = await _context.Vendas.FindAsync(id);
            if (venda != null)
            {
                _context.Vendas.Remove(venda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VendaExists(int id)
        {
            return _context.Vendas.Any(e => e.Id == id);
        }
    }
}
