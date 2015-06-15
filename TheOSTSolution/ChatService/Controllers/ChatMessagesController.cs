using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ChatService.Models;

namespace ChatService.Controllers
{
    public class ChatMessagesController : ApiController
    {
        private ChatServiceContext db = new ChatServiceContext();

        // GET: api/ChatMessages
        public IQueryable<ChatMessage> GetChatMessages()
        {
            return db.ChatMessages;
        }

        // GET: api/ChatMessages/5
        [ResponseType(typeof(ChatMessage))]
        public async Task<IHttpActionResult> GetChatMessage(int id)
        {
            ChatMessage chatMessage = await db.ChatMessages.FindAsync(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            return Ok(chatMessage);
        }

        // PUT: api/ChatMessages/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutChatMessage(int id, ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != chatMessage.MessageId)
            {
                return BadRequest();
            }

            db.Entry(chatMessage).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChatMessageExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ChatMessages
        [ResponseType(typeof(ChatMessage))]
        public async Task<IHttpActionResult> PostChatMessage(ChatMessage chatMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ChatMessages.Add(chatMessage);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = chatMessage.MessageId }, chatMessage);
        }

        // DELETE: api/ChatMessages/5
        [ResponseType(typeof(ChatMessage))]
        public async Task<IHttpActionResult> DeleteChatMessage(int id)
        {
            ChatMessage chatMessage = await db.ChatMessages.FindAsync(id);
            if (chatMessage == null)
            {
                return NotFound();
            }

            db.ChatMessages.Remove(chatMessage);
            await db.SaveChangesAsync();

            return Ok(chatMessage);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ChatMessageExists(int id)
        {
            return db.ChatMessages.Count(e => e.MessageId == id) > 0;
        }
    }
}