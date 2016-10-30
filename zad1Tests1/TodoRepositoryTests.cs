using Microsoft.VisualStudio.TestTools.UnitTesting;
using zad1.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zad1.Interfaces.Repositories.Tests
{
    [TestClass()]
    public class TodoRepositoryTests
    {
  
        [TestMethod()]
        public void GetTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem = new TodoItem("Books");
            todoItem.Id = new Guid();
            rep.Add(todoItem);
            var item = rep.Get(todoItem.Id);

            
            Assert.AreEqual(item, todoItem);
        }
        [TestMethod()]
        public void GetNullTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem = new TodoItem("Books");
            
            var item = rep.Get(todoItem.Id);
            Assert.IsTrue(rep.Get(todoItem.Id) == null);

         //   Assert.AreEqual(item, todoItem);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingNullToDatabaseThrowsException()
        {
            ITodoRepository repository = new TodoRepository();
            repository.Add(null);
        }
        [TestMethod]
        public void AddingItemWillAddToDatabase()
        {
            ITodoRepository repository = new TodoRepository();
            var todoItem = new TodoItem("Groceries");
            repository.Add(todoItem);
            Assert.AreEqual(1, repository.GetAll().Count);
            Assert.IsTrue(repository.Get(todoItem.Id) != null);
        }
        [TestMethod]
        [ExpectedException(typeof(DuplicateTodoItemException))]
        public void AddingExistingItemWillThrowException()
        {
            ITodoRepository repository = new TodoRepository();
            var todoItem = new TodoItem("Groceries");
            repository.Add(todoItem);
            repository.Add(todoItem);
        }

     
        [TestMethod()]
        public void RemoveTrueTest()
        {
            ITodoRepository rep= new TodoRepository();
            var todoItem = new TodoItem("Groceries");
            rep.Add(todoItem);

            Assert.IsTrue(rep.Remove(todoItem.Id));
            Assert.AreEqual(0, rep.GetAll().Count);

        }

        [TestMethod()]
        public void RemoveFalseTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem = new TodoItem("Groceries");
            rep.Add(todoItem);
            rep.Remove(todoItem.Id);

            Assert.IsFalse(rep.Remove(todoItem.Id));
            

        }

        [TestMethod()]
        public void UpdateNewItemTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem = new TodoItem("Groceries");
            rep.Update(todoItem);

            Assert.AreEqual(1,rep.GetAll().Count);
        }

        [TestMethod()]
        public void UpdateItemAlreadyExistsTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem = new TodoItem("Groceries");
            rep.Add(todoItem);
            rep.Update(todoItem);

            Assert.AreEqual(1, rep.GetAll().Count);
        }

        [TestMethod()]
        public void MarkAsCompletedTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem1 = new TodoItem("Groceries");
            var todoItem2 = new TodoItem("Books");
            var todoItem3 = new TodoItem("House decorations");
            todoItem1.MarkAsCompleted();
            rep.Add(todoItem1);
            rep.Add(todoItem2);
            rep.Add(todoItem3);


            Assert.AreEqual(true, rep.MarkAsCompleted(todoItem1.Id));
           
    }

        [TestMethod()]
        public void GetAllTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem1 = new TodoItem("Groceries");
            var todoItem2 = new TodoItem("Books");
            var todoItem3 = new TodoItem("House decorations");
            todoItem3.DateCreated=DateTime.Parse("12/12/2016");
            rep.Add(todoItem1);
            rep.Add(todoItem2);
            rep.Add(todoItem3);
            rep.GetAll();

            Assert.AreEqual(todoItem3, rep.GetAll().First());
            Assert.AreEqual(3, rep.GetAll().Count);
        
        }

        [TestMethod()]
        public void GetActiveTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem1 = new TodoItem("Groceries");
            var todoItem2 = new TodoItem("Books");
            var todoItem3 = new TodoItem("House decorations");
            todoItem1.MarkAsCompleted();
            rep.Add(todoItem1);
            rep.Add(todoItem2);
            rep.Add(todoItem3);
         

            Assert.AreEqual(todoItem2, rep.GetActive().First());
            Assert.AreEqual(2, rep.GetActive().Count);
        }

        [TestMethod()]
        public void GetCompletedTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem1 = new TodoItem("Groceries");
            var todoItem2 = new TodoItem("Books");
            var todoItem3 = new TodoItem("House decorations");
            todoItem1.MarkAsCompleted();
            rep.Add(todoItem1);
            rep.Add(todoItem2);
            rep.Add(todoItem3);
            

            Assert.AreEqual(todoItem1, rep.GetCompleted().First());
            Assert.AreEqual(1, rep.GetCompleted().Count);
        }

        [TestMethod()]
        public void GetFilteredTest()
        {
            ITodoRepository rep = new TodoRepository();
            var todoItem1 = new TodoItem("Groceries");
            var todoItem2 = new TodoItem("Books");
            var todoItem3 = new TodoItem("House decorations");
            todoItem1.MarkAsCompleted();
            rep.Add(todoItem1);
            rep.Add(todoItem2);
            rep.Add(todoItem3);
            

       
            Assert.AreEqual(1, rep.GetFiltered(item => item.IsCompleted).Count);
        }
    }

    internal class DuplicateTodoItemException
    {
    }
}