using System.Collections.Concurrent;

namespace Tp1Laboratorio3.Test.ConcurrentDictionaryTests
{
    public class BooksTest
    {
        [Fact]
        public void Verificar_seguridad_con_hilos()
        {
            // Arrange

            ConcurrentDictionary<int, Book> booksDictionary = new ConcurrentDictionary<int, Book>();
            string removing1;
            string removing;

            // Agregar valores a la ConcurrentDictionary
            for (int i = 0; i < 3; i++)
            {
                booksDictionary.TryAdd(i, new Book($"Harry Potter {i}", 335, "J.K. Rowling"));
            }

            // Listas para almacenar los valores tomados por cada hilo
            List<Book> valuesTakenByThread1 = new List<Book>();
            List<Book> valuesTakenByThread2 = new List<Book>();

            // Hilo 1: toma valores 
            Task task1 = Task.Run(() =>
            {
                foreach (var item in booksDictionary)
                {
                    Book value1;
                    if (booksDictionary.TryRemove(item.Key, out value1))
                    {
                        valuesTakenByThread1.Add(value1);
                    }
                }
            });
            // Hilo 2: toma valores 
            Task task2 = Task.Run(() =>
            {
                foreach (var item in booksDictionary)
                {
                    Book value2;
                    if (booksDictionary.TryRemove(item.Key, out value2))
                    {
                        valuesTakenByThread2.Add(value2);
                    }
                    Thread.Sleep(1000); // simula prcesamiento
                }
            });

            // Espera a que ambos hilos terinen de ejecutarse
            Task.WaitAll(task1, task2);

           
            // Assert

            // Verificar que no hayan elementos iguales tomados por ambos hilos
            foreach (var value in valuesTakenByThread1)
            {
                Assert.DoesNotContain(value, valuesTakenByThread2);
            }

            // Une ambas listas en una nueva 
            List<Book> listConcat = valuesTakenByThread1.Concat(valuesTakenByThread2).ToList(); ;

            // verificar que la lista nueva sea igual a la lista original
            foreach (var bk in booksDictionary.Values)
            {
                Assert.Contains(bk, listConcat);
            }
        }
    }
}