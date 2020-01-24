using static Retyped.dom;

namespace Pong.Web
{
    public class App
    {
        private static TheGame _game;
        
        public static void Main()
        {
            var canvas = new HTMLCanvasElement();
            canvas.style.position = "absolute";
            canvas.style.top = "0";
            canvas.style.bottom = "0";
            canvas.style.left = "0";
            canvas.style.right = "0";
            canvas.style.margin = "auto";
            canvas.width = (uint)GameSettings.Width;
            canvas.height = (uint)GameSettings.Height;
            canvas.id = "monogamecanvas";
            document.body.appendChild(canvas);

            _game = new TheGame();
            _game.Run();
        }
    }
}
