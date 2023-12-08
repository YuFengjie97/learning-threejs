// 多组circle重叠绘制色彩
precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float PI = 3.1415926;
float pix;

vec3 palette(float t) {
  vec3 a = vec3(1.000,0.500,0.500);
  vec3 b = vec3(0.500,0.500,0.500);
  vec3 c = vec3(0.750,1.000,0.667);
  vec3 d = vec3(0.800,1.000,0.333);
  return a + b * cos(6.28318 * (c * t + d));
}

float cirlceMore(vec2 st, vec2 center) {
  float waves = PI*2.;

  float d = distance(st, center) - iTime*0.3;
  d = abs(sin(d * waves));
  return 1. - smoothstep(0., 1., d);
}

vec3 getCC(vec2 st, vec2 pos, vec3 color) {
  float c = cirlceMore(st, pos);
  return c * color;
}

void main() {
  pix = 1. / iResolution.y;
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy) / min(iResolution.x, iResolution.y);
  vec3 fin_c = vec3(0., 0., 0.);

  vec2 bottom = vec2(0., -1.);
  vec2 top = vec2(0., 1.);
  vec2 left = vec2(-1., 0.);
  vec2 right = vec2(1., 0.);

  // vec3 color_blue = vec3(0.0, 0.71, 1.0);
  // float l1 = 1.0-smoothstep(0.,0.05,abs(st.x-1.));
  // float l2 = 1.0-smoothstep(0.,0.05,abs(st.y-1.));
  // fin_c += (l1+l2)*color_blue;

  vec3 color1 = palette(iTime*0.5+10.);
  vec3 color2 = palette(iTime*0.5+10.);
  vec3 color3 = palette(iTime*0.5+20.);
  vec3 color4 = palette(iTime*0.5+20.);
  vec3 cc1 = getCC(st, top, color1);
  vec3 cc2 = getCC(st, bottom, color2);
  vec3 cc3 = getCC(st, left, color3);
  vec3 cc4 = getCC(st, right, color4);
  fin_c += cc1 + cc2 + cc3 + cc4;

  gl_FragColor = vec4(fin_c, 1.0);
}