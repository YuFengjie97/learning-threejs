// shader中怎么画线，plot函数，在绘制垂直方向的线会消失
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

#define PI 3.14159265359

float random(vec2 st) {
  return fract(sin(dot(st.xy, vec2(12.9898, 78.233))) *
    43758.5453123);
}

float random(float seed) {
  return fract(sin(seed) * 100000.0);
}

float plot(vec2 st, float pct, float pix) {
  return smoothstep(pct - pix, pct, st.y) -
    smoothstep(pct, pct + pix, st.y);
}

float f5(float x, float t) {
  return (t + floor(x - t)) / 2.0 - 5.0;
}

void main() {
  float pix = 10.0 / u_resolution.x;
  vec2 st = gl_FragCoord.xy / u_resolution.xx;

  st.x -= 0.5;
  st.y -= 0.3;
  st *= 20.0;

  float y1 = 3.0 * sin(st.x) / st.x;
  float l1 = plot(st, y1, pix * 10.0);
  vec3 color1 = vec3(1.0,0.0,0.0);

  float y2 = sin(f5(st.x, u_time)) + 2.0;
  float l2 = plot(st, y2, pix * 10.0);
  vec3 color2 = vec3(0.11, 0.72, 0.67);

  vec3 color = l1 * color1 + l2 * color2;


  gl_FragColor = vec4(color, 1.0);
}