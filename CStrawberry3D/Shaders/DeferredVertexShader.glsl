#version 130
#define MAX_NUM 8

in vec3 aVertexPosition;

uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
uniform mat4 uNMatrix;

out vec2 vClipPosition;

void main()
{
	vec4 position = vec4(aVertexPosition, 1.0);
	gl_Position = position;
	position.xyz /= position.w;
	vClipPosition.x = position.x*0.5+0.5;
	vClipPosition.y = position.y*0.5+0.5;
}