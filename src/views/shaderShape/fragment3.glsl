#ifdef GL_ES
precision mediump float;
#endif

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

void main() {
	vec2 st = gl_FragCoord.xy / u_resolution.xy;

	vec3 colorBase = vec3(0.0f, 0.93f, 1.0f);

	vec2 o = vec2(0.5);

	float d = distance(o, st);

	vec3 color = mix(u_color, colorBase, vec3(d));
	// vec3 color = vec3(d);

	gl_FragColor = vec4(color, 1.0);
}
