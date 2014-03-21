#version 150

in vec3 aVertexPosition;
in vec2 aTextureCoord;

out vec2 vTextureCoord;
void main()
{
	gl_Position = vec4(aVertexPosition,1);
	vTextureCoord = vec2(aTextureCoord.x, -aTextureCoord.y);
}