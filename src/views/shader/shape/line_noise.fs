precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float pix;

float SMOOTH(float D, float d, float pixNum) {
  return smoothstep(D - pixNum * pix, D + pixNum * pix, d);
}

float rand(float x) {
  return fract(sin(x) * 1.0);
}

float noise(float x) {
  float i = floor(x);
  float f = fract(x);
  return mix(rand(i), rand(i + 1.0), smoothstep(0., 1., f));
}

float plot(vec2 st) {
  return 1. - SMOOTH(0., abs(st.y - noise(st.x)), 40.);
}

void main() {
  vec2 st = gl_FragCoord.xy / iResolution.y;
  pix = 1. / iResolution.y;
  vec3 finc = vec3(0.0);

  float ratio = 4.;

  st *= ratio;
  st.y -= 1.;
  st.x += iTime * 4.;

  vec3 c = vec3(1.0,0.0,0.0);
  finc += c * plot(st);

  gl_FragColor = vec4(finc, 1.0);
}