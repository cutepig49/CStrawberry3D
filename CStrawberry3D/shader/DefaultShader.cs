using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CStrawberry3D.shader
{
    public static class DefaultShader
    {
        private static string _BasicColorVertexShader;
        public static string BasicColorVertexShader
        {
            get
            {
                return _BasicColorVertexShader;
            }
        }
        private static string _BasicColorFragmentShader;
        public static string BasicColorFragmentShader
        {
            get
            {
                return _BasicColorFragmentShader;
            }
        }
        static DefaultShader()
        {
            StreamReader fileReader = new StreamReader("BasicColorVertexShader.glsl");
            _BasicColorVertexShader = fileReader.ReadToEnd();
            fileReader.Close();
            fileReader = new StreamReader("BasicColorFragmentShader.glsl");
            _BasicColorFragmentShader = fileReader.ReadToEnd();
            fileReader.Close();
        }
    }
}
