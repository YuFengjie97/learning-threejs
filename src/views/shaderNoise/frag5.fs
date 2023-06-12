// 柏林噪音
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

#define PI 3.14159265359

vec2 random2(vec2 st) {
  st = vec2(dot(st, vec2(127.1, 311.7)), dot(st, vec2(269.5, 183.3)));
  return -1.0 + 2.0 * fract(sin(st) * 43758.5453123);
}

// Gradient Noise by Inigo Quilez - iq/2013
// https://www.shadertoy.com/view/XdXGW8
float noise(vec2 st) {
  vec2 i = floor(st);
  vec2 f = fract(st);

  vec2 u = f * f * (3.0 - 2.0 * f);

  return mix(mix(dot(random2(i + vec2(0.0, 0.0)), f - vec2(0.0, 0.0)), dot(random2(i + vec2(1.0, 0.0)), f - vec2(1.0, 0.0)), u.x), mix(dot(random2(i + vec2(0.0, 1.0)), f - vec2(0.0, 1.0)), dot(random2(i + vec2(1.0, 1.0)), f - vec2(1.0, 1.0)), u.x), u.y);
}

float block(vec2 st, vec2 pos, float size) {
  vec2 lb = pos - size / 2.0;
  vec2 rt = pos + size / 2.0;
  float l = lb.x;
  float b = lb.y;
  float r = rt.x;
  float t = rt.y;

  if(st.x >= l && st.x <= r && st.y >= b && st.y <= t) {
    return noise(st * 20.0);
    // return  1.0;
  }
  return 1.0;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;

  float b = block(st, vec2(0.5), 0.4);

  gl_FragColor = vec4(vec3(b), 1.0);
}