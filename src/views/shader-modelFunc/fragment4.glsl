// 又一条曲线

#ifdef GL_ES
precision mediump float;
#endif

#define PI 3.14159265359

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;

float plot(vec2 st, float pct) {
  return smoothstep(pct - 0.02, pct, st.y) -
    smoothstep(pct, pct + 0.02, st.y);
}

void main() {
  // 屏幕像素坐标的规一化
  vec2 st = gl_FragCoord.xy / u_resolution;

  // 对st.x进行平滑差值处理，因为已经规一化，所以其实y=xx(3-2x),做了这么一个处理
  float y = smoothstep(0.0, 1.0, st.x);

  // 基准色，通过y来确定，到这一步，你可以在屏幕上看到黑色到白色的一个渐变,这个渐变规律遵循y=xx(3-2x)
  vec3 color = vec3(y);

  // plot函数是用来绘制线条的，st参数，在函数中只用到了st.y, 上面求得的y，被赋值给pct（percentage 百分比）
  float pct = plot(st, y);
  color = (1.0 - pct) * color + pct * vec3(0.0, 1.0, 0.0);

  gl_FragColor = vec4(color, 1.0);
  gl_FragColor = vec4(color, 1.0);
}