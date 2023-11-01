// 造型函数

#ifdef GL_ES
precision mediump float;
#endif

uniform float iTime;
uniform vec2 iResolution;

// 造型函数
float gain(float x, float k) {
  float a = 0.5 * pow(2.0 * ((x < 0.5) ? x : 1.0 - x), k);
  return (x < 0.5) ? a : 1.0 - a;
}

// smooth过渡
float plot(vec2 st, float pct) {
  return smoothstep(pct + 0.02, pct, st.y) - smoothstep(pct, pct - 0.02, st.y);
}

// 随机函数
float random(float x) {
  return fract(sin(x));
}

vec3 rgb2hsb(in vec3 c) {
  vec4 K = vec4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
  vec4 p = mix(vec4(c.bg, K.wz), vec4(c.gb, K.xy), step(c.b, c.g));
  vec4 q = mix(vec4(p.xyw, c.r), vec4(c.r, p.yzx), step(p.x, c.r));
  float d = q.x - min(q.w, q.y);
  float e = 1.0e-10;
  return vec3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

vec3 hsb2rgb(in vec3 c) {
  vec3 rgb = clamp(abs(mod(c.x * 6.0 + vec3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
  rgb = rgb * rgb * (3.0 - 2.0 * rgb);
  return c.z * mix(vec3(1.0), rgb, c.y);
}

void main() {
  vec2 st = gl_FragCoord.xy / iResolution;
  vec3 color = vec3(0.0);

  for(float k = -4.0; k <= 4.0; k += 0.1) {
    float pct = gain(st.x, k);
    float cpct = plot(st, pct);
    vec3 hsl = vec3(sin(iTime + k),1.0,0.5);
    vec3 rgb = hsb2rgb(hsl);

    color += cpct * rgb;
  }

  gl_FragColor = vec4(color, 1.0);
}
