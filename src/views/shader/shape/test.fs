#ifdef GL_ES
precision mediump float;
#endif

uniform vec2 iResolution;
uniform float iTime;

void main(){
  vec2 coord = gl_FragCoord.xy / iResolution.xy;
	vec3 color = vec3(0.0);

	color += sin(coord.x * cos(iTime / 60.0) * 60.0) + sin(coord.y * cos(iTime / 60.0) * 10.0);
	color += cos(coord.y * sin(iTime / 30.0) * 10.0) + cos(coord.x * sin(iTime / 20.0) * 10.0);

	// color *= sin(iTime / 10.0) * 0.5;

	gl_FragColor = vec4(color, 1.0);
}