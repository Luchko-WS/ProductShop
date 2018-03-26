using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Collections;

namespace Utils.Test
{
    [TestClass]
    public class CustomConcurrentQueueTest
    {
        [TestMethod]
        public void AddAndGetElementsInCustomConcurrentQueue()
        {
            CustomConcurrentQueue<int> queue = new CustomConcurrentQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(3);

            int exp1 = 5;
            int exp2 = 3;
            int expectedCpunt = 0;

            int actual;
            queue.Dequeue(out actual);
            Assert.AreEqual(exp1, actual);
            queue.Dequeue(out actual);
            Assert.AreEqual(exp2, actual);
            Assert.AreEqual(expectedCpunt, queue.Count);

            //queue is empty
            Assert.AreEqual(false, queue.Dequeue(out actual));
        }

        [TestMethod]
        public void PeekElementFromCustomConcurrentQueue()
        {
            CustomConcurrentQueue<int> queue = new CustomConcurrentQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(3);
            queue.Enqueue(7);
            queue.Enqueue(1);

            int expectedCount = 4;
            int expectedItem = 5;

            int actual;
            queue.Peek(out actual);
            Assert.AreEqual(expectedItem, actual);
            Assert.AreEqual(expectedCount, queue.Count);
        }
    }
}
