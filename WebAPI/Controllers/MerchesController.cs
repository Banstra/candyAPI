using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebAPI.Entities;

namespace WebAPI.Controllers
{
    public class MerchesController : ApiController
    {
        private SoldatovaCRUDEntities db = new SoldatovaCRUDEntities();

        // GET: api/Merches
        [ResponseType(typeof(List<Models.ResponseMerch>))]
        public IHttpActionResult GetMerches()
        {
            return Ok(db.Merches.ToList().ConvertAll(p => new Models.ResponseMerch(p)));
        }

        // GET: api/Merches/5
        [ResponseType(typeof(Merch))]
        public IHttpActionResult GetMerch(int id)
        {
            Merch merch = db.Merches.Find(id);
            if (merch == null)
            {
                return NotFound();
            }

            return Ok(merch);
        }

        // PUT: api/Merches/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMerch(int id, Merch merch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != merch.ID)
            {
                return BadRequest();
            }

            db.Entry(merch).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MerchExists(id))
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

        // POST: api/Merches
        [ResponseType(typeof(Merch))]
        public IHttpActionResult PostMerch(Merch merch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Merches.Add(merch);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = merch.ID }, merch);
        }

        // DELETE: api/Merches/5
        [ResponseType(typeof(Merch))]
        public IHttpActionResult DeleteMerch(int id)
        {
            Merch merch = db.Merches.Find(id);
            if (merch == null)
            {
                return NotFound();
            }

            db.Merches.Remove(merch);
            db.SaveChanges();

            return Ok(merch);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MerchExists(int id)
        {
            return db.Merches.Count(e => e.ID == id) > 0;
        }
    }
}