#version 130
#define MAX_NUM 8

in vec2 vClipPosition;

uniform vec4 uClearColor;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
uniform vec4 uAmbientLight;
uniform vec3 uDirections[MAX_NUM];
uniform vec4 uDirectionalLights[MAX_NUM];
uniform int uNumDirections;
uniform sampler2D uDeferredPosition;
uniform sampler2D uDeferredDiffuse;
uniform sampler2D uDeferredNormal;

vec3 calcNormal()
{
	vec3 normal = (texture2D(uDeferredNormal, vClipPosition).xyz - 0.5) * 2.0;
	return normal;
}
void main()
{
	vec4 diffuse = texture2D(uDeferredDiffuse, vClipPosition);
	vec3 normal = calcNormal();
	vec4 position = texture2D(uDeferredPosition, vClipPosition);

	int materialID = int((diffuse.w+0.01)*10);

	switch(materialID)
	{
	case 0:
		gl_FragColor = diffuse;
		break;
	case 1:
	case 2:
		vec4 totalAmbient = diffuse * uAmbientLight;
		vec4 totalDiffuse = vec4(0,0,0,1);
		for (int i = 0; i < uNumDirections; i++)
		{
			vec3 direction = -normalize(uDirections[i]);
			float lightWeight = max(dot(normal, direction), 0.0);
			totalDiffuse += uDirectionalLights[i] * lightWeight * diffuse;
		}
		gl_FragColor = totalAmbient + totalDiffuse;
		break;
	}
	//gl_FragColor=vec4(materialID, 0, 0, 1);
	//gl_FragColor = vec4(normal, 1);
	//gl_FragColor = vec4(position.z, 0, 0, 1);
	//gl_FragColor = position;
}