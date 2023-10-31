// 造型函数

#ifdef GL_ES
precision mediump float;
#endif

uniform float u_time;
uniform vec2 u_resolution;
uniform vec2 u_mouse;

// 造型函数
float gain(float x, float k) {
  float a = 0.5 * pow(2.0 * ((x < 0.5) ? x : 1.0 - x), k);
  return (x < 0.5) ? a : 1.0 - a;
}

// smooth过渡
float plot(vec2 st, float pct) {
  return smoothstep(pct + 0.02, pct, st.y) - smoothstep(pct, pct - 0.02, st.y);
}

// 随机函数
float random(float x) {
  return fract(sin(x));
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution;
  vec3 color = vec3(0.0);

  float tpct = 10.0;

  for(float k=-4.0;k<=4.0;k+=0.1) {
    float pct = gain(st.x, k);
    float cpct = plot(st, pct);
    color += cpct * vec3(random(u_time/pct + k),random(k), random(u_time/tpct + k));
  }

  gl_FragColor = vec4(color, 1.0);
}
