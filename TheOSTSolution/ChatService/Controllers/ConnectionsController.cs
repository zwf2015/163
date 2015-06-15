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
    public class ConnectionsController : ApiController
    {
        private ChatServiceContext db = new ChatServiceContext();

        // GET: api/Connections
        public IQueryable<Connection> GetConnections()
        {
            return db.Connections;
        }

        // GET: api/Connections/5
        [ResponseType(typeof(Connection))]
        public async Task<IHttpActionResult> GetConnection(string id)
        {
            Connection connection = await db.Connections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }

            return Ok(connection);
        }

        // PUT: api/Connections/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutConnection(string id, Connection connection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != connection.ConnectionId)
            {
                return BadRequest();
            }

            db.Entry(connection).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConnectionExists(id))
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

        // POST: api/Connections
        [ResponseType(typeof(Connection))]
        public async Task<IHttpActionResult> PostConnection(Connection connection)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Connections.Add(connection);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ConnectionExists(connection.ConnectionId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = connection.ConnectionId }, connection);
        }

        // DELETE: api/Connections/5
        [ResponseType(typeof(Connection))]
        public async Task<IHttpActionResult> DeleteConnection(string id)
        {
            Connection connection = await db.Connections.FindAsync(id);
            if (connection == null)
            {
                return NotFound();
            }

            db.Connections.Remove(connection);
            await db.SaveChangesAsync();

            return Ok(connection);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConnectionExists(string id)
        {
            return db.Connections.Count(e => e.ConnectionId == id) > 0;
        }
    }
}