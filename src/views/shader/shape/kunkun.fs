precision mediump float;

#define PI 3.1415926
#define S(a, b, t) smoothstep(a,b,t)
#define B(a, b, blur, t) S(a-blur, a+blur, t)*S(b+blur, b-blur, t)
#define sat(t) clamp(t, 0., 1.)

uniform vec2 iResolution;

// dot2 and is from https://iquilezles.org/articles/distfunctions2d/
float dot2(in vec2 v) {
  return dot(v, v);
}
float sdBezier(in vec2 pos, in vec2 A, in vec2 B, in vec2 C) {
  vec2 a = B - A;
  vec2 b = A - 2.0 * B + C;
  vec2 c = a * 2.0;
  vec2 d = A - pos;
  float kk = 1.0 / dot(b, b);
  float kx = kk * dot(a, b);
  float ky = kk * (2.0 * dot(a, a) + dot(d, b)) / 3.0;
  float kz = kk * dot(d, a);
  float res = 0.0;
  float p = ky - kx * kx;
  float p3 = p * p * p;
  float q = kx * (2.0 * kx * kx - 3.0 * ky) + kz;
  float h = q * q + 4.0 * p3;
  if(h >= 0.0) {
    h = sqrt(h);
    vec2 x = (vec2(h, -h) - q) / 2.0;
    vec2 uv = sign(x) * pow(abs(x), vec2(1.0 / 3.0));
    float t = clamp(uv.x + uv.y - kx, 0.0, 1.0);
    res = dot2(d + (c + b * t) * t);
  } else {
    float z = sqrt(-p);
    float v = acos(q / (p * z * 2.0)) / 3.0;
    float m = cos(v);
    float n = sin(v) * 1.732050808;
    vec3 t = clamp(vec3(m + m, -n - m, n - m) * z - kx, 0.0, 1.0);
    res = min(dot2(d + (c + b * t.x) * t.x), dot2(d + (c + b * t.y) * t.y));
        // the third root cannot be the closest
        // res = min(res,dot2(d+(c+b*t.z)*t.z));
  }
  return sqrt(res);
}

mat2 rotate(float a) {
  float theta = PI / 180. * a;
  float s = sin(theta);
  float c = cos(theta);
  return mat2(c, -s, s, c);
}

vec4 Eye(vec2 uv) {
  vec4 eye = vec4(1.);
  eye.a = 1. - S(0., 0.01, distance(uv, vec2(-0.28, 0.3)) - 0.18);
  eye.a += 1. - S(0., 0.01, distance(uv, vec2(0.12, 0.3)) - 0.18);

  vec4 border = vec4(0.);
  border.a = B(0., 0.03, 0.003, distance(uv, vec2(-0.28, 0.3)) - 0.18);
  border.a += B(0., 0.03, 0.003, distance(uv, vec2(0.12, 0.3)) - 0.18);
  eye = mix(eye, border, border.a);

  vec4 eyeBall = vec4(0.);
  eyeBall.a = 1. - S(0., 0.007, distance(uv, vec2(-0.34, 0.36)) - 0.035);
  eyeBall.a += 1. - S(0., 0.007, distance(uv, vec2(0.03, 0.36)) - 0.035);
  eye = mix(eye, eyeBall, eyeBall.a);

  return eye;
}

vec4 Cheek(vec2 uv) {
  vec4 cheek = vec4(0.76, 0.26, 0.13, 1.);

  vec2 uv2 = uv * rotate(-30.);
  uv2 = uv2 * vec2(1., 1. / 1.2);
  cheek.a = 1. - S(0., 0.01, distance(uv2, vec2(-0.4, 0.2)) - 0.12);
  cheek.a += 1. - S(0., 0.01, distance(uv, vec2(0.38, 0.03)) - 0.15);

  return cheek;
}

