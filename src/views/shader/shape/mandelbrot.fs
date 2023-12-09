precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float PI = 3.1415926;
float pix;
float maxinterations = 100.;

vec3 palette(float t) {
  vec3 a = vec3(0.000, 0.500, 0.500);
  vec3 b = vec3(0.000, 0.500, 0.500);
  vec3 c = vec3(0.000, 0.500, 0.333);
  vec3 d = vec3(0.000, 0.500, 0.667);
  return a + b * cos(6.28318 * (c * t + d));
}


// int getMdb(vec2 uv) {
//   vec2 z = uv;
//   for(int i = 0; i < maxinterations; i++) {
//     if(length(z) > 3.)
//       return i;
//     z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + uv;
//   }
//   return 0;
// }


vec3 getMdb(vec2 uv) {
  vec2 z = uv;
  for(float i = 0.; i < maxinterations; i++) {
    if(length(z) > 3.) {
      // return i * palette(i*100./maxinterations+iTime * 0.1);
      return sin(vec3(i));
    }
    z = vec2(z.x * z.x - z.y * z.y, 2.0 * z.x * z.y) + uv;
  }
  return vec3(0.);
}

void main() {
  pix = 1. / iResolution.y;
  vec2 st = (gl_FragCoord.xy * 2. - iResolution.xy) / min(iResolution.x, iResolution.y);
  st *= 1.4;

  vec3 fin_c = vec3(0.);

  vec3 mdb = getMdb(st);

  fin_c += mdb;

  gl_FragColor = vec4(fin_c, 1.0);
}