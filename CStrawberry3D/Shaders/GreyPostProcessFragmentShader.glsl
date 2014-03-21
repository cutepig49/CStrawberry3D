#version 130

in vec2 vTextureCoord;

uniform sampler2D uSamplers[8];

void main()
{
	//greyscale
	vec4 color = texture2D(uSamplers[0], vTextureCoord);
	float value = (color.r*0.3 + color.g*0.59 + color.b*0.11) / 3;
	gl_FragColor = vec4(value, value, value, 1);
}