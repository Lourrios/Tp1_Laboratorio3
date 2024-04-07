using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tp1Programacion3.Test.BlockingCollectionTests
{
    public class AdditionTest
    {
        [Fact]
        public void BlockingCollection_ProducerConsumerTest()
        {
            // Arrange
            var blockingCollection = new BlockingCollection<int>();
            int expectedSum = 0;
            int sumConsumer1 = 0;
            int sumConsumer2 = 0;

            // Act

            // Hilo Productor
            Task producerThread = Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= 10; i++) // Insertar números del 1 a N
                {
                    blockingCollection.Add(i);
                    expectedSum += i;
                    Thread.Sleep(100);
                }
                blockingCollection.CompleteAdding();
            });

            // Hilos Consumidores

            Task consumer1 = Task.Factory.StartNew(() =>
            {
                foreach (int number in blockingCollection.GetConsumingEnumerable())
                {
                    sumConsumer1 += number;
                }
            });

            Task consumer2 = Task.Factory.StartNew(() =>
            {
                foreach (int number in blockingCollection.GetConsumingEnumerable())
                {
                    sumConsumer2 += number;
                }
            });

            // Esperar a que todos los hilos terminen
            Task.WaitAll(producerThread, consumer1, consumer2);

            // Assert
            //Assert.Equal(expectedSum, sumConsumer1); // Verificar que la suma en el consumidor 1 es igual a la suma esperada (falla)
            //Assert.Equal(expectedSum, sumConsumer2); // Verificar que la suma en el consumidor 2 es igual a la suma esperada (falla)

            // verifica que la suma de ambos hilos es iguaal a la suma es del hilo productor
            Assert.Equal(expectedSum, sumConsumer1 + sumConsumer2);
        }
    }
}
