precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float random(vec2 uv) {
  // return sin(dot(uv, vec2(t)) * t);
  return fract(sin(dot(uv.xy, vec2(12.9898, 78.233))) * 43758.5453123);
}

float noise(vec2 uv) {
  vec2 i = floor(uv);
  vec2 f = fract(uv);

  float a = random(i);
  float b = random(i + vec2(1.0, 0));
  float c = random(i + vec2(0., 1.));
  float d = random(i + vec2(1.));

  vec2 u = f * f * (3. - 2. * f);

  float v1 = mix(a, b, u.x);
  float v2 = mix(c, d, u.x);
  return mix(v1, v2, u.y);
}

void main() {
  vec2 uv = gl_FragCoord.xy / iResolution.xy;
  vec3 color_fin = vec3(0.);
  vec3 color_red = vec3(1.0, 0., 0.);

  vec2 uv2 = uv * 50.;
  float v = noise(uv2);
  color_fin += color_red * v;

  gl_FragColor = vec4(color_fin, 1.);
}