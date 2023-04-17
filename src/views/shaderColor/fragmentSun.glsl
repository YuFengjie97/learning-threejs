#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

vec3 colorA = vec3(0.2f, 0.5f, 0.9f);
vec3 colorB = vec3(0.68f, 0.13f, 0.02f);
vec3 colorSun = vec3(1.0f, 0.23f, 0.02f);

// s到e区域进行平滑插值
float getPct(float s, float e, float v) {
  return smoothstep(s, e, v);
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;

  vec3 baseColor = mix(vec3(colorA), vec3(0.0), vec3(0.7));

  vec3 color = baseColor;

  vec3 skyColor = mix(colorA, colorB, getPct(-0.6,1.0, st.x));

  color = mix(color, skyColor, getPct(-0.2, 1.0, st.y));

  vec2 sunPos = vec2(0.85, fract(-u_time / 5.0)); // 太阳位置的y坐标，一直循环在画面上

  float dist = distance(st, sunPos); // 像素坐标距离太阳距离，使用归一化坐标

  // color = mix(colorSun, colorA, getPct(0.2, 0.4, (dist)));

  // 太阳
  if( dist<= 0.05) {
    color = colorSun;
  }

  gl_FragColor = vec4(color, 1.0);
}
