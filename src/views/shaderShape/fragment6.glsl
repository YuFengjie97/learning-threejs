uniform vec2 u_resolution;

uniform float u_time;

uniform float u_r1;
uniform float u_r2;
uniform float u_r3;
uniform float u_r4;

vec3 circle(vec2 st, vec2 o, float r, vec3 color) {
  float l = length(st - o);
  float pct = smoothstep(r - r * 0.02, r, l);
  return color * pct;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;

  vec3 color = vec3(0.0);

  vec3 c1 = circle(st, vec2(0.4, 0.4), u_r1, vec3(1.0, 0.0, 1.0));

  vec3 c2 = circle(st, vec2(0.2, 0.1), u_r2, vec3(0.81f, 0.53f, 0.06f));

  vec3 c3 = circle(st, vec2(0.6, 0.5), u_r3, vec3(0.21f, 0.66f, 0.21f));

  vec3 c4 = circle(st, vec2(0.7, 0.6), u_r4, vec3(0.0f, 0.68f, 1.0f));

  color = c1 + c2 + c3 + c4;

  gl_FragColor = vec4(color, 1.0);
}