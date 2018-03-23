using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Collections;

namespace Utils.Test
{
    [TestClass]
    public class CustomQueueTest
    {
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AddAndGetElementsInCustomQueue()
        {
            CustomConcurrentQueue<int> queue = new CustomConcurrentQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(3);

            int exp1 = 5;
            int exp2 = 3;
            int expectedCpunt = 0;

            Assert.AreEqual(exp1, queue.Dequeue());
            Assert.AreEqual(exp2, queue.Dequeue());
            Assert.AreEqual(expectedCpunt, queue.Count);

            //exception
            queue.Dequeue();
        }

        [TestMethod]
        public void PeekElementFromCustomQueue()
        {
            CustomConcurrentQueue<int> queue = new CustomConcurrentQueue<int>();
            queue.Enqueue(5);
            queue.Enqueue(3);
            queue.Enqueue(7);
            queue.Enqueue(1);

            int expectedCount = 4;
            int expectedItem = 5;

            Assert.AreEqual(expectedItem, queue.Peek());
            Assert.AreEqual(expectedCount, queue.Count);
        }
    }
}
