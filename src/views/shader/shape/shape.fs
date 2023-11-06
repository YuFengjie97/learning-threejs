precision mediump float;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

float pix;
float PI = 3.1415926535898;

float SMOOTH(float D, float d, float pixNum) {
  return smoothstep(D - pixNum * pix, D + pixNum * pix, d);
}

// 在直角坐标系中绘制极坐标坐标系
// float polar(vec2 st, float gap) {
//   // float a = atan(st.x, st.y);

//   float res = 0.0;
//   // 同心圆
//   for(float i = 0.; i <= 1.; i += gap) {
//     res += 1.0 - SMOOTH(0.0, abs(length(st) - i), 2.);
//   }

//   float l1 = 1.0 - SMOOTH(0., abs(st.y), 2.);
//   res += l1;
//   float l2 = 1.0 - SMOOTH(0., abs(st.x), 2.);
//   res += l2;
//   float l3 = 1.0 - SMOOTH(0., abs(st.x - st.y), 2.);
//   res += l3;
//   float l4 = 1.0 - SMOOTH(0., abs(st.y + st.x), 2.);
//   res += l4;

//   return res;
// }

float polar(vec2 st, float gap) {
  float res = 0.0;

  // 同心圆
  for(float r = 0.0; r < 2.0; r += gap) {
    res += 1. - SMOOTH(0., abs(st.x - r), 4.);
  }

  //直线
  for(float a = 0.; a <= 2.0; a += .25) {
    res += 1. - SMOOTH(0., abs(PI * a - st.y), 40.);
  }

  return res;
}

vec2 toPolar(vec2 st) {
  // vec2(r, θ)
  float r = length(st);
  float a;

  if(st.x > 0. && st.y > 0.) {
    a = atan(st.y / st.x);
  }
  if(st.x < 0. && st.y > 0.) {
    a = atan(st.y / abs(st.x)) + PI;
  }
  if(st.x < 0. && st.y < 0.) {
    a = atan(abs(st.y) / abs(st.x)) + PI;
  }
  if(st.x > 0. && st.y < 0.) {
    a = atan(st.y / abs(st.x)) + PI * 2.;
  }

  return vec2(r, a);
}

void main() {
  pix = 1.0 / iResolution.y;

  vec2 st = (gl_FragCoord.xy * 2.0 - iResolution.xy) / iResolution.y;
  st = toPolar(st);

  vec3 c_fin = vec3(0.0);
  vec3 c_white = vec3(1.0);

  float pol = polar(st, 0.1);
  c_fin += c_white * pol;

  gl_FragColor = vec4(c_fin, 1.0);
}