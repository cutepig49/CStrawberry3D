using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CStrawberry3D.shader
{
    public static class DefaultShaders
    {
        public readonly static string BasicColorVertexShader;
        public readonly static string BasicColorFragmentShader;
        public readonly static string GlobalColorVertexShader;
        public readonly static string GlobalColorFragmentShader;
        public readonly static string TexturedVertexShader;
        public readonly static string TexturedFragmentShader;

        static DefaultShaders()
        {
            StreamReader fileReader = new StreamReader("BasicColorVertexShader.glsl");
            BasicColorVertexShader = fileReader.ReadToEnd();
            fileReader.Close();

            fileReader = new StreamReader("BasicColorFragmentShader.glsl");
            BasicColorFragmentShader = fileReader.ReadToEnd();
            fileReader.Close();

            fileReader = new StreamReader("GlobalColorVertexShader.glsl");
            GlobalColorVertexShader = fileReader.ReadToEnd();
            fileReader.Close();

            fileReader = new StreamReader("GlobalColorFragmentShader.glsl");
            GlobalColorFragmentShader = fileReader.ReadToEnd();
            fileReader.Close();

            fileReader = new StreamReader("TexturedVertexShader.glsl");
            TexturedVertexShader = fileReader.ReadToEnd();
            fileReader.Close();

            fileReader = new StreamReader("TexturedFragmentShader.glsl");
            TexturedFragmentShader = fileReader.ReadToEnd();
            fileReader.Close();
        }
    }
}
