#ifdef GL_ES
precision mediump float;
#endif

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

float circle(vec2 st, vec2 o, float r) {
	float l = length(st - o);
	return smoothstep(r - 0.003, r, l);
}

void main() {
	vec2 st = gl_FragCoord.xy / u_resolution.xy;

	vec2 o = vec2(0.5, 0.5);

	float r = 0.5;

	float pct = circle(st, o, r);

	vec3 color = vec3(pct);

	gl_FragColor = vec4(color, 1.0);
}
