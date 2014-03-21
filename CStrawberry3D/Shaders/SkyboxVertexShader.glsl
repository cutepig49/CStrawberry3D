#version 150

in vec3 aVertexPosition;

uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;

out vec3 vTextureCoord;

void main()
{
	gl_Position = vec4(aVertexPosition.xy, -1,1);
	
	mat4 r = uVMatrix*uMVMatrix;
	r[3][0] = 0.0;
	r[3][1] = 0.0;
	r[3][2] = 0.0;
	
	vec4 v = inverse(r) * inverse(uPMatrix)*vec4(aVertexPosition.xy, -1,1);
	
	vTextureCoord = -v.xyz;
}