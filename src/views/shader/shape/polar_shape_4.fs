// 蝴蝶函数
precision mediump float;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

float PI = 3.141592653589793;

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

float butterfly(vec2 st) {
  float r = pow(3., sin(st.y)) - 2.*cos(4.*st.y) + pow(sin((2. * st.y - PI) / 24.),2.);
  return st.x - abs(r);
}

void main() {
  vec2 st = (gl_FragCoord.xy * 2.0 - iResolution.xy) / min(iResolution.x, iResolution.y);
  st = toPolar(st);
  st.x *= 4.;

  vec3 c_fin = vec3(0.0);

  vec3 c1 = palette((iTime));

  float v = butterfly(st);
  float s = smoothstep(0., 1., v);
  c_fin = mix(c1, c_fin, s);

  gl_FragColor = vec4(c_fin, 1.0);
}