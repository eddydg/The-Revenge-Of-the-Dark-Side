using System;
 
namespace TRODS
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Game1 game = new Game1()) // Pour s'assurer que tout le contenu sera libéré à la fin de léxecution
            {
                game.Run();
            }
        }
    }
#endif
}

