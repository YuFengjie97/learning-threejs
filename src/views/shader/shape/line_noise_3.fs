precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float pix;

float SMOOTH(float D, float d, float pixNum) {
  return smoothstep(D - pixNum * pix, D + pixNum * pix, d);
}

float hash11(float p)
{
    p = fract(p * .1031);
    p *= p + 33.33;
    p *= p + p;
    return fract(p);
}

float plot(vec2 st, float k) {
  return 1. - SMOOTH(0., abs(st.y - hash11(st.x + k)), 100.);
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

  for(float k = 0.; k < 1.; k += 1.) {
    vec3 c = palette(hash11(st.x - k + iTime * 0.001));
    finc += c * plot(st, k);
  }

  gl_FragColor = vec4(finc, 1.0);
}