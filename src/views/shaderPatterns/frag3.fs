precision mediump float;

#define PI 3.14159265358979323846

uniform vec2 u_resolution;
uniform float u_time;

const float w = 0.8; // 爻的宽度
const float h = 0.1; // 爻的高度
const float yinGap = 0.2; // 阴爻中间的间距
const float gap = 0.1; // 爻的间距

float random(float seed) {
  return fract(sin(seed) * u_time);
}

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

float SMOOTH(float D, float d) {
  return smoothstep(D - 0.01, D + 0.01, d);
}

float rect(vec2 st, float l, float r, float t, float b) {
  float v = SMOOTH(b, st.y) - SMOOTH(t, st.y);
  float h = SMOOTH(l, st.x) - SMOOTH(r, st.x);
  return v * h;
}

float geneYao(vec2 st, bool yang) {
  if(yang) {
    return rect(st, 0.0, w, h, 0.0);
  } else {
    float l = rect(st, 0.0, w * 0.5 - yinGap * 0.5, h, 0.0);
    float r = rect(st, w * 0.5 + yinGap * 0.5, w, h, 0.0);
    return l + r;
  }
}

float geneYi(vec2 st, bool y1, bool y2, bool y3) {
  float W = w;
  float H = h * 3.0 + gap * 2.0;
  vec2 t = vec2(W * 0.5, H * 0.5);
  st -= 0.5;
  st += t;

  float yao1 = geneYao(st, y1);
  st.y -= gap + h;
  float yao2 = geneYao(st, y2);
  st.y -= gap + h;
  float yao3 = geneYao(st, y3);
  st.y -= gap + h;
  return yao1 + yao2 + yao3;
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  vec3 color = vec3(0.0);

  float col = 4.0;

  st *= col;

  vec2 ipos = floor(st);
  st = fract(st);
  float b = genBound(st);

  bool y1 = random(ipos.x + ipos.y + 1.0) > 0.5;
  bool y2 = random(ipos.x + ipos.y + 2.0) > 0.5;
  bool y3 = random(ipos.x + ipos.y + 3.0) > 0.5;

  float yi = geneYi(st, y1, y2, y3);

  color += yi;
  color += b;

  gl_FragColor = vec4(color, 1.0);
}
