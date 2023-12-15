precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float PI = 3.1415926;
float pix;

vec2 rotate(vec2 p, float a) {
  float s = sin(a);
  float c = cos(a);
  mat2 matrix = mat2(c, -s, s, c);
  return p * matrix;
}
// 一般是返回旋转矩阵
// mat2 rotate(vec2 p, float a) {
//   float s = sin(a);
//   float c = cos(a);
//   return mat2(c, -s, s, c);
// }


float easeOutElastic(float x) {
  float c4 = (2. * PI) / 3.;
  return x == 0. ? 0. : x == 1. ? 1. : pow(2., -10. * x) * sin((x * 10. - 0.75) * c4) + 1.;
}

void main() {
  pix = 1. / iResolution.y;
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy) / min(iResolution.x, iResolution.y);
  vec3 fin_c = vec3(0., 0., 0.);

  float t = iTime * 0.8;

  vec3 color_ring = vec3(0.13, 0.27, 0.28);
  vec3 color_act = vec3(0.0, 0.96, 0.83);

  st = rotate(st, PI/4.*easeOutElastic(mod(t, 1.)));
  float d = length(st);
  float theta = mod(atan(st.y, st.x), PI * 2.);

  float ring = 1. - smoothstep(40. * pix, 44. * pix, abs(d - 0.5));

  float gap = 0.;
  for(float i = 0.; i < 8.; i++) {
    float a = PI / 4. * i;
    gap += 1. - smoothstep(90. * pix, 100. * pix, abs(mod(theta, PI / 2.) - a));
  }

  float actIndex = 7.-mod(floor(t*4.), 8.);
  //float actIndex = 7.-mod(floor(t), 8.);
  
  float act = (step(0., theta - actIndex * PI / 4.) - step(0., theta - (actIndex + 1.) * PI / 4.));

  float ring1 = ring - gap;
  float act2 = ring1 * act;
  float ring2 = ring1 - act2;

  fin_c += color_ring * ring2 + color_act * act2;

  gl_FragColor = vec4(fin_c, 1.0);
}