using System;

namespace Pong.Linux
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new TheGame())
                game.Run();
        }
    }
}
