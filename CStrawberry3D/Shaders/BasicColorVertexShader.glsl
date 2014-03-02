#version 130

in vec3 aVertexPosition;
in vec4 aVertexColor;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
out vec4 vVertexColor;
void main(void) {
	gl_Position = uPMatrix*uVMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
	vVertexColor = aVertexColor;
}