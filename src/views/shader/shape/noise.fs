// 使用sdf来绘制图形，使用一维noise来定义他的旋转平移缩放，颜色
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

float ndot(vec2 a, vec2 b ) { return a.x*b.x - a.y*b.y; }
float sdRhombus( in vec2 p, in vec2 b ) 
{
    p = abs(p);
    float h = clamp( ndot(b-2.0*p,b)/dot(b,b), -1.0, 1.0 );
    float d = length( p-0.5*b*vec2(1.0-h,1.0+h) );
    return d * sign( p.x*b.y + p.y*b.x - b.x*b.y );
}


float sdStar5(in vec2 p, in float r, in float rf)
{
    const vec2 k1 = vec2(0.809016994375, -0.587785252292);
    const vec2 k2 = vec2(-k1.x,k1.y);
    p.x = abs(p.x);
    p -= 2.0*max(dot(k1,p),0.0)*k1;
    p -= 2.0*max(dot(k2,p),0.0)*k2;
    p.x = abs(p.x);
    p.y -= r;
    vec2 ba = rf*vec2(-k1.y,k1.x) - vec2(0,1);
    float h = clamp( dot(p,ba)/dot(ba,ba), 0.0, r );
    return length(p-ba*h) * sign(p.y*ba.x-p.x*ba.y);
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
  float i = (noise(iTime+seed))*10.;

  float theta = sin(noise(iTime+i)) * PI*4.;
  st = rotate(st, theta);
  float v1 = abs(sin(noise(iTime+i)));
  float v2 = abs(sin(noise(iTime+i+10.)));
  float v3 = abs(sin(noise(iTime+i+20.)));
  float xoff = (sin(noise(iTime+i)))/10.;
  float yoff = (sin(noise(iTime+i)))/10.;
  st += vec2(xoff,yoff);
  vec3 color = palette(noise(iTime*i));

  float box = sdStar5(st,v1,v2);
  float smoothStart = abs(sin(noise(iTime * i)))*10.*pix;
  float smoothEnd = abs(sin(noise(iTime * i)))*40.*pix;
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