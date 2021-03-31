using System;

namespace Console.Event.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            WaterHeater water = new WaterHeater();
            water.Boilwater();

            System.Console.ReadKey();
        }





    }
}
