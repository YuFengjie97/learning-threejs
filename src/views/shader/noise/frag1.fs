// 雪花随机噪音
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

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;

  st *= 30.0;
  vec2 ipos = floor(st);
  vec2 fpos = fract(st);

  vec3 color = vec3(random(ipos));

  gl_FragColor = vec4(color, 1.0);
}