// 一个圆用noise来根据角度去顶距离，比较柔和的变化，然后，多组在4个方向上重叠在一起

precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float PI = 3.1415926;
float pix;

float rand(float x) {
  return fract(sin(x));
}

float noise(float x) {
  float i = floor(x);
  float f = fract(x);
  return mix(rand(i), rand(i + 1.), smoothstep(0., 1., f));
}

vec3 palette(float t) {
  vec3 a = vec3(1.000,0.500,0.500);
  vec3 b = vec3(0.500,0.500,0.500);
  vec3 c = vec3(0.750,1.000,0.667);
  vec3 d = vec3(0.800,1.000,0.333);
  return a + b * cos(6.28318 * (c * t + d));
}

vec3 danceCircle(vec2 st, float thetaSeed, float timeSeed, float circleSeed) {
  float r = length(st);
  float baseSize = 0.2;
  float changeSize = 0.6;

  float i = baseSize + changeSize * noise(sin(thetaSeed) + timeSeed);
  float v = 1. - smoothstep(0., 20.*pix, abs(r - i));

  return palette(circleSeed) * v;
}

void main() {
  pix = 1./iResolution.y;
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy) / min(iResolution.x, iResolution.y);
  vec3 fin_c = vec3(0.);

  float theta = mod(atan(st.y, st.x), 2. * PI);

  float total = 20.;
  for(float i=0.;i<total;i++) {
    float timeOff = (sin(iTime + i*0.1)*0.5 + 0.25);
    float thetaOff = PI*0.5 * mod(i,4.);
    vec3 circle = danceCircle(st, theta+thetaOff,iTime+timeOff, i*0.1);
    fin_c += circle;
  }

  gl_FragColor = vec4(fin_c, 1.0);
}