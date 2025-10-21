using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeScribe.Data;
using SafeScribe.Dtos;
using SafeScribe.Models;
using SafeScribe.Services;

namespace SafeScribe.Controllers
{
    [Route("/api/v1/notas")]
    [ApiController]
    public class NotesController : ControllerBase
    {

        private readonly AppDbContext _context;

        public NotesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Authorize(Roles = "Editor, Admin")]
        /// Cria uma nota em nome do usuário logado
        public async Task<IActionResult> CriarNota([FromBody] NoteCreateDto dto)
        {
            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }

            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
           

            var usuario = await _context.Users.FindAsync(int.Parse(id));

            Console.WriteLine($"User ID capturado: {id}");

            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = int.Parse(id),
                User = usuario
            };

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetNote", new { id = note.Id }, note);
        }


        // GET: api/Notes/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Editor, Admin")]
        public async Task<ActionResult<Note>> GetNote(int id)
        {

            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var note = await _context.Notes.Include(n => n.User) 
                                            .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return NotFound();
            }


            if ((userRole == "Editor" && note.UserId.ToString() != userId))
            {
                return StatusCode(403, new { message = "Usuário não autorizado a visualizar esta nota." });
            }

            return note;
        }

        





        // PUT: api/Notes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Editor, Admin")]
        public async Task<IActionResult> AtualizarNota(int id, [FromBody] NoteCreateDto dto)
        {

            var userRole = User.FindFirstValue(ClaimTypes.Role);
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            

            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            if ((userRole == "Editor" && note.UserId.ToString() != userId))
            {
                return StatusCode(403, new { message = "Usuário não autorizado a visualizar esta nota." });
            }

            // Caso tenha permissão, atualiza os dados
            note.Title = dto.Title;
            note.Content = dto.Content;
            note.UpdatedAt = DateTime.UtcNow;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
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


        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            _context.Notes.Remove(note);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
