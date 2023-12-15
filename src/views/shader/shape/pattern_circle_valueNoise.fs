precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;

float size = 16.;

float random(vec2 uv) {
  // return fract(sin(dot(uv.xy, vec2(12.9898, 78.233))) * 43758.5453123);
  return fract((sin(dot(uv.xy, vec2(12.9898, 78.233))) + 1.) * 43758.5453123 + iTime);
}

float noise(vec2 uv) {
  vec2 i = floor(uv);
  vec2 f = fract(uv);

  float a = random(i);
  float b = random(i + vec2(size, 0));
  float c = random(i + vec2(0., size));
  float d = random(i + vec2(size));

  vec2 u = f * f * (3. - 2. * f);
  // vec2 u = smoothstep(0., 1., f);

  float v1 = mix(a, b, u.x);
  float v2 = mix(c, d, u.x);
  return mix(v1, v2, u.y);
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

void main() {
  vec2 uv = gl_FragCoord.xy;
  vec3 color_fin = vec3(0.);

  float iy = floor(uv.y / size);
  float ix = floor(uv.x / size);
  float y = iy * size + size / 2.;
  float x = ix * size + size / 2.;

  vec2 center = vec2(x, y);
  float n = noise(center);
  float rn = (size / 2.) * n * 0.8;
  vec3 color = palette(n);
  float c = circle(uv, center, rn);
  color_fin += color * c;

  gl_FragColor = vec4(color_fin, 1.);
}