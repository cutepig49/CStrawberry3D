#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;

uniform sampler2D uSamplers[MAX_NUM];
uniform float AveLum;
uniform vec2 uSamplerRects[MAX_NUM];

void main()
{
	float dx = 1.0/uSamplerRects[0].x;
	float dy = 1.0/uSamplerRects[0].y;
	//对uSamplers[0]进行采样
	vec4 color = texture2D(uSamplers[0],vTextureCoord);
	color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,0.0));

	color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy));
	color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,dy));

	color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*2.0));
	color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,dy*2.0));

	color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*3.0));
	color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,dy*3.0));
	color /= 8.0;
	//计算该像素在Tone Mapping之后的亮度值，如果依然很大，则该像素将产生光晕
	vec4 cout = vec4(0.0,0.0,0.0,0.0);
	float lum = color.x * 0.3 + color.y *0.59 + color.z * 0.11;
	//vec4 p = color*(lum/AveLum);
	vec4 p = color*(lum/0.8);
	p /= vec4(vec4(1.0,1.0,1.0,0.0)+p);
	float luml = (p.x+p.y+p.z)/3.0;
	if (luml > 0.8)
	{
		cout = p;
	}
	gl_FragData[0] = cout;	
}