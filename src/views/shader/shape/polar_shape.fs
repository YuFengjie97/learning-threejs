precision mediump float;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

float pix;
float PI = 3.141592653589793;

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
    res += 1. - SMOOTH(0., abs(PI * a - st.y), 10.);
  }
  return res;
}

vec2 toPolar(vec2 st) {
  float r = length(st);
  float theta = mod( atan(st.y,st.x), 2.*PI);
  // float theta = atan(st.y,st.x);

  return vec2(r, theta);
}

void main() {
  pix = 1.0 / iResolution.y;

  vec2 st = (gl_FragCoord.xy * 2.0 - iResolution.xy) / min(iResolution.x, iResolution.y);
  st = toPolar(st);

  vec3 c_fin = vec3(0.0);
  vec3 c_white = vec3(1.0);

  float pol = polar(st, 0.1);
  c_fin += c_white * pol;

  st.x *= 10.;

  float v = st.x - abs(5. * sin(4. * st.y));
  float s1 = 1. - smoothstep(-0.,1., v);
  c_fin = mix(c_fin, c_white, s1);

  gl_FragColor = vec4(c_fin, 1.0);
}