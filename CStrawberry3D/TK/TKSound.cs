using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;
using System.Runtime.InteropServices;

namespace CStrawberry3D.TK
{
    public class TKSound
    {
        public static TKSound Create()
        {
            return new TKSound();
        }
        string _command;
        bool _isOpen;

        [DllImport("winmm.dll")]
        static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        TKSound()
        {
        }
        public void Close()
        {
            _command = "close MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
            _isOpen = false;
        }
        public void Open(string fileName)
        {
            _command = "open \"" + fileName + "\" type mpegvideo alias MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);
            _isOpen = true;
        }
        public void Play(bool loop)
        {
            if (_isOpen)
            {
                _command = "play MediaFile";
                if (loop)
                {
                    _command += " REPEAT";
                }
                mciSendString(_command, null, 0, IntPtr.Zero);
            }
        }
    }
}
