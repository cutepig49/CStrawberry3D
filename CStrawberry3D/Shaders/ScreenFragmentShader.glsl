#version 130

in vec2 vTextureCoord;

uniform sampler2D uSamplers[8];

void main()
{
	gl_FragColor = texture2D(uSamplers[0], vTextureCoord);
	//gl_FragColor = vec4(1,1,1,1);
}