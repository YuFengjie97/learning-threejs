precision mediump float;

uniform float iTime;
uniform vec2 iResolution;

float pix;

float SMOOTH(float D, float d) {
  return smoothstep(D - pix, D + pix, d);
}

// A simple way to create color variation in a cheap way (yes, trigonometrics ARE cheap
// in the GPU, don't try to be smart and use a triangle wave instead).
// See https://iquilezles.org/articles/palettes for more information
vec3 pal(in float t, in vec3 a, in vec3 b, in vec3 c, in vec3 d) {
  return a + b * cos(6.28318 * (c * t + d));
}

void main() {
  vec2 p = gl_FragCoord.xy / iResolution.xy;
  pix = 10.0 / iResolution.y;

  p.x += 0.1 * iTime;

  // compute colors
  vec3 col = pal(p.x, vec3(0.5, 0.5, 0.5), vec3(0.5, 0.5, 0.5), vec3(1.0, 1.0, 1.0), vec3(0.0, 0.33, 0.67));
  if(p.y > (1.0 / 7.0))
    col = pal(p.x, vec3(0.5, 0.5, 0.5), vec3(0.5, 0.5, 0.5), vec3(1.0, 1.0, 1.0), vec3(0.0, 0.10, 0.20));
  if(p.y > (2.0 / 7.0))
    col = pal(p.x, vec3(0.5, 0.5, 0.5), vec3(0.5, 0.5, 0.5), vec3(1.0, 1.0, 1.0), vec3(0.3, 0.20, 0.20));
  if(p.y > (3.0 / 7.0))
    col = pal(p.x, vec3(0.5, 0.5, 0.5), vec3(0.5, 0.5, 0.5), vec3(1.0, 1.0, 0.5), vec3(0.8, 0.90, 0.30));
  if(p.y > (4.0 / 7.0))
    col = pal(p.x, vec3(0.5, 0.5, 0.5), vec3(0.5, 0.5, 0.5), vec3(1.0, 0.7, 0.4), vec3(0.0, 0.15, 0.20));
  if(p.y > (5.0 / 7.0))
    col = pal(p.x, vec3(0.5, 0.5, 0.5), vec3(0.5, 0.5, 0.5), vec3(2.0, 1.0, 0.0), vec3(0.5, 0.20, 0.25));
  if(p.y > (6.0 / 7.0))
    col = pal(p.x, vec3(0.8, 0.5, 0.4), vec3(0.2, 0.4, 0.2), vec3(2.0, 1.0, 1.0), vec3(0.0, 0.25, 0.25));

  float f = fract(p.y * 7.0);
  col *= smoothstep(0.49, 0.47, abs(f - 0.5));
    // shadowing
  col *= 0.5 + 0.5 * sqrt(4.0 * f * (1.0 - f));

  gl_FragColor = vec4(col, 1.0);
}