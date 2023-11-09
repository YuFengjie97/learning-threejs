precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

float pix = 0.;

// https://iquilezles.org/articles/distfunctions2d/
float sdTriangleIsosceles(in vec2 p, in vec2 q) {
  p.x = abs(p.x);
  vec2 a = p - q * clamp(dot(p, q) / dot(q, q), 0.0, 1.0);
  vec2 b = p - q * vec2(clamp(p.x / q.x, 0.0, 1.0), 1.0);
  float s = -sign(q.y);
  vec2 d = min(vec2(dot(a, a), s * (p.x * q.y - p.y * q.x)), vec2(dot(b, b), s * (p.y - q.y)));
  return -sqrt(d.x) * sign(d.y);
}

float sdEquilateralTriangle(in vec2 p, in float r) {
  const float k = sqrt(3.0);
  p.x = abs(p.x) - r;
  p.y = p.y + r / k;
  if(p.x + k * p.y > 0.0)
    p = vec2(p.x - k * p.y, -k * p.x - p.y) / 2.0;
  p.x -= clamp(p.x, -2.0 * r, 0.0);
  return -length(p) * sign(p.y);
}

float coor(vec2 st) {
  float res = 0.0;
  for(float i = -2.; i < 2.; i += 0.1) {
    float v = abs(st.x - i);
    float s = 1. - smoothstep(0., pix * 2., v);
    res += s;
  }
  for(float i = -2.; i < 2.; i += 0.1) {
    float v = abs(st.y - i);
    float s = 1. - smoothstep(0., pix * 2., v);
    res += s;
  }
  return res;
}

void main() {
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy) / min(iResolution.x, iResolution.y);
  vec3 c_fin = vec3(0.0);
  vec3 c_white = vec3(1.0);
  vec3 c_red = vec3(1.0, 0., 0.);

  pix = 1. / iResolution.y;

  c_fin += coor(st) * c_white;

  float v = sdEquilateralTriangle(st, .2);
  float s = 1. - smoothstep(0., pix * 40., v);
  c_fin = mix(c_fin, c_red, s);

  gl_FragColor = vec4(c_fin, 1.0);
}