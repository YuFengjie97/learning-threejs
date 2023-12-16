precision mediump float;

uniform vec2 iResolution;
uniform float iTime;

float PI = 3.1415926;

float size = 1.;

float random(vec2 uv) {
  // return fract(sin(dot(uv.xy, vec2(12.9898, 78.233))) * 43758.5453123);
  return fract((sin(dot(uv.xy, vec2(12.9898, 78.233)))) * 43758.5453123);
}

float noise(vec2 uv) {
  vec2 i = floor(uv);
  vec2 f = fract(uv);

  float a = random(i);
  float b = random(i + vec2(size, 0));
  float c = random(i + vec2(0., size));
  float d = random(i + vec2(size));

  vec2 u = f * f * (3. - 2. * f);
  // vec2 u = smoothstep(0., 1., f);

  float v1 = mix(a, b, u.x);
  float v2 = mix(c, d, u.x);
  return mix(v1, v2, u.y);
}

vec3 palette(float t) {
  vec3 a = vec3(1.000, 0.500, 0.500);
  vec3 b = vec3(0.500, 0.500, 0.500);
  vec3 c = vec3(0.750, 1.000, 0.667);
  vec3 d = vec3(0.800, 1.000, 0.333);
  return a + b * cos(6.28318 * (c * t + d));
}

#define octaves 6
float fbm(vec2 uv) {
  float lacunarity = 2.;
  float gain = 0.5;

  float amplitude = 0.5;
  float frequency = 1.;

  float result = 0.;

  for(int i = 0; i < octaves; i++) {
    result += amplitude * noise(frequency * uv);
    frequency *= lacunarity;
    amplitude *= gain;
  }
  return result;
}

void main() {
  vec2 uv = gl_FragCoord.xy / iResolution.y;
  vec3 color_fin = vec3(0.0);

  vec2 pos = uv * 10.;
  float a = noise(pos * vec2(cos(iTime * 0.1), sin(iTime * 0.1)) * 0.5) * PI * 2.;
  float col = noise(vec2(1.) * a);

  float c = fbm(pos);
  color_fin += c * palette(col);

  gl_FragColor = vec4(color_fin, 1.);
}