// 一个偶然发现
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

#define PI 3.14159265359

float random(vec2 _st, float t) {
  return fract(sin(dot(_st, _st) * 43708.0 + t));
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  float n = random(st, u_time);

  vec3 color = vec3(1.0);

  gl_FragColor = vec4(color * n, 1.0);
}