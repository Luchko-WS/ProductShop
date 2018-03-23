using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Collections;
using System.Diagnostics;

namespace Utils.Test
{
    [TestClass]
    public class CustomLinkedListTest
    {
        [TestMethod]
        public void FindMinElementInCustomLinkedList()
        {
            CustomLinkedList<int> list = new CustomLinkedList<int>();
            list.Add(5);
            list.Add(7);
            list.Add(3);
            list.Add(1);
            list.Add(3);

            int expectedMin = 1;
            Debug.Assert(list != null && list.First != null, "First element is null");

            int actualMin = list.First.Value;
            var current = list.First;
            while (current.NextNode != null)
            {
                current = current.NextNode;
                if (actualMin > current.Value)
                {
                    actualMin = current.Value;
                }
            }

            Assert.AreEqual(expectedMin, actualMin);
        }

        class TestReferenceValue
        {
            public string Name { get; set; }
        }

        [TestMethod]
        public void RemoveElementInCustomLinkedList()
        {
            var item1 = new TestReferenceValue() { Name = "Test1" };
            var item2 = new TestReferenceValue() { Name = "Test2" };
            var item3 = new TestReferenceValue() { Name = "Test3" };
            CustomLinkedList<TestReferenceValue> list = new CustomLinkedList<TestReferenceValue>();
            list.Add(item1);
            list.Add(item2);
            list.Add(item3);
            list.Remove(item2);

            int expectedCount = 2;
            TestReferenceValue expectedItem1 = item1;
            TestReferenceValue expectedItem2 = item3;

            Assert.AreEqual(expectedCount, list.Count);
            Assert.AreEqual(expectedItem1, list.First.Value);
            Assert.AreEqual(expectedItem2, list.First.NextNode.Value);
        }

        [TestMethod]
        public void CopyOfCustomLinkedList()
        {
            var item1 = new TestReferenceValue() { Name = "Test1" };
            var item2 = new TestReferenceValue() { Name = "Test2" };

            CustomLinkedList<TestReferenceValue> list1 = new CustomLinkedList<TestReferenceValue>();
            list1.Add(item1);
            list1.Add(item2);

            CustomLinkedList<TestReferenceValue> list2 = new CustomLinkedList<TestReferenceValue>();
            foreach (var item in list1)
            {
                list2.Add(item);
            }

            list2.Remove(list2.First.Value);

            Assert.AreEqual(item1, list1.First.Value);

            foreach (var item in list1)
            {
                System.Console.WriteLine(item.Name);
            }
            foreach (var item in list2)
            {
                System.Console.WriteLine(item.Name);
            }
        }
    }
}
