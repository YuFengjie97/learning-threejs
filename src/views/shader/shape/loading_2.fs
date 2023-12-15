// #version 330 es

precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float TWO_PI = 6.2831852;

float easeOutElastic(float x) {
  float c4 = TWO_PI / 3.;
  return exp2(-10. * x) * sin((x * 10. - 0.75) * c4);
}

void main() {
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy);
  vec3 color_fin = vec3(0.);

  float time = iTime * 1000.;
  float cycle = 1000.;
  float cycleIndex = floor(time / cycle);
  float cycleProgress = fract(time / cycle);
  float t = easeOutElastic(cycleProgress);

  vec3 color_ring = vec3(0.13, 0.27, 0.28);
  vec3 color_act = vec3(0.0, 0.96, 0.83);

  float d = length(st);

  float w = iResolution.y * 0.07;
  float ring = smoothstep(w + 4., w, abs(d - iResolution.y * 0.5));

  float gapNum = 8.;
  float gapSize = TWO_PI / (gapNum * 2.);

  float theta = mod(atan(st.y, st.x), TWO_PI) + t * gapSize;

  float gap = mod(floor(theta / gapSize), 2.);
  float ring2 = ring - gap;

  float progress = ring2 * (step(0., theta) - step(0., theta - gapSize * (mod(cycleIndex, gapNum * 2.)+1. + t)));
  color_fin += ring2 * color_ring;
  color_fin = mix(color_fin, color_act, progress) ;

  gl_FragColor = vec4(color_fin, 1.0);
}