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

float genCross(vec2 uv, vec2 pos, vec2 size) {
  float box1 = box(uv, pos, size);
  float box2 = box(uv, pos, size.yx);

  return box1 + box2;
}

float f5(float x, float t) {
  return (t + floor(x - t)) / 2.0 - 5.0;
}

mat2 rotate2d(float _angle) {
  return mat2(cos(_angle), -sin(_angle), sin(_angle), cos(_angle));
}

void main() {
  vec2 uv = gl_FragCoord.xy;
  vec2 center = u_resolution.xy / 2.0;
  vec3 color = vec3(0.0);

  vec2 translate = vec2(cos(u_time), sin(u_time));
  // vec2 translate = vec2(cos(f5(uv.x, u_time)), sin(f5(uv.x, u_time)));
  uv += translate * 300.0;

  uv -= vec2(0.5);
  mat2 rotate = rotate2d(sin(u_time)*PI);
  uv *= rotate * uv;
  uv += vec2(0.5);

  // float translateX = 4.0 + 4.0 * smoothstep(0.0, 0.7, sin(u_time * 10.0));
  // float translateY = sin(f5(uv.x, u_time)) - 5.0;
  // uv += vec2(translateY, translateY) * 30.0;

  float colorPct = abs(sin(u_time));

  vec3 cross1 = genCross(uv, center, vec2(200.0, 50.0)) * u_color;

  color = mix(color, cross1, colorPct);

  gl_FragColor = vec4(color, 1.0);
}
