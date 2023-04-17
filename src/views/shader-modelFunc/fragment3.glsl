// 又一条曲线，y = smoothstep(0,1,x),也就是0到1区间的  y = x*x*(3-2*x)

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

float plot(vec2 st, float pct) {
  float lineWidth = 0.005;
  // return smoothstep(pct - lineWidth, pct, st.y); // 大绿
  // return smoothstep(pct, pct + lineWidth, st.y); // 小绿
  // return smoothstep(pct - lineWidth, pct, st.y) -  smoothstep(pct, pct + lineWidth, st.y);
  // return smoothstep(pct, pct + lineWidth, st.y) - smoothstep(pct - lineWidth, pct, st.y); // 绿色的反义词是紫色

  return step(pct - lineWidth, st.y) - step(pct + lineWidth, st.y); // 小绿
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution;

  // float y = smoothstep(0.0, 1.0, st.x);
  float y = sin(st.x);

  vec3 color = vec3(y);

  float pct = plot(st, y);
  color = (1.0 - pct) * color + pct * vec3(0.0, 1.0, 0.0);

  gl_FragColor = vec4(color, 1.0);
  gl_FragColor = vec4(color, 1.0);
}