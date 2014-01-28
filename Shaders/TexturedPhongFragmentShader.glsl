#version 130

in vec2 vTextureCoord;
in vec3 vLightWeighting;

uniform int uNumSamplers;
uniform sampler2D uSamplers[8];

void main(void) {
	vec4 textureColor;
	if (uNumSamplers == 0)
	{
		textureColor = vec4(1,1,1,1);
	}
	else
	{
		textureColor = texture2D(uSamplers[0], vTextureCoord);
	}
	for (int i = 1; i < uNumSamplers; i++)
	{
		textureColor = textureColor * texture2D(uSamplers[i], vTextureCoord);
	}
	gl_FragColor = vec4(textureColor.rgb * vLightWeighting, textureColor.a);
}