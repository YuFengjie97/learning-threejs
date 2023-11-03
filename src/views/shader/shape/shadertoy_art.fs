precision mediump float;

#define S smoothstep;
#define PI 3.1415926;


uniform float iTime;
uniform vec2 iResolution;

vec3 palette(float t) {
  vec3 a = vec3(0.5, 0.5, 0.50);
  vec3 b = vec3(0.5, 0.5, 0.5);
  vec3 c = vec3(1.0, 1.0, 1.0);
  vec3 d = vec3(0.00, 0.33, 0.67);

  // return a + b * cos(2.0 * PI * (c * t + d)); // 为什么这么写不行？？？
  float TWO_PI = 2.0 * PI;
  return a + b * cos(TWO_PI * (c * t + d));
}

void main() {
  // vec2 uv = gl_FragCoord.xy / iResolution.xy * 2.0 - 1.0;
  // uv.x *= iResolution.x / iResolution.y;
  vec2 uv = (gl_FragCoord.xy * 2.0 - iResolution.xy) / iResolution.y;

  float d = length(uv);

  vec3 col = palette(d);

  d = sin(d * 8.0 + iTime) / 8.0;
  d = abs(d);
  d = 0.002 / d;

  d = smoothstep(0.0, 0.1, d);
  col *= d;

  gl_FragColor = vec4(col, 1.0);
}