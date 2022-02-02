using Microsoft.Extensions.DependencyInjection;
using System;

namespace ApiIntegration.Runner {
    class Program {
        public static ServiceProvider ServiceProvider { get; private set; }

        public static void Setup(){
            var serviceCollection = new ServiceCollection();

            ServiceProvider = serviceCollection.BuildServiceProvider();

            //serviceCollection.AddSingleton<ITourRepository, TourRepository>();
        }
        static void Main(string[] args) {
            Setup();
            Console.WriteLine("Hello World!");
        }
    }
}
