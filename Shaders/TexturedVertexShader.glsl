varying vec2 vTexCoord0;

void main(){
	gl_Position = ftransform();
	vTexCoord0 = gl_MultiTexCoord0.xy;
}