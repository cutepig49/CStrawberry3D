uniform sampler2D uSampler0;

varying vec2 vTexCoord0;

void main(){
	gl_FragColor = texture2D(uSampler0, vTexCoord0);
}