precision mediump float;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

float pix;
float PI = 3.141592653589793;

float SMOOTH(float D, float d, float pixNum) {
  return smoothstep(D - pixNum * pix, D + pixNum * pix, d);
}

float easeOutQuart(float x) {
  return 1. - pow(1. - x, 4.);
}

vec2 toPolar(vec2 st) {
  float r = length(st);
  float theta = mod(atan(st.y, st.x), 2. * PI);

  return vec2(r, theta);
}

// See https://iquilezles.org/articles/palettes for more information
vec3 palette(float t) {
  vec3 a = vec3(0.610, 0.498, 0.650);
  vec3 b = vec3(0.388, 0.498, 0.350);
  vec3 c = vec3(0.530, 0.498, 0.620);
  vec3 d = vec3(3.438, 3.012, 4.025);
  return a + b * cos(6.28318 * (c * t + d));
}

void main() {
  pix = 1.0 / iResolution.y;

  vec2 st = (gl_FragCoord.xy * 2.0 - iResolution.xy) / min(iResolution.x, iResolution.y);
  st = toPolar(st);

  vec3 c_fin = vec3(0.0);

  st.x *= 8.;

  vec3 c1 = palette(abs(sin(st.y - iTime)));
  float roateSpeed = PI * easeOutQuart(abs(sin(iTime)));
  float scale = 10. * clamp(abs(sin(iTime)), 0.1, 1.0);
  float s1 = 1. - SMOOTH(0., st.x - abs(scale * sin(4. * (st.y + roateSpeed))), 4.);
  c_fin += s1 * c1;

  gl_FragColor = vec4(c_fin, 1.0);
}