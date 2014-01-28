#version 130

in vec3 aVertexPosition;
in vec2 aTextureCoord;
in vec3 aVertexNormal;

uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;

out vec4 vWorldPosition;
out vec2 vTextureCoord;
out vec3 vNormal;

void main()
{
	gl_Position = uPMatrix*uVMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
	vWorldPosition = uMVMatrix*vec4(aVertexPosition, 1.0);
	vTextureCoord = aTextureCoord;
	vNormal = (uMVMatrix * vec4(aVertexNormal, 0)).xyz;
}