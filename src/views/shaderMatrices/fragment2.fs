precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

#define PI 3.14159265359;

float circle(vec2 uv, vec2 o, float r, float w) {
  float d = distance(uv, o);

  // float cOut = 1.0 - step(r, d);
  // float cInner = 1.0 - step(r - w, d);
  // return cOut - cInner;

  // float cOut = 1.0 - smoothstep(r - w, r, d);
  // float cInner = 1.0 - step(r - w, d);
  // return cOut - cInner;

  float cOut = smoothstep(r - w / 2.0, r, d);
  float cInner = smoothstep(r, r + w / 2.0, d);
  return cOut - cInner;
}

void main() {
  vec3 colorBase = vec3(0.0);

  colorBase += circle(gl_FragCoord.xy, vec2(300.0), 100.0, 40.0) * vec3(0.0, 1.0, 0.1);

  gl_FragColor = vec4(colorBase, 1.0);
}
