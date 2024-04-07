using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1Programacion3.Test.ConcurrentDictionaryTests
{
    public class NoReferenceTest
    {
        [Fact]
        public void Threads_should_not_contain_same_elements()
        {
            // Arrange
            ConcurrentDictionary<int, string> books = new ConcurrentDictionary<int, string>();
            string removing1;
            string removing;

            // Agregar valores a la ConcurrentDictionary
            for (int i = 0; i < 10; i++)
            {
                books.TryAdd(i, $"Harry potter {i}");
            }

            // Listas para almacenar los valores tomados por cada hilo
            List<string> valuesTakenByThread1 = new List<string>();
            List<string> valuesTakenByThread2 = new List<string>();

            // Hilo 1: Toma valores
            Task task1 = Task.Factory.StartNew(() =>
            {
                foreach (var item in books)
                {
                    string value;
                    if (books.TryRemove(item.Key, out value))
                    {
                        valuesTakenByThread1.Add(value);
                    }
                }
            });

            // Hilo 2: Toma valores
            Task task2 = Task.Factory.StartNew(() =>
            {
                foreach (var item in books)
                {
                    string value;
                    if (books.TryRemove(item.Key, out value))
                    {
                        valuesTakenByThread2.Add(value);
                    }
                }
            });
            // concatena ambas listas en una nueva
            List<string> listConcat = valuesTakenByThread1.Concat(valuesTakenByThread2).ToList(); ;

            // Espera a que ambos hilos completen
            Task.WaitAll(task1, task2);

            // Assert
            // Verificar que no hayan elementos iguales tomados por ambos hilos
            foreach (var value in valuesTakenByThread1)
            {
                Assert.DoesNotContain(value, valuesTakenByThread2);
            }
            // Verificar que las collections no estan vacías
            Assert.NotEmpty(valuesTakenByThread1);
            Assert.NotEmpty(valuesTakenByThread2);

            // verificar que la lista nueva sea igual a la lista original
            foreach (var bk in books.Values)
            {
                Assert.Contains(bk, listConcat);
            }

        }
    }
}
