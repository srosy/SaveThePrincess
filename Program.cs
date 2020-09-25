using System;

namespace SaveThePrincess
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SaveThePrincess())
                game.Run();
        }
    }
}
