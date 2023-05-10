// 砖墙
precision mediump float;

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

// 已知矩形中心位置和尺寸
// float box(vec2 st, vec2 pos, vec2 size) {
//   st += size / 2.0;
//   vec2 lb = pos;
//   vec2 rt = pos + size;

//   vec2 uv = smoothstep(lb, lb + vec2(0.001), st);
//   uv *= smoothstep(rt + vec2(0.001), rt, st);
//   return uv.x * uv.y;
// }

// 在st中心绘制矩形,现在0,0上绘制矩形，然后平移半个size单位
float box(vec2 st, vec2 size) {
  vec2 lb = vec2(0.0);
  vec2 rt = size;

  vec2 uv = smoothstep(lb, lb + vec2(0.001), st);
  uv *= smoothstep(rt + vec2(0.001), rt, st);
  return  uv.x * uv.y;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;

  st *= 4.0;
  st = fract(st);

  vec2 boxSize = vec2(0.9, 0.9);
  float b = box(st, boxSize);

  gl_FragColor = vec4(vec3(b), 1.0);
}
