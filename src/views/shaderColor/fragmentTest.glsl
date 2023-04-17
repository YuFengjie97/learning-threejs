uniform vec2 u_resolution;

void main() {
  vec3 color1 = vec3(1.0f, 0.0f, 0.0f);
  vec3 color2 = vec3(0.0, 1.0, 0.0);

  vec2 st = gl_FragCoord.xy / u_resolution.xy;

  gl_FragColor = vec4(mix(color1, color2, smoothstep(0.0, 1.0, st.x)), 1.0);
}