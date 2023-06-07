// 格子衬衫
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

#define PI 3.14159265359

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;

  vec3 color = vec3(1.0);

  gl_FragColor = vec4(color, 1.0);
}