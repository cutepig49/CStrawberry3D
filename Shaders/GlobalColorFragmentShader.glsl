#version 130

in vec3 vWorldPosition;
in vec3 vNormal;

uniform vec4 uGlobalColor;
uniform int uPositionIndex;
uniform int uDiffuseIndex;
uniform int uNormalIndex;
uniform vec3 uDirections[8];

vec4 calcNormal()
{
	vec4 normal = vec4(normalize(vNormal) * 0.5 + 0.5, 1);
	return normal;
}

void main(void) {
	gl_FragData[uPositionIndex] = vec4(vWorldPosition, 1.0);
	gl_FragData[uDiffuseIndex] = uGlobalColor;
	gl_FragData[uNormalIndex] = calcNormal();
}