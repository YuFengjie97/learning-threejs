#ifdef GL_ES
precision mediump float;
#endif

#define TWO_PI 6.28318530718

uniform vec2 iResolution;
uniform float iTime;

float pix;

float SMOOTH(float D, float d) {
  return smoothstep(D - pix, D + pix, d);
}

float circle(vec2 st, vec2 center,float r) {
  float dist = distance(st, center);
  return 1.0 - SMOOTH(r, dist);
}

// https://thebookofshaders.com/edit.php#06/easing.frag
float bounceOut(float t) {
  const float a = 4.0 / 11.0;
  const float b = 8.0 / 11.0;
  const float c = 9.0 / 10.0;

  const float ca = 4356.0 / 361.0;
  const float cb = 35442.0 / 1805.0;
  const float cc = 16061.0 / 1805.0;

  float t2 = t * t;

  return t < a
    ? 7.5625 * t2
    : t < b
      ? 9.075 * t2 - 9.9 * t + 3.4
      : t < c
        ? ca * t2 - cb * t + cc
        : 10.8 * t * t - 20.52 * t + 10.72;
}

//  Function from Iñigo Quiles
//  https://www.shadertoy.com/view/MsS3Wc
vec3 hsb2rgb(in vec3 c) {
  vec3 rgb = clamp(abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
  rgb = rgb * rgb * (3.0 - 2.0 * rgb);
  return c.z * mix(vec3(1.0), rgb, c.y);
}

void main() {

  pix = 1.0 / iResolution.x;
  vec3 color = vec3(0.0);


/**
  如果分辨率y小于x，就是以y为基准进行坐标归一化，中心位置：z/0.5=ux/uy，z就是要找的以y为基准归一化后，x方向的归一化坐标
  如果x<y，0.5/z=ux/uy
  比例式相等关键：归一化后的坐标比例与之前未归一化相等
*/
  float ux = iResolution.x;
  float uy = iResolution.y;
  vec2 st = ux < uy ? gl_FragCoord.xy / ux : gl_FragCoord.xy / uy;
  vec2 center = ux < uy ? vec2(0.5, uy/ux*0.5) : vec2(ux/uy*0.5, 0.5);

  vec2 toCenter = center - st;
  float angle = atan(toCenter.y, toCenter.x) + bounceOut( sin(iTime));
  float radius = length(toCenter) * 2.0;
  vec3 c = hsb2rgb(vec3((angle / TWO_PI) + 0.5, radius, 1.0));

  float pct = circle(st, center,0.4) - circle(st, center,0.3);

  color = pct * c;

  gl_FragColor = vec4(color, 1.0);
}