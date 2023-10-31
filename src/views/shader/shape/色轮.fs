#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718

uniform vec2 u_resolution;
uniform float u_time;

float pix;

float SMOOTH(float D, float d) {
  return smoothstep(D - pix, D + pix, d);
}

float circle(vec2 st, float r) {
  vec2 center = vec2(0.5);
  float dist = distance(st, center);
  return 1.0 - SMOOTH(r, dist);
}

// https://thebookofshaders.com/edit.php#06/easing.frag
float bounceOut(float t) {
  const float a = 4.0 / 11.0;
  const float b = 8.0 / 11.0;
  const float c = 9.0 / 10.0;

  const float ca = 4356.0 / 361.0;
  const float cb = 35442.0 / 1805.0;
  const float cc = 16061.0 / 1805.0;

  float t2 = t * t;

  return t < a
    ? 7.5625 * t2
    : t < b
      ? 9.075 * t2 - 9.9 * t + 3.4
      : t < c
        ? ca * t2 - cb * t + cc
        : 10.8 * t * t - 20.52 * t + 10.72;
}

//  Function from Iñigo Quiles
//  https://www.shadertoy.com/view/MsS3Wc
vec3 hsb2rgb(in vec3 c) {
  vec3 rgb = clamp(abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
  rgb = rgb * rgb * (3.0 - 2.0 * rgb);
  return c.z * mix(vec3(1.0), rgb, c.y);
}

void main() {
  vec2 st = gl_FragCoord.xy / min(u_resolution.x, u_resolution.y);
  pix = 1.0 / u_resolution.x;
  vec3 color = vec3(0.0);

  vec2 toCenter = vec2(0.5) - st;
  float angle = atan(toCenter.y, toCenter.x) + bounceOut( sin(u_time));
  float radius = length(toCenter) * 2.0;
  vec3 c = hsb2rgb(vec3((angle / TWO_PI) + 0.5, radius, 1.0));

  float pct = circle(st, 0.4) - circle(st, 0.3);

  color = pct * c;

  gl_FragColor = vec4(color, 1.0);
}