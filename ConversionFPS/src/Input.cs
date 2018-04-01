using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ConversionFPS
{
    class Input
    {
        public static KeyboardState PreviousKeyboardState, CurrentKeyboardState;
        public static MouseState PreviousMouseState, CurrentMouseState;
        
        public static Rectangle MouseBox;

        public static void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            PreviousMouseState = CurrentMouseState;

            CurrentKeyboardState = Keyboard.GetState();
            CurrentMouseState = Mouse.GetState();

            MouseBox = new Rectangle(CurrentMouseState.X, CurrentMouseState.Y, 1, 1);
        }

        public static Keys[] KeysPressed()
        {
            return CurrentKeyboardState.GetPressedKeys();
        }

        public static bool KeyPressed(Keys k, bool firstPress)
        {
            return firstPress ? (PreviousKeyboardState[k] == KeyState.Up && CurrentKeyboardState[k] == KeyState.Down) : (CurrentKeyboardState.IsKeyDown(k));
        }

        public static bool Left(bool firstClick)
        {
            return firstClick ? (PreviousMouseState.LeftButton == ButtonState.Released && CurrentMouseState.LeftButton == ButtonState.Pressed) : (CurrentMouseState.LeftButton == ButtonState.Pressed);
        }

        public static bool Right(bool firstClick)
        {
            return firstClick ? (PreviousMouseState.RightButton == ButtonState.Released && CurrentMouseState.RightButton == ButtonState.Pressed) : (CurrentMouseState.RightButton == ButtonState.Pressed);
        }

        public static bool Middle(bool firstClick)
        {
            return firstClick ? (PreviousMouseState.MiddleButton == ButtonState.Released && CurrentMouseState.MiddleButton == ButtonState.Pressed) : (CurrentMouseState.MiddleButton == ButtonState.Pressed);
        }
    }
}
