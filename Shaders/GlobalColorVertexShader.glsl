uniform mat4 uPMatrix;
uniform mat4 uMVMatrix;

void main(void) {
	gl_Position = ftransform();
}