uniform sampler2D uSamplers[8];
uniform int uNumSamplers;
varying vec2 vTexCoord;
void main(){
	vec4 color = vec4(0,0,0,0);
	for (int i=0; i<uNumSamplers; i++){
		color = color+texture2D(uSamplers[i], vTexCoord);
	}
	gl_FragColor = color;
}