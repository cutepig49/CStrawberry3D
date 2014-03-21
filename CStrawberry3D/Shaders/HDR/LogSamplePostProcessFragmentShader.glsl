#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;

uniform sampler2D uSamplers[MAX_NUM];

void main()
{
    vec4 lumfact = vec4(0.27,0.67,0.06,0.0); 
    vec4 color = texture2D(uSamplers[0],vTextureCoord);
    float lum = log(dot(color , lumfact) + 0.0001);
    gl_FragColor= vec4(lum,lum,lum,1.0);
}