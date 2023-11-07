precision mediump float;

uniform vec2 iResolution;
uniform float iTime;
uniform vec2 iMouse;

float pix;
float PI = 3.141592653589793;

float SMOOTH(float D, float d, float pixNum) {
  return smoothstep(D - pixNum * pix, D + pixNum * pix, d);
}


void main() {
  pix = 1.0 / iResolution.y;
  // vec2 st = gl_FragCoord.xy / iResolution;
  vec2 st = (gl_FragCoord.xy * 2.0 - iResolution.xy) / min(iResolution.x, iResolution.y);
  vec3 c_fin = vec3(0.0);
  vec3 c_white = vec3(1.0);

  st *= 5.;
  c_fin += vec3(abs(st), .0);

  float r = length(st);
  float theta = atan(st.y,st.x);

  float l = 1.0 - SMOOTH(0., r- abs(sin(4.*theta)),10.);
  c_fin += l * c_white;


  gl_FragColor = vec4(c_fin, 1.0);
}