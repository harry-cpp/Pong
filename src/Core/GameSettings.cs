using Microsoft.Xna.Framework.Graphics;

using FarseerPhysics.Dynamics;

namespace Pong
{
    public static class GameSettings
    {
        public static int Width;
        public static int Height = 600;

        static GameSettings()
        {
            var mode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            Width = (600 * mode.Width) / mode.Height;
        }
    }
}