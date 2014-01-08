attribute vec3 aVertexPosition;
uniform mat4 uMVMatrix;
uniform mat4 uPMatrix;
uniform mat4 uVMatrix;
void main(void) {
    gl_Position = uPMatrix*uVMatrix*uMVMatrix*vec4(aVertexPosition, 1.0);
}