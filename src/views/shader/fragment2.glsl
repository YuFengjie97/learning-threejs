uniform vec2 u_resolution;
uniform float u_time;

void main() {
  vec2 st = gl_FragCoord.xy / u_resolution.xx;
  float pix = 5.0 / u_resolution.x;

  float l = smoothstep(0.0, pix, st.x - st.y);

  gl_FragColor = vec4(l);
}