vec4 Mouth(vec2 uv) {
  vec2 uv2 = uv * vec2(0.65, 1.);
  vec4 mouth = vec4(0.85, 0.46, 0.29, 1.);
  mouth.a = 1. - S(0., 0.01, distance(uv2, vec2(-0.05, -0.03)) - 0.15);

  vec4 border = vec4(0.);
  border.a = B(0., 0.02, 0.003, distance(uv2, vec2(-0.05, -0.03)) - 0.15);

  mouth = mix(mouth, border, border.a);

  float lip = B(0., 0.03, 0.003, distance(uv2, vec2(-0.05, 0.13)) - 0.18);
  mouth.rgb = mix(mouth.rgb, vec3(0.), lip);

  return mouth;
}

vec4 Head(vec2 uv) {
  vec4 head = vec4(0.9, 0.8, 0.3, 1.);

  vec2 uv2 = uv * vec2(0.8, .85);
  head.a = 1. - S(0.26, 0.28, distance(uv2, vec2(0., 0.1)) - 0.2);

  vec4 border = vec4(0.);
  border.a = B(0.26, 0.29, 0.003, (distance(uv2, vec2(0., 0.1)) - 0.2));
  head = mix(head, border, border.a);

  return head;
}

vec4 Hair(vec2 uv) {
  vec4 hair = vec4(0.42, 0.39, 0.43, 1.);
  float h1 = sdBezier(uv, vec2(-0.0, 0.7), vec2(0.5, 0.85), vec2(0.7, 0.2));
  float h2 = sdBezier(uv, vec2(-0.2, 0.7), vec2(-0.5, 0.85), vec2(-0.7, 0.2));
  float h = B(-1., 0.1, 0.01, h1) + B(-1., 0.1, 0.01, h2);
  hair.a = h;
  return hair;
}

vec4 Basketball(vec2 uv) {
  vec4 basketball = vec4(0.96, 0.51, 0.14, 1.);
  basketball.a = 1. - S(0., 0.01, distance(uv, vec2(0.8, -0.5)) - 0.3);

  vec4 basketballBorder = vec4(0.,0.,0.,B(0., 0.03, 0.005,distance(uv, vec2(0.8, -0.5)) - 0.3));
  basketball = mix(basketball, basketballBorder, basketballBorder.a);

  // vec4 c = B(0.26, 0.29, 0.005, distance(uv*rotate(-26.)*vec2(1.,1.8),vec2(-.0,-1.06)) - 0.2) * vec4(0.,0.,0.,1.);
  // basketball = mix(basketball, c, c.a);

  vec3 color_b = vec3(0.,0.,0.);
  float b1 = B(0.26, 0.29, 0.005, distance(uv*vec2(1.1,0.8), vec2(1.2, -0.5)) - 0.1);
  float b2 = B(0.26, 0.29, 0.005, distance(uv*vec2(1.,1.), vec2(0.7, -0.85)) - 0.2);
  float b3 =  B(0.26, 0.29, 0.005, distance(uv*rotate(26.)*vec2(1.,1.8),vec2(1.47, -0.06)) - 0.2);
  float b4 =  B(0.26, 0.29, 0.005, distance(uv*rotate(-26.)*vec2(1.,1.8),vec2(-.0,-1.06)) - 0.2);
  basketball.rgb = mix(basketball.rgb, color_b, b1+b2+b3+b4);

  return basketball;
}

vec4 KunKun(vec2 uv) {
  vec4 kk = vec4(0.47, 0.7, 0.93, 1.);

  vec4 head = Head(uv);
  kk = mix(kk, head, head.a);

  vec4 eye = Eye(uv);
  kk = mix(kk, eye, eye.a);

  vec4 cheek = Cheek(uv);
  kk = mix(kk, cheek, cheek.a);

  vec4 mouth = Mouth(uv);
  kk = mix(kk, mouth, mouth.a);

  vec4 hair = Hair(uv);
  kk = mix(kk, hair, hair.a);

  vec4 basketball = Basketball(uv);
  kk = mix(kk, basketball, basketball.a);

  return kk;
}

void main() {
  vec2 uv = (gl_FragCoord.xy * 2. - iResolution.xy) / iResolution.y;
  vec4 kk = KunKun(uv);

  gl_FragColor = mix(vec4(0.), kk, kk.a);
}