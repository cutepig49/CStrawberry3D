using System.IO;

namespace CStrawberry3D.shader
{
	public static class DefaultShaders
	{
		public const string BasicColorVertexShader = @"attribute vec3 aVertexPosition;
attribute vec4 aVertexColor;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
varying vec4 vVertexColor;
void main(void) {
	gl_Position = uPMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
	vVertexColor = aVertexColor;
}";
		public const string BasicColorFragmentShader = @"varying vec4 vVertexColor;
void main(void) {
	gl_FragColor = vVertexColor;
}";
		public const string GlobalColorVertexShader = @"attribute vec3 aVertexPosition;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
void main(void) {
	//gl_Position = uPMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
    gl_Position = uPMatrix*uMVMatrix*vec4(aVertexPosition, 1.0) ;
        //gl_Position = gl_Vertex;
}";
		public const string GlobalColorFragmentShader = @"uniform vec4 uGlobalColor;
void main(void) {
	gl_FragColor = uGlobalColor;
}";
		public const string TexturedVertexShader = @"attribute vec3 aVertexPosition;
attribute vec2 aTexCoord;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
varying vec2 vTexCoord;
void main(){
	gl_Position = uPMatrix * uMVMatrix * vec4(aVertexPosition, 1.0);
	vTexCoord = aTexCoord;
}";
		public const string TexturedFragmentShader = @"uniform sampler2D uSamplers[8];
uniform int uNumSamplers;
varying vec2 vTexCoord;
void main(){
	vec4 color = vec4(0,0,0,0);
	for (int i=0; i<uNumSamplers; i++){
		color = color+texture2D(uSamplers[i], vTexCoord);
	}
	gl_FragColor = color;
}";
	}
}
