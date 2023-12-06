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
  return mix(rand(i), rand(i+1.), smoothstep(0.,1.,f));
}

float sdBox(in vec2 p, in vec2 b) {
  vec2 d = abs(p) - b;
  return length(max(d, 0.0)) + min(max(d.x, d.y), 0.0);
}

vec3 palette(float t) {
  vec3 a = vec3(1.000,0.500,0.500);
  vec3 b = vec3(0.500,0.500,0.500);
  vec3 c = vec3(0.750,1.000,0.667);
  vec3 d = vec3(0.800,1.000,0.333);
  return a + b * cos(6.28318 * (c * t + d));
}


vec2 rotate(vec2 p, float a) {
  float s = sin(a);
  float c = cos(a);
  mat2 matrix = mat2(c,-s,s,c);
  return p * matrix;
}

vec3 createBox(vec2 st, float seed) {
  float i = (noise(sin(iTime+seed)));

  float theta = sin(noise(iTime*i)) * PI*4.;
  st = rotate(st, theta);
  float w = abs(sin(noise(iTime*i)));
  float h = abs(sin(noise(iTime*i+1.)));
  float xoff = (sin(noise(iTime*i)))/10.;
  float yoff = (sin(noise(iTime*i+1.)))/10.;
  st += vec2(xoff,yoff);
  vec3 color = palette(noise(iTime*i));

  float box = sdBox(st,vec2(w,h));
  float smoothStart = abs(sin(noise(iTime * i)))*10.*pix;
  float smoothEnd = abs(sin(noise(iTime * i)))*30.*pix;
  float b = 1. - smoothstep(smoothStart,smoothEnd,abs(box-0.1));

  return b*color;
}

void main() {
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy) / min(iResolution.x, iResolution.y);
  pix = 1./iResolution.y;
  vec3 fin_c = vec3(0., 0., 0.);

  for(float i=0.;i<10.;i++) {
    vec3 box = createBox(st, i*0.01);
    fin_c += box;
  }

  gl_FragColor = vec4(fin_c, 1.0);
}