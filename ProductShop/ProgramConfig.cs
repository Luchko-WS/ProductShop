namespace ProductShop
{
    public static class ProgramConfig
    { 
        public static class BuyerConfig
        {
            //When buyer enters the shop, he must visit all the stands and buy random (1-3) number of items at each of the stands. 
            private const int _minNumbersOfProducts = 1;
            private const int _maxNumbersOfProducts = 3;
            //Show information about vsiting/leaving stand
            private const bool _showBuyerLogInConsole = false;

            public static int MinNumbersOfProducts
            {
                get { return _minNumbersOfProducts; }
            }
            public static int MaxNumbersOfProducts
            {
                get { return _maxNumbersOfProducts; }
            }
            public static bool ShowBuyerLogInConsole
            {
                get { return _showBuyerLogInConsole; }
            }
        }

        public static class SellerConfig
        {
            //Each seller can serve 1 buyer at one moment. Buyer's serving takes some time.
            //(serving time in miliseconds)
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
 
        public static class StandConfig
        {
            //Each stand has a random number of sellers from 3 to 7.
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