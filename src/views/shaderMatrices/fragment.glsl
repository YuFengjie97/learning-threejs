precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

vec3 plotRect(float base, vec3 baseColor, vec3 color, vec2 pos, float w, float h) {
	pos = pos.xy / base;
	w = w / base;
	h = w / base;
	float t = pos.y;
	float b = pos.y - h;
	float l = pos.x;
	float r = pos.x + w;
	float ver = smoothstep(b, t, pos.y);
	float hor = smoothstep(l, r, pos.x);
	float pct = ver * hor;
	if(pct == 0.0 || pct == 1.0) {
		return baseColor;
	} else {
		return color;
	}
}

void main() {
	float base = u_resolution.y;
	vec3 baseColor = vec3(0.3);
	vec3 color = plotRect(base, baseColor, vec3(0.07f, 0.77f, 0.79f), vec2(200.0), 100.0, 100.0);
	gl_FragColor = vec4(color, 1.0);
}
