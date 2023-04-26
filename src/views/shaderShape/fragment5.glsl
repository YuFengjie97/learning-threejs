uniform vec2 u_resolution;

/**
矩形使用border-box模型
st像素归一化的坐标
pos矩形左上角的坐标
*/
vec3 rect(vec2 st, float borderWidth) {
  float left = step(borderWidth, st.x);
  float bottom = step(borderWidth, st.y);
  float right = step(borderWidth, 1.0 - st.x);
  float top = step(borderWidth, 1.0 - st.y);

  return vec3(left * bottom * right * top);
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;
  vec3 color = vec3(0.0);

  color = rect(st, 0.1);

  gl_FragColor = vec4(color, 1.0);
}