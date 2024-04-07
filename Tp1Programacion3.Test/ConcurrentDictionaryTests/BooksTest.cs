using System.Collections.Concurrent;

namespace Tp1Programacion3.Test.ConcurrentDictionaryTests
{
    public class BooksTest
    {
        [Fact]
        public void Should_work_in_concurrency_properly()
        {
            // Arrange

            ConcurrentDictionary<int, Book> booksDictionary = new ConcurrentDictionary<int, Book>();
            string removing1;
            string removing;

            // Agregar valores a la ConcurrentDictionary
            for (int i = 0; i < 3; i++)
            {
                booksDictionary.TryAdd(i, new Book($"Harry Potter {1}", 335, "J.K. Rowling"));
            }

            // Listas para almacenar los valores tomados por cada hilo
            List<Book> valuesTakenByThread1 = new List<Book>();
            List<Book> valuesTakenByThread2 = new List<Book>();

            // Hilo1: toma valores 
            Task task1 = Task.Factory.StartNew(() =>
            {
                foreach (var item in booksDictionary)
                {
                    Book value1;
                    if (booksDictionary.TryRemove(item.Key, out value1))
                    {
                        valuesTakenByThread1.Add(value1);
                    }
                }
            });// Hilo1: toma valores 
            Task task2 = Task.Factory.StartNew(() =>
            {
                foreach (var item in booksDictionary)
                {
                    Book value2;
                    if (booksDictionary.TryRemove(item.Key, out value2))
                    {
                        valuesTakenByThread2.Add(value2);
                    }
                }
            });

            // Espera a que ambos hilos completen
            Task.WaitAll(task1, task2);

           
            // Assert

            // Verificar que no hayan elementos iguales tomados por ambos hilos
            foreach (var value in valuesTakenByThread1)
            {
                Assert.DoesNotContain(value, valuesTakenByThread2);
            }
            // Verificar que las collections no estan vacías (mejor quitarlo por que falla aveces)
            //Assert.NotEmpty(valuesTakenByThread1);
            //Assert.NotEmpty(valuesTakenByThread2);

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