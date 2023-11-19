precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float rand(float x) {
  return fract(sin(x) * 1.0);
}

float noise(float x) {
  float i = floor(x);
  float f = fract(x);
  return mix(rand(i), rand(i + 1.0), smoothstep(0., 1., f));
}

float plot(vec2 st) {
  return 1. - smoothstep(0., 0.3, abs(st.y-noise(st.x)));
}

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
  vec3 finc = vec3(0.0);
  st.y -= 0.5;
  st *= 10.;

  for(float i=0.;i<4.;i++) {
    vec2 uv = st;
    uv.x += iTime + i;
    float l1 = plot(uv);
    finc = mix(finc, palette(i), l1);
  }

  gl_FragColor = vec4(finc, 1.0);
}