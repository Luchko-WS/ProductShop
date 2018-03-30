namespace ProductShop
{
    public static class ProgramConfig
    {
        //When buyer enters the shop, he must visit all the stands and buy random (1-3) number of items at each of the stands. 
        public static class BuyerConfig
        {
            private const int _minNumbersOfProducts = 1;
            private const int _maxNumbersOfProducts = 3;

            public static int MinNumbersOfProducts
            {
                get { return _minNumbersOfProducts; }
            }
            public static int MaxNumbersOfProducts
            {
                get { return _maxNumbersOfProducts; }
            }
        }

        //Each seller can serve 1 buyer at one moment. Buyer's serving takes some time.
        public static class SellerConfig
        {
            //serving time in miliseconds
            private const int _minServingTime = 10;
            private const int _maxServingTime = 50;

            public static int MinServingTime
            {
                get { return _minServingTime; }
            }
            public static int MaxServingTime
            {
                get { return _maxServingTime; }
            }
        }

        //Each stand has a random number of sellers from 3 to 7.
        public static class StandConfig
        {
            private const int _minNumberOfSellers = 3;
            private const int _maxNumberOfSellers = 7;

            public static int MinNumberOfSellers
            {
                get { return _minNumberOfSellers; }
            }
            public static int MaxNumberOfSellers
            {
                get { return _maxNumberOfSellers; }
            }
        }
    }
}