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

void main() {
  float pix = 20.0 / u_resolution.x;
  vec2 st = gl_FragCoord.xy / u_resolution.xy;

  st.y -= 0.4;
  st *= 20.0;

  float tx = st.x + u_time;

  float i = floor(tx);
  float f = fract(tx);
  float y = mix(random(i), random(i + 1.0), smoothstep(0.0, 1.0, f));
  
  float l = plot(st, y, pix);

  gl_FragColor = vec4(l);
}