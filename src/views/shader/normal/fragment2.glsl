uniform vec2 u_resolution;
uniform float u_time;

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;
  // float pix = 5.0 / u_resolution.x;

  vec2 pos = vec2(0.5, 0.5) - st;
  float r = length(pos) * 3.0;
  float a = atan(pos.y, pos.x);

  float f = abs(cos(a * 20.0) * sin(a * abs(sin(u_time) * 10.0))) * .8 + .1;

  vec3 color = vec3(1.0-smoothstep(f, f + 0.02, r), 0.4, 0.4);

  gl_FragColor = vec4(color, 1.0);
}