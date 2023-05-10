// 砖墙
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

// D坐标(你要在哪里画线)，d归一化的坐标
// float genLine(float D, float d) {
//   float halfL = smoothstep(D - 0.01, D, d);
//   float halfR = smoothstep(D, D + 0.01, d);
//   return halfL - halfR;
// }

// 对归一化的坐标，最大尺寸，画边界
// float genBound(vec2 st) {
//   float l = genLine(0.0, st.x);
//   float r = genLine(1.0, st.x);
//   float b = genLine(0.0, st.y);
//   float t = genLine(1.0, st.y);
//   return l + r + b + t;
// }

float genCorner(vec2 D, vec2 st) {
  vec2 halfS = smoothstep(D - vec2(0.01), D, st);
  vec2 halfE = smoothstep(D, D + vec2(0.01), st);
  vec2 c = halfS - halfE;
  return c.x + c.y;
}

float genBound(vec2 st) {
  // 这是不使用genCorner的完整写法
  // vec2 lb = smoothstep(vec2(0.0) - vec2(0.01), vec2(0.0), st) - smoothstep(vec2(0.0), vec2(0.0) + vec2(0.01), st);
  // vec2 rt = smoothstep(vec2(1.0) - vec2(0.01), vec2(1.0), st) - smoothstep(vec2(1.0), vec2(1.0) + vec2(0.01), st);
  // return lb.x + lb.y + rt.x + rt.y;
  float lb = genCorner(vec2(0.0),st);
  float rt = genCorner(vec2(1.0),st);
  return lb + rt;
}

// 在st划定的区域内的中心画一个size的矩形
float box(vec2 st, vec2 size) {
  st = vec2(0.5) - size * 0.5;
  vec2 lb = vec2(0.0);
  vec2 rt = size;

  vec2 uv = smoothstep(lb, lb + vec2(0.001), st);
  uv *= smoothstep(rt + vec2(0.001), rt, st);
  return uv.x * uv.y;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  vec3 color = vec3(0.0);

  st *= 4.0;
  st = fract(st);

  vec2 boxSize = vec2(0.25, 0.25);
  st -= vec2(0.5);
  float b = box(st, boxSize);
  st += vec2(0.5);
  color += b * vec3(0.29, 0.79, 0.15);

  float bound = genBound(st);
  vec3 boundColor = vec3(0.91, 0.86, 0.28);
  color += bound * boundColor;
  // if(bound != 0.0) {
  //   color *= 0.0;
  //   color += bound * boundColor;
  // }

  gl_FragColor = vec4(color, 1.0);
}
