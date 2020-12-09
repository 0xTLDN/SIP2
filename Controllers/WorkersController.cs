using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System.Net;
using System.Net.Mail;

namespace WebApplication1.Controllers
{
    public class WorkersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Workers
        //public async Task<IActionResult> Index()
        //{
        //    var applicationDbContext = _context.Worker.Include(w => w.Project);
        //    return View(await applicationDbContext.ToListAsync());
        //}

        //[HttpGet]
        public async Task<IActionResult> Index(string searchWord)
        {
            ViewData["GetDataFromSearch"] = searchWord;
            var query = from w in _context.Worker
                        join p in _context.Project on w.ProjectId equals p.Id
                        select new Worker { FirstName = w.FirstName, ProjectId = w.ProjectId, Project = p, Email = w.Email, Id = w.Id, Job = w.Job, LastName = w.LastName, PhoneNumber = w.PhoneNumber, YearsOfExperience = w.YearsOfExperience };

            if (!String.IsNullOrEmpty(searchWord))
            {
                query = query.Where(o => o.FirstName.Contains(searchWord) || 
                                            o.LastName.Contains(searchWord) ||
                                            o.Project.Name.Contains(searchWord) ||
                                            o.Email.Contains(searchWord));
            }
            return View(await query.ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .Include(w => w.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name");
            return View();
        }

        // POST: Workers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProjectId,FirstName,LastName,Email,PhoneNumber,Job,YearsOfExperience")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                _context.Add(worker);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", worker.ProjectId);
            return View(worker);
        }

        // GET: Workers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", worker.ProjectId);
            return View(worker);
        }

        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProjectId,FirstName,LastName,Email,PhoneNumber,Job,YearsOfExperience")] Worker worker)
        {
            if (id != worker.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Project, "Id", "Name", worker.ProjectId);
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .Include(w => w.Project)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var worker = await _context.Worker.FindAsync(id);
            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(int id)
        {
            return _context.Worker.Any(e => e.Id == id);
        }

        // GET: Workers/SendEmail/
        public IActionResult SendEmail(int id)
        {
            try
            {
                MimeMessage message = new MimeMessage();
            
                MailboxAddress fromAddress = new MailboxAddress("Admin", "testdevadress9955@gmail.com");
                message.From.Add(fromAddress);

                // Get worker from ID
                Worker worker = _context.Worker.FirstOrDefault(x => x.Id == id);

                MailboxAddress toAddress = new MailboxAddress(worker.FirstName, worker.Email);
                message.To.Add(toAddress);

                message.Subject = "Test email for ASP.net Core with MailKit";

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = (@"<h2><center> HELLO </center></h2>
                <b>You just received this automated email to ask for something using <u>HTMLBODY</u></b>
                <br />
                <i>Kind Regars</i>");

                message.Body = bodyBuilder.ToMessageBody();

                MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();

                smtpClient.Connect("smtp.gmail.com", 587);
                smtpClient.Authenticate("testdevadress9955@gmail.com", "Destiny9955");
                smtpClient.Send(message);
                smtpClient.Disconnect(true);
                smtpClient.Dispose();

                TempData["alertMessage"] = "Whatever you want to alert the user with";
                return View();
            }
            catch
            {
                return Content("<script>alert('Email Not Sent');</script>");
            }
        }


    }
}
