#include "../../lygiz/generative/noised.glsl"

precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

#define PI 3.14159265359;

void main() {
  
  float v = gl_FragCoord.x + gl_FragCoord.y + u_time;
  vec3 color = vec3(perlinNoise(v));

  gl_FragColor = vec4(color, 1.0);
}
