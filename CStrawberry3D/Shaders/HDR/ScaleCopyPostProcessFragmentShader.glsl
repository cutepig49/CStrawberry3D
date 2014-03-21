#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;

uniform sampler2D uSamplers[MAX_NUM];
uniform vec2 uSamplerRects[MAX_NUM];

void main()
{
    float dx = 1.0/uSamplerRects[0].x;
    float dy = 1.0/uSamplerRects[0].y;
    vec4 color = texture2D(uSamplers[0],vTextureCoord);
	
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx,0.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,0.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,0.0));

    color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx,dy));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,dy));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,dy));

    color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*2.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx,dy*2.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,dy*2.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,dy*2.0));

    color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*3.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx,dy*3.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,dy*3.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*3.0,dy*3.0));

    color /= 16.0;
    gl_FragColor = color;
}