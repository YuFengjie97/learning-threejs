precision mediump float;

#define PI 3.14159265358979323846

uniform vec2 u_resolution;
uniform float u_time;

float SMOOTH(float D, float d) {
  return smoothstep(D - 0.01, D + 0.01, d);
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  vec3 color = vec3(0.0);

  gl_FragColor = vec4(color, 1.0);
}
