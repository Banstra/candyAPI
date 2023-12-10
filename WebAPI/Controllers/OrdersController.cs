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
    public class OrdersController : ApiController
    {
        private SoldatovaCRUDEntities db = new SoldatovaCRUDEntities();

        // GET: api/Orders
        public IQueryable<Order> GetOrders()
        {
            return db.Orders;
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult GetOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }
        [Route("api/getorders")]
        public IHttpActionResult GetOrders (int MerchID)
        {
            var orders = db.Orders.ToList().Where(p => p.MerchID == MerchID);
            return Ok(orders);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrder(int id, Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.ID)
            {
                return BadRequest();
            }

            db.Entry(order).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public IHttpActionResult PostOrder(Order order)
        {
          
            if (!(db.Workers.ToList().FirstOrDefault(p => p.ID == order.UserID) is Worker))
            {
                ModelState.AddModelError("User", "An anauthorized user");
            }
           if(!(db.Merches.ToList().FirstOrDefault(p => p.ID == order.MerchID) is Merch))
            {
                ModelState.AddModelError("Merch", "No such merch in the shop");
            }
           if(!(db.Places.ToList().FirstOrDefault(p => p.ID == order.Place) is Place))
            {
                ModelState.AddModelError("Place", "Choose a place from 1 to 4");
            }
          if(order.Amount == 0)
            {
                ModelState.AddModelError("Amount", "Choose the amount of product");
            }

            var merch = db.Merches.ToList().FirstOrDefault(p => p.ID == order.MerchID);
            Random rnd = new Random();
            int num = rnd.Next(1000, 9999);

            order.DateOrder = DateTime.Now;

            if (order.Place == 1)
            {
                order.DateArrive = order.DateOrder.AddDays(6);

                order.Amount = 1;


            }
            if (order.Place == 2)
            {
                order.DateArrive = order.DateOrder.AddDays(3);
            }
            if (order.Place == 3)
            {
                order.DateArrive = order.DateOrder.AddDays(7);
            }
            if (order.Place == 4)
            {
                order.DateArrive = order.DateOrder.AddDays(1);
            }
            else
            {
                order.DateArrive = order.DateOrder.AddDays(6);
            }
            if (DateTime.Compare(DateTime.Now, order.DateArrive) > 0)
            {
                order.Arrived = false;
            }
            else
            {
                order.Arrived = true;
            }


            order.Cost = (merch.cost - merch.cost * merch.sale / 100) * order.Amount;
            order.Sale = merch.sale;
            order.Code = num;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Orders.Add(order);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = order.ID }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public IHttpActionResult DeleteOrder(int id)
        {
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            db.SaveChanges();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.ID == id) > 0;
        }
    }
}