uniform vec2 u_resolution;

uniform float u_time;

float line(vec2 st, float lineW){
  return step(st.x - 0.5, lineW);
}

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xy;

  vec3 color1 = vec3(0.0);

  vec3 color2 = vec3(0.79f, 0.76f, 0.1f);

  vec3 color = mix(color1, color2, vec3(line(st, 0.02)));

  gl_FragColor = vec4(color, 1.0);
}