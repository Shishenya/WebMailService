using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace mails.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        private readonly TodoContext _context;

        public MailsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Mails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mails>>> GetMails()
        {
            if (_context.Mails == null)
            {
                return NotFound();
            }
            return await _context.Mails.ToListAsync();
        }

        // GET: api/Mails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mails>> GetMails(long id)
        {
            if (_context.Mails == null)
            {
                return NotFound();
            }
            var mails = await _context.Mails.FindAsync(id);

            if (mails == null)
            {
                return NotFound();
            }

            return mails;
        }

        // PUT: api/Mails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMails(long id, Mails mails)
        {
            if (id != mails.Id)
            {
                return BadRequest();
            }

            _context.Entry(mails).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MailsExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Mails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MailsShort>> PostMails(MailsShort mailsShort)
        {
            
            // реализация отправки письма
            MailsSend mailsSend = new MailsSend(ParametersMails.UserMail, ParametersMails.UserPassword, ParametersMails.StmpClient, ParametersMails.SmtpPort);
            var sendMailResult = mailsSend.SendMyEmail(mailsShort.Subject, mailsShort.Body, mailsShort.Recipients);

            // Дополнительные поля
            string result = sendMailResult.Item1;
            string failedMessage = sendMailResult.Item2;
            DateTime dateMail = DateTime.Now;
            
            // Формируем запись в БД
            Mails mails = new Mails
            {
                Id = 0,
                Subject = mailsShort.Subject,
                Body = mailsShort.Body,
                Recipients= mailsShort.Recipients,
                DateMail = dateMail,
                Result = result,
                FailedMessage = failedMessage
            };

            if (_context.Mails == null)
            {
                return Problem("Entity set 'TodoContext.Mails'  is null.");
            }
            _context.Mails.Add(mails);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMails), new { id = mails.Id }, mailsToShort(mails));
        }

        // Преобразование 
        private static MailsShort mailsToShort(Mails mails) => new MailsShort
        {
            // Id = mails.Id,
            Subject = mails.Subject,
            Body = mails.Body,
            Recipients = mails.Recipients,
        };

        // DELETE: api/Mails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMails(long id)
        {
            if (_context.Mails == null)
            {
                return NotFound();
            }
            var mails = await _context.Mails.FindAsync(id);
            if (mails == null)
            {
                return NotFound();
            }

            _context.Mails.Remove(mails);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MailsExists(long id)
        {
            return (_context.Mails?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
