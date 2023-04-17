// 动态渐变色，随时间改变

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

// vec3 colorA = vec3(0.912,0.894,0.000);
// vec3 colorB = vec3(0.025,0.006,1.000);

void main() {

	vec3 colorA = vec3(cos(u_time));
	vec3 colorB = vec3(sin(u_time));

	vec2 st = gl_FragCoord.xy / u_resolution.xy;
	vec3 pct = vec3(1.0);

	pct.r = smoothstep(0.0, 1.0, st.x);
	pct.g = sin(pct.x * PI);
	pct.b = pow(st.x,0.5);

	vec3 color = mix(colorA, colorB, pct);
	gl_FragColor = vec4(color, 1.0);
}
