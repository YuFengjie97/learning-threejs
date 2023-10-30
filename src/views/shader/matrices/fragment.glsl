precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

/**
 * pos: 矩形左下角坐标
 */
vec3 plotRect(vec2 resolution, vec2 st, vec3 baseColor, vec3 color, vec2 pos, float w, float h) {
	st = st.xy / resolution.y;
	pos = pos.xy / resolution.y;
	w = w / resolution.y;
	h = h / resolution.y;
	float t = pos.y + h;
	float b = pos.y;
	float l = pos.x;
	float r = pos.x + w;
	float ver = smoothstep(b, t, st.y);
	float hor = smoothstep(l, r, st.x);

	if (ver == 1.0) {
		ver = 0.0;
	}
	if (hor == 1.0) {
		hor = 0.0;
	}
	float pct = ver * hor;

	if (pct == 0.0) {
		return baseColor;
	} else {
		// return color;
		return color * pct;
	}
}

void main() {
	vec3 baseColor = vec3(0.3);

	vec2 st = gl_FragCoord.xy;

	vec3 color = plotRect(u_resolution, st, baseColor, vec3(0.07f, 0.77f, 0.79f), vec2(400.0, 400.0),400.0, 400.0);
	
	gl_FragColor = vec4(color, 1.0);
}
