// 格子衬衫
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

#define PI 3.14159265359

void tile(inout vec2 st, float n) {
  st *= n;
  st = fract(st);
}

void rotate2D(inout vec2 _st, float _angle) {
  _st -= 0.5;
  _st = mat2(cos(_angle), -sin(_angle), sin(_angle), cos(_angle)) * _st;
  _st += 0.5;
}

float SMOOTH(float D, float d) {
  return smoothstep(D - 0.01, D + 0.01, d);
}

float box(vec2 size, vec2 st) {
  float t = 0.0 + size.y / 2.0;
  float b = 0.0 - size.y / 2.0;
  float l = 0.0 - size.x / 2.0;
  float r = 0.0 + size.x / 2.0;

  float h = SMOOTH(b, st.y) - SMOOTH(t, st.y);
  float w = SMOOTH(l, st.x) - SMOOTH(r, st.x);
  return h * w;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  vec3 color = vec3(0.0);

  bool isRotate = false;

  tile(st, 4.0);

  if (isRotate) {
    rotate2D(st, sin(u_time));
  } else {
    rotate2D(st, PI / 180.0 * -45.0);
  }
  st -= vec2(0.5);

  float b = box(vec2(0.2) * (abs(cos(u_time))) + vec2(0.2), st);
  vec3 bColor = vec3(0.61f, 0.04f, 0.04f);
  color += b * bColor;
  // st += vec2(0.5);


  float lW = abs(sin(u_time)) * 0.1 + 0.05;
  float l1 = SMOOTH(0.0 - lW / 2.0, st.x) - SMOOTH(lW / 2.0, st.x);
  float l2 = SMOOTH(0.0 - lW / 2.0, st.y) - SMOOTH(lW / 2.0, st.y);
  float l3 = SMOOTH(1.0 - lW / 2.0, st.x) - SMOOTH(1.0 + lW / 2.0, st.x);
  float l4 = SMOOTH(1.0 - lW / 2.0, st.y) - SMOOTH(1.0 + lW / 2.0, st.y);
  vec3 lineColor = vec3(0.84f, 0.51f, 0.07f);
  color += (l1 + l2 +l3 + l4 ) * lineColor;

  gl_FragColor = vec4(color, 1.0);
}