precision mediump float;

#define PI 3.14159265358979323846

uniform vec2 u_resolution;
uniform float u_time;

float pix;

float SMOOTH(float D, float d) {
  return smoothstep(D - pix, D + pix, d);
}

float justice(vec2 st) {
  return SMOOTH(0.5, st.x);
}

float strength(vec2 st) {
  st = (st - .5) * 2.1912 + .5;
  return SMOOTH(cos(st.y * PI) * 0.25 + 0.5, st.x);
}

float death(vec2 st) {
  return SMOOTH(-st.x + 1., st.y);
}

float wall(vec2 st) {
  return SMOOTH(0.45, st.x) - SMOOTH(0.55, st.x);
}

float highPriestess(vec2 st) {
  vec2 pos = st;
  float c1 = 1. - SMOOTH(0.3, length( pos - 0.5));
  float c2 = 1. - SMOOTH(0.25, length(pos - 0.5));
  return c1 - c2;
}

void main() {
  pix = 1.0 / u_resolution.x;

  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  vec3 color = vec3(1.0);

  float s = justice(st);
  // s = strength(st);
  // s = death(st);
  s = wall(st);
  s = highPriestess(st);
  color = color * s;

  gl_FragColor = vec4(color, 1.0);
}
