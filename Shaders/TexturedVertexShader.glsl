attribute vec3 aVertexPosition;
attribute vec2 aTextureCoord;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
varying vec2 vTextureCoord;
void main()
{
	gl_Position = uPMatrix*uVMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
	vTextureCoord = aTextureCoord;
}