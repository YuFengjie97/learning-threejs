// 彩虹

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

vec3 color1 = vec3(0.912, 0.017, 0.052);
vec3 color2 = vec3(0.05, 0.91, 0.81);
vec3 color3 = vec3(0.000, 1.000, 0.183);
vec3 color4 = vec3(0.561, 0.008, 1.000);
vec3 color5 = vec3(0.9f, 1.0f, 0.01f);

vec3 getPct(float s, float e, float v) {
	vec3 pct = vec3(0);
	pct.r = smoothstep(s, e, v);
	pct.g = pct.r;
	pct.b = pct.r;
	// pct.g = sin(v * PI);
	// pct.b = pow(v, 0.5);
	return pct;
}

void main() {
	vec2 st = gl_FragCoord.xy / u_resolution.xy;

	float y = st.y;

	vec3 pct;
	vec3 color;

	if(y >= 0.0 && y < 0.25) {
		pct = getPct(0.0, 0.25, y);
		color = mix(color1, color2, pct);

	} else if(y >= 0.25 && y < 0.5) {
		pct = getPct(0.25, 0.5, y);
		color = mix(color2, color3, pct);

	} else if(y >= 0.5 && y < 0.75) {
		pct = getPct(0.5, 0.75, y);
		color = mix(color3, color4, pct);

	} else {
		pct = getPct(0.75, 1.0, y);
		color = mix(color4, color5, pct);

	}
	gl_FragColor = vec4(color, 1.0);
}
