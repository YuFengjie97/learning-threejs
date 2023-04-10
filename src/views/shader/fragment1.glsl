uniform vec2 u_resolution;
uniform float u_time;

void main() {
  float t = u_time;
  vec2 r = u_resolution.xy;

  vec3 c;
  float l, z = t;
  for(int i = 0; i < 3; i++) {
    vec2 uv, p = gl_FragCoord.xy / r;
    uv = p;
    p -= .5;
    p.x *= r.x / r.y;
    l = length(p);
    uv += p / l * (sin(z) + 1.) * abs(sin(l * 9. - z - z));
    c[i] = .01 / length(mod(uv, 1.0) - 0.5);
  }
  gl_FragColor = vec4(c / l, t);
  // gl_FragColor = vec4(c.r, c.g, c.b, t);
}