using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication12_bulkExtensions.Data;
using WebApplication12_bulkExtensions.Models;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;

namespace WebApplication12_bulkExtensions.Controllers
{
    public class StatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment _hostingEnvironment;

        public StatesController(ApplicationDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: States
        public async Task<IActionResult> Index()
        {
            return View(await _context.States.ToListAsync());
        }

        // GET: States/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .FirstOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // GET: States/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: States/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Abbreviation,Type,Country,Region,RegionName,Division,DivisionName,Flag")] State state)
        {
            if (ModelState.IsValid)
            {
                _context.Add(state);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(state);
        }

        // GET: States/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            return View(state);
        }

        // POST: States/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Abbreviation,Type,Country,Region,RegionName,Division,DivisionName,Flag")] State state)
        {
            if (id != state.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(state);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StateExists(state.Id))
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
            return View(state);
        }

        // GET: States/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var state = await _context.States
                .FirstOrDefaultAsync(m => m.Id == id);
            if (state == null)
            {
                return NotFound();
            }

            return View(state);
        }

        // POST: States/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var state = await _context.States.FindAsync(id);
            _context.States.Remove(state);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StateExists(string id)
        {
            return _context.States.Any(e => e.Id == id);
        }

        // GET: States/BulkCreate
        public IActionResult BulkCreate()
        {
            return View();
        }

        // POST: States/BulkCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreate(String fileLocation)
        {
            List<State> statesToImport = new List<State>();
            string filePath = string.Empty;

            if (fileLocation != null)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                filePath = webRootPath + "\\data\\" + fileLocation;

                string fileData = await System.IO.File.ReadAllTextAsync(filePath);

                foreach (string row in fileData.Split('\n'))
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        Guid obj = Guid.NewGuid();

                        statesToImport.Add(new State
                        {
                           
                            Id = Convert.ToString(obj), //Convert.ToString(new Guid()),
                            Name = row.Split(',')[0],
                            Abbreviation = row.Split(',')[1],
                            Type = row.Split(',')[2],
                            Country = row.Split(',')[3]
                            // Region = Convert.ToInt32(row.Split(',')[5]),
                            // RegionName = row.Split(',')[6],
                            // Division = Convert.ToInt32(row.Split(',')[7]),
                            // DivisionName = row.Split(',')[8]
                        });
                    }
                }
                // logically, we want to toss back a list view of what the data looks like, then the user can confirm the transaction
                // similar to a delete function, but with a table view of things....

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.BulkInsert(statesToImport);
                    transaction.Commit();
                }
            }

            return View();
            
        }

        // GET: States/BulkCreate
        public IActionResult BulkCreateJson()
        {
            return View();
        }

        // POST: States/BulkCreate
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BulkCreateJson(String fileLocation)
        {
            List<State> statesToImport = new List<State>();
            string filePath = string.Empty;

            if (fileLocation != null)
            {
                string webRootPath = _hostingEnvironment.WebRootPath;
                filePath = webRootPath + "\\data\\" + fileLocation;

                string fileData = await System.IO.File.ReadAllTextAsync(filePath);



                var jsonData = JsonConvert.DeserializeObject<List<StateJson>>(fileData);

                foreach (var state in jsonData)
                {
                    if (state != null)
                    {
                        Guid obj = Guid.NewGuid();

                        statesToImport.Add(new State
                        {

                            Id = Convert.ToString(obj), //Convert.ToString(new Guid()),
                            Name = state.Name,
                            Abbreviation = state.Abbreviation,
                            Type = state.Type,
                            Country = state.Country,
                            Region = Convert.ToInt32(state.Region),
                            // RegionName = row.Split(',')[6],
                            // Division = Convert.ToInt32(row.Split(',')[7]),
                            // DivisionName = row.Split(',')[8]
                        });
                    }
                }

                statesToImport = statesToImport.OrderBy(x => x.Name).ToList();

                using (var transaction = _context.Database.BeginTransaction())
                {
                    _context.BulkInsertOrUpdate(statesToImport, new BulkConfig { PreserveInsertOrder = true });
                    transaction.Commit();
                }
            }

            return View();

        }




    }
}
