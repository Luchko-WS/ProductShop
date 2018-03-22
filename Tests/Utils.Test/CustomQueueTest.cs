using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Collections;

namespace Utils.Test
{
    [TestClass]
    public class CustomQueueTest
    {
        [TestMethod]
        public void AddAndGetElementsInCustomQueue()
        {
            CustomQueue<int> queue = new CustomQueue<int>();
            queue.Push(5);
            queue.Push(3);

            int exp1 = 5;
            int exp2 = 3;
            int expectedCpunt = 0;

            Assert.AreEqual(exp1, queue.Pop());
            Assert.AreEqual(exp2, queue.Pop());
            Assert.AreEqual(expectedCpunt, queue.Count);
        }

        [TestMethod]
        public void PeekElementFromCustomQueue()
        {
            CustomQueue<int> queue = new CustomQueue<int>();
            queue.Push(5);
            queue.Push(3);
            queue.Push(7);
            queue.Push(1);

            int expectedCount = 4;
            int expectedItem = 5;

            Assert.AreEqual(expectedItem, queue.Peek());
            Assert.AreEqual(expectedCount, queue.Count);
        }

        [TestMethod]
        public void ClearCustomQueue()
        {
            CustomQueue<int> queue = new CustomQueue<int>();
            queue.Push(5);
            queue.Push(3);
            queue.Clear();
            queue.Push(10);

            int expected = 10;

            Assert.AreEqual(expected, queue.Pop());
        }
    }
}
