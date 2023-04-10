
uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

void main() {
	float a = gl_FragCoord.x - u_mouse.x;
	float b = gl_FragCoord.y - u_mouse.y;

	float distance = sqrt(pow(a, 2.0) + pow(b, 2.0));
	if(distance < 100.0) {
		gl_FragColor = vec4(1, 0, 0, 1);
	} else {
		gl_FragColor = vec4(1, 1, 0, 1);
	}
}
