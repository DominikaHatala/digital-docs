using System;


namespace Xpdl
{
    /// <summary>
    ///     Klasa pozwalajaca na sprawdzenie dzialania klasy Reader.cs
    /// </summary>
    class Tester
    {
        static void Main(string[] args)
        {
            Reader reader = new Reader("klawisze.xpdl");

            while(true)
            {
                var input = Console.ReadLine();

                if(string.IsNullOrEmpty(input))
                {
                    break;
                }

                try
                {
                    var (transitionAction, nextActivities) = reader.GetActivityStatus(input);

                    var activities = nextActivities.Count > 0 ? string.Join(", ", nextActivities.ToArray()) : "Exit";

                    Console.WriteLine($"Activity ID: {input}. Action: {transitionAction.ToString()}. Next activities :  {activities}");
                }
                catch(ReaderException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
