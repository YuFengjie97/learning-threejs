#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

vec3 colorA = vec3(0.149,0.141,0.912);
vec3 colorB = vec3(1.000,0.833,0.224);

void main() {
	float a = sin(u_time)
	
	vec3 color = mix(colorA, colorB, a)
	gl_FragColor = vec4(color, 1.0)
}
