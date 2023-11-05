precision mediump float;
#define PI 3.1415926;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

// https://iquilezles.org/articles/distfunctions2d/
float sdSegment(in vec2 p, in vec2 a, in vec2 b) {
  vec2 pa = p - a, ba = b - a;
  float h = clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
  return length(pa - ba * h);
}


void main() {

  vec3 finc = vec3(0.0);

  vec2 center = iResolution.xy / 2.0;

  float line = 1.0 - step(4., (sdSegment(gl_FragCoord.xy, center, iMouse)));
  finc += line * vec3(1.0, 0.0, 0.0);

  float cir1 = 1.0 - smoothstep(0., 10., distance(center, gl_FragCoord.xy) - 5.0);
  float cir2 = 1.0 - smoothstep(0., 10., distance(iMouse, gl_FragCoord.xy) - 5.0);
  finc += cir1 * vec3(0.0, 0.0, 1.0);
  finc += cir2 * vec3(0.0, 1.0, 0.0);

  gl_FragColor = vec4(finc, 1.0);
}