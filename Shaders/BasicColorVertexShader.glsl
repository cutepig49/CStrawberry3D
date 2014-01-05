attribute vec3 aVertexPosition;
attribute vec4 aVertexColor;

varying vec4 vVertexColor;

void main(void) {
	gl_Position = ftransform();
	vVertexColor = aVertexColor;
	
}