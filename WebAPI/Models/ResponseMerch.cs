using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public class ResponseMerch
    {
        public ResponseMerch (Entities.Merch merch)
        {
            ID = merch.ID;
            name = merch.name;
            cost = merch.cost;
            sale = merch.sale;
            picture = merch.picture;
        }
        public int ID { get; set; }
        public string name { get; set; }
        public int cost { get; set; }
        public int? sale { get; set; }
        public string picture { get; set; }
    }
}