using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Collections;

namespace ProductShop
{
    public class Buyer
    {
        private Shop _shop;
        private CustomLinkedList<Stand> _standToVisit;

        public int ProductsNumber { get; set; }

        public Buyer(Shop shop)
        {
            _shop = shop ?? throw new ArgumentNullException("Shop can't be null");

            _standToVisit = new CustomLinkedList<Stand>();
            //stands to visit
        }
    }
}
