using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConversionFPS
{
    class Input
    {
        private static KeyboardState oldKeyboardState, currentKeyboardState;
        private static MouseState oldMouseState, currentMouseState;

        public static Rectangle MouseBox;
        public static Vector2 MousePos;

        public static void Update()
        {
            oldKeyboardState = currentKeyboardState;
            oldMouseState = currentMouseState;

            currentKeyboardState = Keyboard.GetState();
            currentMouseState = Mouse.GetState();

            MouseBox = new Rectangle(currentMouseState.X, currentMouseState.Y, 1, 1);
            MousePos = new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        public static Keys[] KeysPressed()
        {
            return currentKeyboardState.GetPressedKeys();
        }

        public static bool KeyPressed(Keys k, bool firstPress)
        {
            return firstPress ? (oldKeyboardState[k] == KeyState.Up && currentKeyboardState[k] == KeyState.Down) : (currentKeyboardState[k] == KeyState.Down);
        }

        public static bool Left(bool firstClick)
        {
            return firstClick ? (oldMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed) : (currentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool Right(bool firstClick)
        {
            return firstClick ? (oldMouseState.RightButton == ButtonState.Released && currentMouseState.RightButton == ButtonState.Pressed) : (currentMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool Middle(bool firstClick)
        {
            return firstClick ? (oldMouseState.MiddleButton == ButtonState.Released && currentMouseState.MiddleButton == ButtonState.Pressed) : (currentMouseState.MiddleButton == ButtonState.Pressed);
        }
    }
}
