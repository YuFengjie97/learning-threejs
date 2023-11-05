precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float pix;

float SMOOTH(float D, float d, float pixNum) {
  return smoothstep(D - pixNum * pix, D + pixNum * pix, d);
}

// rand and noise is from https://thebookofshaders.com/11/?lan=ch
float rand(float x) {
  return fract(sin(x + iTime * 0.1) * 1.0);
}
float noise(float x) {
  float i = floor(x);
  float f = fract(x);
  return mix(rand(i), rand(i + 1.0), smoothstep(0., 1., f));
}

float plot(vec2 st, float k) {
  return 1. - SMOOTH(0., abs(st.y - noise(st.x + k)), 100.);
}

// https://iquilezles.org/articles/palettes/
vec3 palette(float t) {
  vec3 a = vec3(0.186, 0.966, 0.731);
  vec3 b = vec3(0.199, 0.400, 0.604);
  vec3 c = vec3(1.432, 0.024, 0.755);
  vec3 d = vec3(0.048, 2.989, 2.952);

  float TWO_PI = 2.0 * PI;
  return a + b * cos(TWO_PI * (c * t + d));
}

void main() {
  vec2 st = gl_FragCoord.xy / iResolution.y;
  pix = 1. / iResolution.y;
  vec3 finc = vec3(0.0);

  float ratio = 4.;

  st *= ratio;
  st.y -= 1.;
  st.x += iTime;

  for(float k = 0.; k < 5.; k += .5) {
    vec3 c = palette(noise(st.x - k +iTime*0.001));
    finc += c * plot(st, k);
  }

  gl_FragColor = vec4(finc, 1.0);
}