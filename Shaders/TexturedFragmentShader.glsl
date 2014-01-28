#version 130
#define MAX_NUM 8

in vec2 vTextureCoord;
in vec4 vWorldPosition;
in vec3 vNormal;

uniform sampler2D uSamplers[MAX_NUM];
uniform int uNumSamplers;
uniform int uPositionIndex;
uniform int uDiffuseIndex;
uniform int uNormalIndex;

vec3 calcNormal()
{
	vec3 normal = normalize(vNormal) * 0.5 + 0.5;
	return normal;
}
void main(){
	gl_FragData[uPositionIndex] = vWorldPosition;
	gl_FragData[uNormalIndex] = vec4(calcNormal(), 1);
	vec4 color = vec4(0,0,0,0);
	for (int i=0; i<uNumSamplers; i++){
		color = color+texture2D(uSamplers[i], vTextureCoord);
	}
	gl_FragData[uDiffuseIndex] = color;
}