// 旋转三角
precision mediump float;

#define PI 3.14159265358979323846

uniform vec2 u_resolution;
uniform vec2 u_mouse;
uniform float u_time;
uniform vec3 u_color;

float genCorner(vec2 D, vec2 st) {
  vec2 halfS = smoothstep(D - vec2(0.01), D, st);
  vec2 halfE = smoothstep(D, D + vec2(0.01), st);
  vec2 c = halfS - halfE;
  return c.x + c.y;
}

float genBound(vec2 st) {
  float lb = genCorner(vec2(0.0), st);
  float rt = genCorner(vec2(1.0), st);
  return lb + rt;
}

void tile(inout vec2 st, float zoom) {
  st *= zoom;
  st = fract(st);
}

// 非常严谨的三角形，超出区域是白色
float triangle(vec2 st) {
  float z1 = 1.0 - smoothstep(st.x - 0.01, st.x, st.y);
  float z2 = smoothstep(0.0, 0.01, st.y);
  float z3 = 1.0 - smoothstep(1.0 - 0.01, 1.0, st.x);
  return z1 * z2 * z3;
}

// float triangle(vec2 st) {
//   float pix = 50.0 / u_resolution.x;
//   float z1 = smoothstep(0., -pix, st.y - st.x);
//   float z2 = smoothstep(0., pix, st.y);
//   float z3 = smoothstep(0., -pix, st.x - 1.);
//   return z1 * z2 * z3;
// }

void rotate2D(inout vec2 st, float angle) {
  st -= 0.5;
  st = mat2(cos(angle), -sin(angle), sin(angle), cos(angle)) * st;
  st += 0.5;
}

void rotateTilePattern(inout vec2 st) {
  st *= 2.0;
  float index = 0.0;
  index += step(1.0, mod(st.x, 2.0));
  index += step(1.0, mod(st.y, 2.0)) == 1.0 ? 2.0 : 0.0; // 在垂直方向上是加2

  if(index == 0.0) {
    // rotate2D(st, PI / 180.0 * 90.0);
  } else if(index == 1.0) {
    rotate2D(st, PI / 180.0 * 90.0);
  } else if(index == 2.0) {
    rotate2D(st, PI / 180.0 * 270.0);
  } else if(index == 3.0) {
    rotate2D(st, PI / 180.0 * 180.0);
  }

  st = fract(st);
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  vec3 color = vec3(0.0);

  tile(st, 2.0);

  rotateTilePattern(st);

  rotate2D(st, 10.0 * sin(u_time));

  float t = triangle(st);
  color += t * vec3(1.0);

  gl_FragColor = vec4(color, 1.0);
}
