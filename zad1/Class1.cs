using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace zad1
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DateCompleted { get; set; }
        public DateTime DateCreated { get; set; }

        public TodoItem(string text)
        {
            Id = Guid.NewGuid();
            Text = text;
            IsCompleted = false;
            DateCreated = DateTime.Now;
        }
        public void MarkAsCompleted()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                DateCompleted = DateTime.Now;
            }
        }
    }

    namespace Interfaces
    {
        public interface ITodoRepository
        {
            /// <summary >
            /// Gets  TodoItem  for a given id
            /// </summary >
            /// <returns >TodoItem  if found , null  otherwise </returns >
            TodoItem Get(Guid todoId);
            /// <summary >
            /// Adds  new  TodoItem  object  in  database.
            /// If  object  with  the  same id  already  exists ,
            /// method  should  throw  DuplicateTodoItemException  with  the  message"duplicate  id: {id}".
            /// </summary >
            void Add(TodoItem todoItem);
            /// <summary >
            /// Tries to  remove a TodoItem  with  given id from  the  database.
            /// </summary >
            /// <returns >True if success , false  otherwise </returns >
            bool Remove(Guid todoId);
            /// <summary >
            /// Updates  given  TodoItem  in  database.
            /// If  TodoItem  does  not exist , method  will  add  one.
            /// </summary >
            void Update(TodoItem todoItem);
            /// <summary >
            /// Tries to mark a TodoItem  as  completed  in  database.
            /// </summary >
            /// <returns >True if success , false  otherwise </returns >
            bool MarkAsCompleted(Guid todoId);
            /// <summary >
            /// Gets  all  TodoItem  objects  in database , sorted  by date  created (descending)
            /// </summary >
            List<TodoItem> GetAll();
            /// <summary >
            /// Gets  all  incomplete  TodoItem  objects  in  database
            /// </summary >
            List<TodoItem> GetActive();
            /// <summary >
            /// Gets  all  completed  TodoItem  objects  in  database
            /// </summary >
            List<TodoItem> GetCompleted();
            /// <summary >
            /// Gets  all  TodoItem  objects  in  database  that  apply to the  filter
            /// </summary >
            List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction);
        }

        namespace Repositories
        {
            /// <summary >
            /// Class  that  encapsulates  all  the  logic  for  accessing  TodoTtems.
            ///  </summary >

            public class TodoRepository : ITodoRepository
            {
                /// <summary >
                /// Repository  does  not  fetch  todoItems  from  the  actual  database ,
                /// it uses in  memory  storage  for  this  excersise.
                /// </summary >
                private readonly List<TodoItem> _inMemoryTodoDatabase;
                public TodoRepository(List<TodoItem> initialDbState = null)
                {
                    if (initialDbState != null)
                    {
                        _inMemoryTodoDatabase = initialDbState;
                    }
                    else
                    {
                        _inMemoryTodoDatabase = new List<TodoItem>();
                    }
                }
                //  Shorter way to write  this in C# using ??  operator:
                //  _inMemoryTodoDatabase = initialDbState  ?? new List <TodoItem >();
                // x ?? y -> if x is not null , expression  returns x. Else y.


                public TodoItem Get(Guid todoId)
                {

                    return _inMemoryTodoDatabase.Where(item => item.Id == todoId).FirstOrDefault();
                }
                public void Add(TodoItem todoItem)
                {
                    if (todoItem == null)
                        throw new ArgumentNullException();
                    else
                    {
                        TodoItem item = _inMemoryTodoDatabase.Where(s => s.Id == todoItem.Id).FirstOrDefault();
                        if (item != null)
                        {
                            throw new DuplicateTodoItemException("duplicate id: {id}", todoItem.Id);
                        }

                        else
                            _inMemoryTodoDatabase.Add(todoItem);
                    }
                }
                public bool Remove(Guid todoId)
                {
                    TodoItem item = _inMemoryTodoDatabase.Where(s => s.Id == todoId).FirstOrDefault();
                    if (item != null)
                        return _inMemoryTodoDatabase.Remove(item);
                    return false;
                }
                public void Update(TodoItem todoItem)
                {
                    TodoItem item = _inMemoryTodoDatabase.Where(s => s.Id == todoItem.Id).FirstOrDefault();
                    if (item == null)
                        _inMemoryTodoDatabase.Add(todoItem);
                    else
                    {
                        _inMemoryTodoDatabase.Remove(item);
                        _inMemoryTodoDatabase.Add(todoItem);
                    }
                        
                }
                public bool MarkAsCompleted(Guid todoId)
                {
                    TodoItem item = _inMemoryTodoDatabase.Where(s => s.Id == todoId).FirstOrDefault();
                    if (item != null)
                    {
                        if (item.IsCompleted)
                            return true;
                        else
                        {
                            item.MarkAsCompleted();
                            _inMemoryTodoDatabase.Where(s => s.Id == todoId).ToList().ForEach(s => s.IsCompleted = true);
                            return true;
                        }
                    }
                    return false;
                }
                public List<TodoItem> GetAll()
                {
                    List<TodoItem> list;
                    return list = _inMemoryTodoDatabase.OrderByDescending(s => s.DateCreated).ToList();
                }
                public List<TodoItem> GetActive()
                {
                    List<TodoItem> list;
                    return list = _inMemoryTodoDatabase.Where(s => s.IsCompleted == false).ToList();
                }
                public List<TodoItem> GetCompleted()
                {
                    List<TodoItem> list;
                    return list = _inMemoryTodoDatabase.Where(s => s.IsCompleted == true).ToList();
                }
                public List<TodoItem> GetFiltered(Func<TodoItem, bool> filterFunction)
                {
                    List<TodoItem> list;
                    return list = _inMemoryTodoDatabase.Where(filterFunction).ToList();
                }

                [Serializable]
                public class DuplicateTodoItemException : Exception
                {
                    private Guid id;
                    private string text;

                    public DuplicateTodoItemException()
                    {
                    }

                    public DuplicateTodoItemException(string message) : base(message)
                    {

                    }

                    public DuplicateTodoItemException(string text, Guid id) 
                    {
                        this.id = id;
                        this.text = text;
                    }

                    public DuplicateTodoItemException(string message, Exception innerException) : base(message, innerException)
                    {
                    }

                    protected DuplicateTodoItemException(SerializationInfo info, StreamingContext context) : base(info, context)
                    {
                    }
                }
            }
        }
    }


    public interface IGenericList<X> : IEnumerable<X>
    {

        void Add(X item);
        bool Remove(X item);
        bool RemoveAt(int index);
        X GetElement(int index);
        int IndexOf(X item);
        int Count { get; }
        void Clear();
        bool Contains(X item);

    }
    public class GenericList<X> : IGenericList<X>
    {
        private X[] _internalStorage;
        private int len = 0;

        public GenericList()
        {

            X[] _internalStorage = new X[4];
        }

        public GenericList(int initialSize)
        {
            if (initialSize > 0)
            {
                Array.Resize(ref _internalStorage, initialSize);

            }
            else
                throw new ArgumentException("Size must be greater than zero");
        }

        public IEnumerator<X> GetEnumerator()
        {
            return new GenericListEnumerator<X>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public class GenericListEnumerator<X> : IEnumerator<X>
        {
            private X[] _inStorage;
            private int _current = -1;

            public GenericListEnumerator(GenericList<X> genlist)
            {
                _inStorage = genlist._internalStorage;
            }
            public X Current
            {
                get
                {
                    try
                    {
                        return _inStorage[_current];
                    }
                    catch (IndexOutOfRangeException)
                    { 
                    throw new InvalidOperationException();
                    }
                }
            }

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            public bool MoveNext()
            {
                _current++;
                return (_current < _inStorage.Length);

            }
            public void Reset()
            {
                _current = -1;
            }

            void IDisposable.Dispose() { }
          /*  private object Current1
            {
                get { return this.Current; }
            }
            object IEnumerator.Current
            {
                get { return Current1; }
            } */


        }

        public void Add(X item)
        {
            if (item == null)
                throw new ArgumentNullException();

            if (len >= _internalStorage.Length)
            {
                Array.Resize(ref _internalStorage, _internalStorage.Length * 2);
            }


            _internalStorage[len] = item;
            len++;

        }
        public int Count
        {
            get
            {
                return len;
            }
        }
        public bool RemoveAt(int index)
        {
            int i;
            if (index > len)
                return false;
            else
            {

                for (i = index; i < len - 1; i++)
                    _internalStorage[i] = _internalStorage[i + 1];
                //_internalStorage[_internalStorage.Count() - 1] = 0;
                len--;
                if (len == _internalStorage.Length)
                    Array.Resize(ref _internalStorage, len);
                return true;
            }

        }
        public bool Remove(X item)
        {
            int poz;
            poz = IndexOf(item);
            if (poz == -1)
                return false;
            else return RemoveAt(poz);
        }
        public X GetElement(int index)
        {
            if (index < 0)
                throw new IndexOutOfRangeException("Index cannot have negative value");
            else
                return _internalStorage[index];
        }
        public int IndexOf(X item)
        {
            int i;
            for (i = 0; i < _internalStorage.Length; i++)
                if (_internalStorage[i].Equals(item))
                {

                    return i;

                }

            return -1;


        }
        public void Clear()
        {
            _internalStorage = null;
        }
        public bool Contains(X item)
        {
            int i;
            for (i = 0; i < _internalStorage.Length; i++)
                if (_internalStorage[i].Equals(item))
                {

                    return true;
                }

            return false;
        }
    }
}


