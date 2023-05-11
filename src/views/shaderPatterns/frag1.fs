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
  float lb = genCorner(vec2(0.0), st);
  float rt = genCorner(vec2(1.0), st);
  return lb + rt;
}

// 在st划定的区域内的中心画一个size的矩形
float box(vec2 st, vec2 size) {
  vec2 lb = vec2(0.0);
  vec2 rt = size;

  vec2 uv = smoothstep(lb, lb + vec2(0.001), st);
  uv *= smoothstep(rt + vec2(0.001), rt, st);
  return uv.x * uv.y;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  // vec2 st = gl_FragCoord.xy / u_resolution.xy;
  // st.x *= u_resolution.x/u_resolution.y;

  vec3 color = vec3(0.57f, 0.8f, 0.79f);

  st *= vec2(10.0, 20.0);
  st.x += step(1.0, mod(st.y,2.0)) * 0.5;
  st = fract(st);

  vec2 boxSize = vec2(0.9, 0.8);
  st -= vec2(0.5) - boxSize * 0.5;
  float b = box(st, boxSize);
  if(b == 1.0) {
    color = b * vec3(0.88f, 0.35f, 0.07f); // 颜色的叠加才算是真正造型函数执行的那一步，所以transfrom应该在此之前和在此之后复原
  }
  st += vec2(0.5) - boxSize * 0.5;

  // float bound = genBound(st);
  // vec3 boundColor = vec3(0.91, 0.86, 0.28);
  // color += bound * boundColor;

  // if(bound != 0.0) {
  //   color *= 0.0;
  //   color += bound * boundColor;
  // }

  gl_FragColor = vec4(color, 1.0);
}
