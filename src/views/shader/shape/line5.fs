precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float pix;

float w = 0.05;

float plotSinX(vec2 uv, float k) {
  float pct = uv.y - sin(cos(uv.x * 8.0+k) + iTime) / 3. - 0.5;
  float pixs = pix * 80.0;
  return 1.0 - smoothstep(w - pixs, w+pixs, abs(pct));
}

float plotSinY(vec2 uv, float k) {
  float pct = uv.x - sin(cos(uv.y * 8.0+k) + iTime) / 3. - 0.5;
  float pixs = pix * 80.0;
  return 1.0 - smoothstep(w - pixs, w+pixs, abs(pct));
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

  gl_FragColor = vec4(finc, 1.0);
}