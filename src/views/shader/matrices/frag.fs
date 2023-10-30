precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

#define PI 3.14159265359

// #define SMOOTH(D, d) smoothstep(D - 1.0,D + 1.0, d);

float SMOOTH(float D, float d) {
  return smoothstep(D - 1.0, D + 1.0, d);
}

float random(float x) {
  float a = 12.9898, b = 78.233, c = 43758.5453;
  float dt = dot(vec2(x, x), vec2(a, b));
  float sn = mod(dt, 3.14);
  return fract(sin(sn) * c);
}

vec3 randomColor(float t) {
  return vec3(random(t), random(t + 3.0), random(t + 4.0));
}

float box(vec2 uv, vec2 pos, vec2 size) {
  float t = pos.y + size.y / 2.0;
  float b = pos.y - size.y / 2.0;
  float l = pos.x - size.x / 2.0;
  float r = pos.x + size.x / 2.0;

  float w = SMOOTH(l, uv.x) - SMOOTH(r, uv.x);
  float h = SMOOTH(b, uv.y) - SMOOTH(t, uv.y);
  return w * h;
}

float box(vec2 uv, vec2 size) {
  float t = 0.0 + size.y / 2.0;
  float b = 0.0 - size.y / 2.0;
  float l = 0.0 - size.x / 2.0;
  float r = 0.0 + size.x / 2.0;

  float w = SMOOTH(l, uv.x) - SMOOTH(r, uv.x);
  float h = SMOOTH(b, uv.y) - SMOOTH(t, uv.y);
  return w * h;
}

float genCross(vec2 uv, vec2 size) {
  // float box1 = box(uv, pos, size);
  // float box2 = box(uv, pos, size.yx);
  float box1 = box(uv, size);
  float box2 = box(uv, size.yx);
  return box1 + box2;
}

mat2 rotate2d(float _angle) {
  return mat2(cos(_angle), -sin(_angle), sin(_angle), cos(_angle));
}

mat2 scale(vec2 _scale) {
  return mat2(_scale.x, 0.0, 0.0, _scale.y);
}

void main() {
  vec2 uv = gl_FragCoord.xy;
  vec3 color = vec3(0.0);

  vec2 tCenter = u_resolution.xy / 2.0;
  uv -= tCenter;
  // uv = rotate2d(u_time * 4.0) * uv;

  uv = scale(vec2(sin(u_time) + 1.2)) * uv;

  // vec2 t1 = vec2(cos(u_time), sin(u_time));
  // uv += t1 * 1.0;

  float cross1 = genCross(uv, vec2(100.0, 20.0));
  color += cross1 * vec3(0.0f, 1.0f, 0.95f);

  gl_FragColor = vec4(color, 1.0);
}
