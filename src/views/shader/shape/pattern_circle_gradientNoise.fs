precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float size = 16.;

vec2 random2(vec2 st) {
  st = vec2(dot(st, vec2(0.970, 0.940)), dot(st, vec2(-0.320, -0.750)));
  return -1.0 + 2.0 * fract(sin(st) * 43758.697);
}

// Gradient Noise by Inigo Quilez - iq/2013
// https://www.shadertoy.com/view/XdXGW8
float noise(vec2 st) {
  vec2 i = floor(st);
  vec2 f = fract(st);

  vec2 u = f * f * (3.0 - 2.0 * f);

  vec2 a = random2(i + vec2(0.0, 0.0));
  vec2 b = random2(i + vec2(1., 0.0));
  vec2 c = random2(i + vec2(0.0, 1.));
  vec2 d = random2(i + vec2(1., 1.));

  float a2 = dot(a, f - vec2(0.0, 0.0));
  float b2 = dot(b, f - vec2(1., 0.0));
  float c2 = dot(c, f - vec2(0.0, 1.));
  float d2 = dot(d, f - vec2(1., 1.));

  float x1 = mix(a2, b2, u.x);
  float x2 = mix(c2, d2, u.x);
  return mix(x1, x2, u.y);
}

vec3 palette(float t) {
  vec3 a = vec3(0.000, 0.500, 0.500);
  vec3 b = vec3(0.000, 0.500, 0.500);
  vec3 c = vec3(0.000, 0.500, 0.333);
  vec3 d = vec3(0.000, 0.500, 0.667);
  return a + b * cos(6.28318 * (c * t + d));
}

float circle(vec2 uv, vec2 center, float r) {
  float d = distance(uv, center);
  return smoothstep(r * 0.4, 0., d - r);
}

float fbm(vec2 p) {
  float total = 0.0;
  float amplitude = 1.0;
  float frequency = 1.0;

  for(int i = 0; i < 5; i++) {
    total += amplitude * noise(p * frequency);
    amplitude *= 0.5;
    frequency *= 2.0;
  }

  return total;
}

void main() {
  vec2 uv = gl_FragCoord.xy / iResolution.xy;
  uv *= 10.;
  vec3 color_fin = vec3(0.);

  // float iy = floor(uv.y / size);
  // float ix = floor(uv.x / size);
  // float y = iy * size + size / 2.;
  // float x = ix * size + size / 2.;

  // vec2 center = vec2(x, y);
  // float n = noise(center);
  // float rn = (size / 2.) * n * 0.8;
  // vec3 color = palette(n);
  // float c = circle(uv, center, rn);
  // color_fin += color * c;
  float n = fbm(uv);
  // float n = noise(uv);
  color_fin += vec3(1.) * n;

  gl_FragColor = vec4(color_fin, 1.);
}