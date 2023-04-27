uniform vec2 u_resolution;

uniform float u_time;

uniform float u_r1;
uniform float u_r2;
uniform float u_r3;
uniform float u_r4;

float rand(vec2 co)
{
  return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
}

bool circle(vec2 st, vec2 o, float r) {
  float l = length(st - o);
  float pct = smoothstep(r - r * 0.02, r, l);
  if (pct <= r) {
    return true;
  }
  return false;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;

  vec3 color = vec3(1.0);

  bool c1 = circle(st, vec2(0.4, 0.4), u_r1);
  if (c1) {
    color = vec3(1.0, 0.0, 1.0);
  }

  bool c2 = circle(st, vec2(0.2, 0.1), u_r2);
  if (c2) {
    color = vec3(0.81f, 0.53f, 0.06f);
  }

  bool c3 = circle(st, vec2(0.6, 0.5), u_r3);
  if (c3) {
    color = vec3(0.21f, 0.66f, 0.21f);
  }

  bool c4 = circle(st, vec2(0.7, 0.6), u_r4);
  if (c4) {
    color = vec3(0.0f, 0.68f, 1.0f);
  }

  gl_FragColor = vec4(color, 1.0);
}