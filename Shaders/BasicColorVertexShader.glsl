attribute vec3 aVertexPosition;
attribute vec4 aVertexColor;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
varying vec4 vVertexColor;
void main(void) {
	gl_Position = uPMatrix*uVMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
	vVertexColor = aVertexColor;
}