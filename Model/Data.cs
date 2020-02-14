using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace DynamicMenu
{
    public class EditorDB
    {
        readonly SQLiteAsyncConnection _database;
        private int counter;
        public int Counter { get { return counter; } }
        public List<InstructionInstanceSerialized> lista;
        public EditorDB(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<InstructionInstanceSerialized>().Wait();
            counter = 0;
            lista = new List<InstructionInstanceSerialized>();
        }
        public Task<List<InstructionInstanceSerialized>> GetEditorAsync()
        {
            var task = _database.Table<InstructionInstanceSerialized>()
                .OrderBy(x => x.Id)
                .ToListAsync();
            /*
             * task.ContinueWith(x => {
                lista = x.Result;
                counter = x.Result.Count;
                });
            */
            return task;
        }
        public Task<int> SaveEditorAsync(IList<InstructionInstanceSerialized> lista)
        {
            // guardar todo lo acumulado a db y limpiar
            var task = _database.InsertAllAsync(lista);
            task.ContinueWith(code => lista.Clear());
            return task;
        }
        public Task<int> SaveEditorAsync()
        {
            return SaveEditorAsync(lista);
        }
        private void AddToSave(InstructionInstanceSerialized i)
        {
            lista.Add(i);
        }
        public void AddToSave(InstructionInstance i)
        {
            lista.Add(new InstructionInstanceSerialized(i.Instruction.Name + " " + i.ToString()));
        }

    }
}
