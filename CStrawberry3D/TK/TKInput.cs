using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Input;

namespace CStrawberry3D.TK
{
    public class TKInput
    {
        public bool MouseLB { get; private set; }
        public bool MouseRB { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int DeltaX { get; private set; }
        public int DeltaY { get; private set; }
        public TKInput()
        {
            MouseLB = false;
            MouseRB = false;
            X = Mouse.GetState().X;
            Y = Mouse.GetState().Y;
            DeltaX = 0;
            DeltaY = 0;
        }
        public void Update()
        {
            var mouseState = Mouse.GetState();
            var x = mouseState.X;
            var y = mouseState.Y;
            DeltaX = x - X;
            DeltaY = y - Y;
            X = x;
            Y = y;
            MouseLB = mouseState[MouseButton.Left];
            MouseRB = mouseState[MouseButton.Right];
        }
        public bool KeyDown(Interface.Key key)
        {
            var keyState = Keyboard.GetState();
            switch(key)
            {
                case Interface.Key.Down:
                    return keyState[Key.Down];
                case Interface.Key.Up:
                    return keyState[Key.Up];
                case Interface.Key.Left:
                    return keyState[Key.Left];
                case Interface.Key.Right:
                    return keyState[Key.Right];
                case Interface.Key.PageDown:
                    return keyState[Key.PageDown];
                case Interface.Key.PageUp:
                    return keyState[Key.PageUp];
                case Interface.Key.Space:
                    return keyState[Key.Space];
                default:
                    return false;
            }
        }
    }
}
