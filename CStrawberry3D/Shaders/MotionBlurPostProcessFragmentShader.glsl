#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;

uniform sampler2D uSamplers[MAX_NUM];

void main()
{
	gl_FragColor = texture2D(uSamplers[0], vTextureCoord)*0.5+ texture2D(uSamplers[1], vTextureCoord)*0.5;
}