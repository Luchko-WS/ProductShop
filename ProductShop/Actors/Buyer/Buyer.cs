﻿using System;
using System.Threading;
using Utils;
using Utils.Collections;

namespace ProductShop.Actors
{
    public class Buyer
    {
        private Guid _guid;
        private Shop _shop;
        private Stand _currentStand;
        private CustomLinkedList<Stand> _standsToVisit;
        private int _productsCount;

        public int ProductsCount
        {
            get
            {
                return _productsCount;
            }
        }
        
        public Buyer(Shop shop)
        {
            _shop = shop ?? throw new ArgumentNullException("Shop can't be null");

            _guid = Guid.NewGuid();

            _standsToVisit = new CustomLinkedList<Stand>();
            foreach (var stand in shop.Stands)
            {
                _standsToVisit.Add(stand);
            }

            Thread buyerThread = new Thread(() =>
            {
                DoWork();
            });

            _shop.RegisterBuyer();
            buyerThread.Start();
        }

        public void DoWork()
        {
            LeaveCurrentStand();
            Stand stand = SelectStand();
            if (stand != null)
            {
                BuyProductsFromStand(stand);
            }
            else
            {
                _shop.UnregisterBuyer();
            }
        }

        private Stand SelectStand()
        {
            Stand selectedStand = null;
            if (_standsToVisit.Count != 0)
            {
                int minCount = _standsToVisit.First.Value.GetBuyersCountInQueue();
                selectedStand = _standsToVisit.First.Value;

                var currentNode = _standsToVisit.First;
                while (currentNode.NextNode != null)
                {
                    currentNode = currentNode.NextNode;
                    int buyersCountInQueue = currentNode.Value.GetBuyersCountInQueue();
                    if (minCount > buyersCountInQueue)
                    {
                        minCount = buyersCountInQueue;
                        selectedStand = currentNode.Value;
                    }
                }
                _standsToVisit.Remove(selectedStand);
            }
            return selectedStand;
        }

        private void BuyProductsFromStand(Stand stand)
        {
            Random rnd = new Random();
            _productsCount = rnd.Next(ProgramConfig.BuyerConfig.MinNumbersOfProducts, ProgramConfig.BuyerConfig.MaxNumbersOfProducts);
            SetCurrentStand(stand, _productsCount);
            stand.AddBuyerToQueue(this);
        }

        private void SetCurrentStand(Stand stand, int productCount)
        {
            _currentStand = stand;
            if (ProgramConfig.BuyerConfig.ShowBuyerLogInConsole)
            {
                ConsoleHelper.WriteInfo(String.Format("\nBuyer {0} say:\nI am coming to stand with {1}s.", _guid.ToString(), stand.Product.Name) +
                    String.Format(" I want to buy {0} {1}(s).", productCount, stand.Product.Name));
            }
        }

        private void LeaveCurrentStand()
        {
            if (_currentStand != null)
            {
                if (ProgramConfig.BuyerConfig.ShowBuyerLogInConsole)
                {
                    ConsoleHelper.WriteInfo(String.Format("\nBayer {0} say:\nI am leaving the stand with {1}s.", _guid.ToString(), _currentStand.Product.Name));
                }
                _currentStand = null;
            }
        }
    }
}
