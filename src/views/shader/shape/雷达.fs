// 雷达
precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float pix = 1.0;

float SMOOTH(float D, float d) {
  return smoothstep(D-pix,D+pix,d);
}

/**
  空心圆，带边宽度
  uv坐标，中心坐标，半径，borderWidth
*/
float circle(vec2 uv, vec2 c, float r, float bw) {
  float dist = distance(uv, c);
  float c1 = SMOOTH(dist, r);
  float c2 = SMOOTH(dist, r-bw);
  return c1 - c2;
}

// c-center,corss的半径
float _cross(vec2 uv, vec2 c, float r) {
  vec2 d = uv - c;
  float len = length(d);
  if (len < r && (abs(d.x) - abs(d.y) == 0.0)) {
    return 1.0;
  }
  return 0.0;
}

void main() {
  vec2 center = iResolution.xy * 0.5;
  vec2 uv = gl_FragCoord.xy; // 直接使用uv坐标，不用归一化
  vec3 color = vec3(0.0);
  vec3 color1 =  vec3(0.17, 0.95, 0.91);
  vec3 color2 =  vec3(0.98);
  
  float c0 = circle(uv, center, 10.0, 1.0);
  color += c0 * color1;
  float c1 = circle(uv, center, 100.0, 2.0);
  color += c1 * color1;
  float c2 = circle(uv, center, 140.0, 2.0);
  color += c2 * color1;
  float c3 = circle(uv, center, 180.0, 2.0);
  color += c3 * color1;
  float c4 = circle(uv, center, 220.0, 4.0);
  color += c4 * color2;

  float cro = _cross(uv, center, 140.0);
  color += cro * color1;


  gl_FragColor = vec4(color, 1.0);
}
