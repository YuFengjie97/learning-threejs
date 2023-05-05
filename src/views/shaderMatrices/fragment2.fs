precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

#define PI 3.14159265359;

float box(vec2 st, vec2 size) {
  vec2 uv = smoothstep(size, size + vec2(0.01), st);
  uv *= smoothstep(size, size + vec2(0.01), vec2(1.0) - st);
  return uv.x * uv.y;
}

mat2 rotate2d(float _angle) {
  return mat2(cos(_angle), -sin(_angle), sin(_angle), cos(_angle));
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;
  vec2 size = vec2(0.3);

  vec2 translate = vec2(cos(u_time), sin(u_time));
  st += translate * 0.3;

  st = rotate2d(sin(u_time)*PI) * st;

  float b1 = box(st, size);

  gl_FragColor = vec4(vec3(b1), 1.0);
}
