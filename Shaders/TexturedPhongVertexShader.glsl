attribute vec3 aVertexPosition;
attribute vec3 aVertexNormal;
attribute vec2 aTextureCoord;

uniform mat4 uVMatrix;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uNMatrix;

uniform vec4 uAmbientLight;

uniform vec3 uDirections[8];
uniform vec4 uDirectionalLights[8];
uniform int uNumDirections;

varying vec2 vTextureCoord;
varying vec3 vLightWeighting;

void main(void) 
{
	gl_Position = uPMatrix * uVMatrix * uMVMatrix * vec4(aVertexPosition, 1);
	vTextureCoord = aTextureCoord;
	
	if (uNumDirections == 0)
	{
		vLightWeighting = vec3(1,1,1);
	}
	else
	{
		vec3 transformedNormal = (uNMatrix*vec4(aVertexNormal, 1)).xyz;
		float directionalLightWeighting = max(dot(transformedNormal, -uDirections[0]), 0.0);
		vLightWeighting = uAmbientLight.rgb + uDirectionalLights[0].rgb * directionalLightWeighting;
		
		for (int i = 1; i < uNumDirections; i++)
		{
			directionalLightWeighting = max(dot(transformedNormal, -uDirections[i]), 0.0);
			vLightWeighting = vLightWeighting + uAmbientLight.rgb + uDirectionalLights[i].rgb * directionalLightWeighting;
		}
	}
}