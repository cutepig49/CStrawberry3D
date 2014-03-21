#version 130
#define MAX_NUM 8

in vec3 vTextureCoord;

uniform samplerCube uCubeSamplers[MAX_NUM];
uniform int uPositionIndex;
uniform int uDiffuseIndex;
uniform int uNormalIndex;
uniform int uMaterialID;

void main(){
	//gl_FragData[uPositionIndex] = vWorldPosition;
	//gl_FragData[uNormalIndex] = vec4(calcNormal(), 1);
	float materialID = float(uMaterialID) * 0.1;
	gl_FragData[uDiffuseIndex] = vec4(textureCube(uCubeSamplers[0], vTextureCoord).xyz, materialID);
	//gl_FragData[uDiffuseIndex] = vec4(materialID, 0, 0, 1);
}