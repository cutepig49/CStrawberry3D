#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;

uniform sampler2D uSamplers[MAX_NUM];
uniform vec2 uSamplerRects[MAX_NUM];

void main()
{
	vec4 color = texture2D(uSamplers[0],vTextureCoord);
	gl_FragColor = color;
}