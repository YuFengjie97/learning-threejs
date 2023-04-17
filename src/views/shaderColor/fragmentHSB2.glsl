#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform float u_time;

//  Function from Iñigo Quiles
//  https://www.shadertoy.com/view/MsS3Wc
vec3 hsb2rgb(in vec3 c) {
  vec3 rgb = clamp(abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
  rgb = rgb * rgb * (3.0 - 2.0 * rgb);
  return c.z * mix(vec3(1.0), rgb, c.y);
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;

  vec2 o = vec2(0.5, 0.5);

  vec2 c = st - o;

  float angle = atan(c.y, c.x); // 在hsb极坐标下，角度大小代表的是色相

  float radius = length(c) * 2.0; // 在hsb极坐标下，半径距离代表的是饱和度

  // float t = sin(u_time) * 3.0;
  float t = 1.0 - pow(abs(sin(PI * sin(u_time) / 2.0)), 0.5);

  vec3 rgb = hsb2rgb(vec3(angle / PI + t, radius, 1.0));

  gl_FragColor = vec4(rgb, 1.0);
}