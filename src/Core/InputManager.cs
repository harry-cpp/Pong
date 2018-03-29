using Microsoft.Xna.Framework.Input;

namespace Pong
{
    public static class InputManager
    {
        private static KeyboardState _prevkstate, _kstate;
        private static GamePadState _prevgstate1, _gstate1;
        private static GamePadState _prevgstate2, _gstate2;

        public static GamePadState[] GStates => new[] { _gstate1, _gstate2 }; 

        public static bool IsKeyDown(Keys key)
        {
            return _kstate.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _kstate.IsKeyDown(key) && !_prevkstate.IsKeyDown(key);
        }

        public static bool IsButtonPressed(int index, Buttons button)
        {
            if (index == 0)
                return _gstate1.IsButtonDown(button) && !_prevgstate1.IsButtonDown(button);
            else
                return _gstate2.IsButtonDown(button) && !_prevgstate2.IsButtonDown(button);
        }

        public static void Update()
        {
            _prevkstate = _kstate;
            _prevgstate1 = _gstate1;
            _prevgstate2 = _gstate2;

            _kstate = Keyboard.GetState();
            _gstate1 = GamePad.GetState(0);
            _gstate2 = GamePad.GetState(1);
        }
    }
}