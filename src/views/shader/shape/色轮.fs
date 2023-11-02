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

float circle(vec2 st, float r) {
  float dist = length(st);
  return 1.0 - SMOOTH(r, dist);
}

float ring(vec2 st, float r, float bw) {
float c1 = circle(st, r + bw);
float c2 = circle(st, r);
return c1 - c2;
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

return t < a ? 7.5625 * t2 : t < b ? 9.075 * t2 - 9.9 * t + 3.4 : t < c ? ca * t2 - cb * t + cc : 10.8 * t * t - 20.52 * t + 10.72;
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

vec2 st = gl_FragCoord.xy / iResolution.xy - 0.5;
st.x *= iResolution.x / iResolution.y;
// vec2 st = (gl_FragCoord.xy / iResolution.xy - 0.5) / iResolution.y;

vec3 color = vec3(0.0);

float angle = atan(st.y, st.x) + bounceOut(sin(iTime));
float radius = length(st) * 2.0;

float ringPct = ring(st, 0.25,0.1);

vec3 c = hsb2rgb(vec3((angle / TWO_PI) + 0.5, radius, 1.0));
color += ringPct * c;

gl_FragColor = vec4(color, 1.0);
}