#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;

uniform sampler2D uSamplers[MAX_NUM];
uniform vec2 uSamplerRects[MAX_NUM];

void main()
{
    float dx = 1.0/float(uSamplerRects[0].x);
    float dy = 1.0/float(uSamplerRects[0].y);
    vec4 color = texture2D(uSamplers[0],vTextureCoord);
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,0.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*4.0,0.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*6.0,0.0));

    color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*2.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,dy*2.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*4.0,dy*2.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*6.0,dy*2.0));

    color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*4.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,dy*4.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*4.0,dy*4.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*6.0,dy*4.0));

    color += texture2D(uSamplers[0],vTextureCoord+vec2(0.0,dy*6.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*2.0,dy*6.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*4.0,dy*6.0));
    color += texture2D(uSamplers[0],vTextureCoord+vec2(dx*6.0,dy*6.0));

    color /= 16.0;
    gl_FragColor = color;
}