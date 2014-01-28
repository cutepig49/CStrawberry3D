#version 130

in vec3 aVertexPosition;
in vec3 aVertexNormal;

uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
uniform mat4 uNMatrix;

out vec3 vWorldPosition;
out vec3 vNormal;

void main(void) {
    gl_Position = uPMatrix*uVMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
	vWorldPosition = gl_Position.xyz;
	vNormal = (uMVMatrix * vec4(aVertexNormal, 0)).xyz;
}