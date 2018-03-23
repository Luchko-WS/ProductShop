namespace ProductShop
{
    public class Product
    {
        private string _name;
        private double _price;

        public double Price
        {
            get
            {
                return _price;
            }
        }


        public string Name
        {
            get
            {
                return _name;
            }

        }


        public Product(string name, double price)
        {
            _name = name;
            _price = price;
        }
    }
}
