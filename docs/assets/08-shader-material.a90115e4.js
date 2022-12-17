import{G as S,B as T,d as W,Y as E,a7 as L,g as H,h as k,S as B,C as V,P as q,W as O,A as U,a as j,b as A,O as N,V as R}from"./OrbitControls.d879c3bb.js";import{d as D,g as h,h as Y,c as $,f as y,o as J}from"./index.3e71378a.js";const i=`
    uniform float time;
    varying vec2 vUv;


    void main()
    {
    vec3 posChanged = position;
    posChanged.x = posChanged.x*(abs(sin(time*1.0)));
    posChanged.y = posChanged.y*(abs(cos(time*1.0)));
    posChanged.z = posChanged.z*(abs(sin(time*1.0)));
    //gl_Position = projectionMatrix * modelViewMatrix * vec4(position*(abs(sin(time)/2.0)+0.5),1.0);
    gl_Position = projectionMatrix * modelViewMatrix * vec4(posChanged,1.0);
    }

`,K=`
    precision highp float;
    uniform float time;
    uniform float alpha;
    uniform vec2 resolution;
    varying vec2 vUv;

    void main2(void)
    {
    vec2 position = vUv;
    float red = 1.0;
    float green = 0.25 + sin(time) * 0.25;
    float blue = 0.0;
    vec3 rgb = vec3(red, green, blue);
    vec4 color = vec4(rgb, alpha);
    gl_FragColor = color;
    }

    #define PI 3.14159
    #define TWO_PI (PI*2.0)
    #define N 68.5

    void main(void)
    {
    vec2 center = (gl_FragCoord.xy);
    center.x=-10.12*sin(time/200.0);
    center.y=-10.12*cos(time/200.0);

    vec2 v = (gl_FragCoord.xy - resolution/20.0) / min(resolution.y,resolution.x) * 15.0;
    v.x=v.x-10.0;
    v.y=v.y-200.0;
    float col = 0.0;

    for(float i = 0.0; i < N; i++)
    {
    float a = i * (TWO_PI/N) * 61.95;
    col += cos(TWO_PI*(v.y * cos(a) + v.x * sin(a) + sin(time*0.004)*100.0 ));
    }

    col /= 5.0;

    gl_FragColor = vec4(col*1.0, -col*1.0,-col*4.0, 1.0);
    }


`,Q=`
    // from http://glsl.heroku.com/e#7906.0


    uniform float time;
    uniform vec2 resolution;

    // 2013-03-30 by @hintz

    #define CGFloat float
    #define M_PI 3.14159265359

    vec3 hsvtorgb(float h, float s, float v)
    {
    float c = v * s;
    h = mod((h * 6.0), 6.0);
    float x = c * (1.0 - abs(mod(h, 2.0) - 1.0));
    vec3 color;

    if (0.0 <= h && h < 1.0)
    {
    color = vec3(c, x, 0.0);
    }
    else if (1.0 <= h && h < 2.0)
    {
    color = vec3(x, c, 0.0);
    }
    else if (2.0 <= h && h < 3.0)
    {
    color = vec3(0.0, c, x);
    }
    else if (3.0 <= h && h < 4.0)
    {
    color = vec3(0.0, x, c);
    }
    else if (4.0 <= h && h < 5.0)
    {
    color = vec3(x, 0.0, c);
    }
    else if (5.0 <= h && h < 6.0)
    {
    color = vec3(c, 0.0, x);
    }
    else
    {
    color = vec3(0.0);
    }

    color += v - c;

    return color;
    }

    void main(void)
    {

    vec2 position = (gl_FragCoord.xy - 0.5 * resolution) / resolution.y;
    float x = position.x;
    float y = position.y;

    CGFloat a = atan(x, y);

    CGFloat d = sqrt(x*x+y*y);
    CGFloat d0 = 0.5*(sin(d-time)+1.5)*d;
    CGFloat d1 = 5.0;

    CGFloat u = mod(a*d1+sin(d*10.0+time), M_PI*2.0)/M_PI*0.5 - 0.5;
    CGFloat v = mod(pow(d0*4.0, 0.75),1.0) - 0.5;

    CGFloat dd = sqrt(u*u+v*v);

    CGFloat aa = atan(u, v);

    CGFloat uu = mod(aa*3.0+3.0*cos(dd*30.0-time), M_PI*2.0)/M_PI*0.5 - 0.5;
    // CGFloat vv = mod(dd*4.0,1.0) - 0.5;

    CGFloat d2 = sqrt(uu*uu+v*v)*1.5;

    gl_FragColor = vec4( hsvtorgb(dd+time*0.5/d1, sin(dd*time), d2), 1.0 );
    }

`,X=`
    uniform vec2 resolution;
    uniform float time;

    vec2 rand(vec2 pos)
    {
    return fract( 0.00005 * (pow(pos+2.0, pos.yx + 1.0) * 22222.0));
    }
    vec2 rand2(vec2 pos)
    {
    return rand(rand(pos));
    }

    float softnoise(vec2 pos, float scale)
    {
    vec2 smplpos = pos * scale;
    float c0 = rand2((floor(smplpos) + vec2(0.0, 0.0)) / scale).x;
    float c1 = rand2((floor(smplpos) + vec2(1.0, 0.0)) / scale).x;
    float c2 = rand2((floor(smplpos) + vec2(0.0, 1.0)) / scale).x;
    float c3 = rand2((floor(smplpos) + vec2(1.0, 1.0)) / scale).x;

    vec2 a = fract(smplpos);
    return mix(
    mix(c0, c1, smoothstep(0.0, 1.0, a.x)),
    mix(c2, c3, smoothstep(0.0, 1.0, a.x)),
    smoothstep(0.0, 1.0, a.y));
    }

    void main(void)
    {
    vec2 pos = gl_FragCoord.xy / resolution.y;
    pos.x += time * 0.1;
    float color = 0.0;
    float s = 1.0;
    for(int i = 0; i < 8; i++)
    {
    color += softnoise(pos+vec2(i)*0.02, s * 4.0) / s / 2.0;
    s *= 2.0;
    }
    gl_FragColor = vec4(color);
    }

`,Z=`


    uniform float time;
    uniform vec2 resolution;

    vec2 rand(vec2 pos)
    {
    return
    fract(
    (
    pow(
    pos+2.0,
    pos.yx+2.0
    )*555555.0
    )
    );
    }

    vec2 rand2(vec2 pos)
    {
    return rand(rand(pos));
    }

    float softnoise(vec2 pos, float scale) {
    vec2 smplpos = pos * scale;
    float c0 = rand2((floor(smplpos) + vec2(0.0, 0.0)) / scale).x;
    float c1 = rand2((floor(smplpos) + vec2(1.0, 0.0)) / scale).x;
    float c2 = rand2((floor(smplpos) + vec2(0.0, 1.0)) / scale).x;
    float c3 = rand2((floor(smplpos) + vec2(1.0, 1.0)) / scale).x;

    vec2 a = fract(smplpos);
    return mix(mix(c0, c1, smoothstep(0.0, 1.0, a.x)),
    mix(c2, c3, smoothstep(0.0, 1.0, a.x)),
    smoothstep(0.0, 1.0, a.x));
    }

    void main( void ) {
    vec2 pos = gl_FragCoord.xy / resolution.y - time * 0.4;

    float color = 0.0;
    float s = 1.0;
    for (int i = 0; i < 6; ++i) {
    color += softnoise(pos + vec2(0.01 * float(i)), s * 4.0) / s / 2.0;
    s *= 2.0;
    }
    gl_FragColor = vec4(color,mix(color,cos(color),sin(color)),color,1);
    }

`,oo=`

    uniform float time;
    uniform vec2 resolution;

    // tie nd die by Snoep Games.

    void main( void ) {

    vec3 color = vec3(1.0, 0., 0.);
    vec2 pos = (( 1.4 * gl_FragCoord.xy - resolution.xy) / resolution.xx)*1.5;
    float r=sqrt(pos.x*pos.x+pos.y*pos.y)/15.0;
    float size1=2.0*cos(time/60.0);
    float size2=2.5*sin(time/12.1);

    float rot1=13.00; //82.0+16.0*sin(time/4.0);
    float rot2=-50.00; //82.0+16.0*sin(time/8.0);
    float t=sin(time);
    float a = (60.0)*sin(rot1*atan(pos.x-size1*pos.y/r,pos.y+size1*pos.x/r)+time);
    a += 200.0*acos(pos.x*2.0+cos(time/2.0))+asin(pos.y*5.0+sin(time/2.0));
    a=a*(r/50.0);
    a=200.0*sin(a*5.0)*(r/30.0);
    if(a>5.0) a=a/200.0;
    if(a<0.5) a=a*22.5;
    gl_FragColor = vec4( cos(a/20.0),a*cos(a/200.0),sin(a/8.0), 1.0 );
    }


`,eo=`


    uniform float time;
    uniform vec2 resolution;


    void main( void )
    {

    vec2 uPos = ( gl_FragCoord.xy / resolution.xy );//normalize wrt y axis
    //suPos -= vec2((resolution.x/resolution.y)/2.0, 0.0);//shift origin to center

    uPos.x -= 1.0;
    uPos.y -= 0.5;

    vec3 color = vec3(0.0);
    float vertColor = 2.0;
    for( float i = 0.0; i < 15.0; ++i )
    {
    float t = time * (0.9);

    uPos.y += sin( uPos.x*i + t+i/2.0 ) * 0.1;
    float fTemp = abs(1.0 / uPos.y / 100.0);
    vertColor += fTemp;
    color += vec3( fTemp*(10.0-i)/10.0, fTemp*i/10.0, pow(fTemp,1.5)*1.5 );
    }

    vec4 color_final = vec4(color, 1.0);
    gl_FragColor = color_final;
    }

`,to={class:"viewCon"},no=D({__name:"08-shader-material",setup(ao){const u=h(),v=h();let n=window.innerWidth,r=window.innerHeight,p,m,e,l,c,a;Y(()=>{C(),b(),_(),F(),x()});const d={step:.02};function C(){var t;let o=new S({autoPlace:!1,width:300});o.domElement.style.cssText="position: fixed; top: 0; right: 0;",(t=v.value)==null||t.appendChild(o.domElement),o.add(d,"step",0,1,.01)}function w(){a.rotation.x+=d.step,a.rotation.y+=d.step,a.rotation.z+=d.step,a.material.forEach(function(o){o.uniforms.time.value+=.01})}function _(){const o=new T(40,40,40),t=s(i,K),f=s(i,Q),g=s(i,X),G=s(i,Z),M=s(i,oo),z=s(i,eo),I=[t,f,g,G,M,z];return a=new W(o,I),e.add(a),a}function s(o,t){const f={time:{type:"f",value:.2},scale:{type:"f",value:.2},alpha:{type:"f",value:.6},resolution:{type:"v2",value:new E}};return f.resolution.value.x=window.innerWidth,f.resolution.value.y=window.innerHeight,new L({uniforms:f,vertexShader:o,fragmentShader:t,transparent:!0})}function F(){const o=new H(16777215,1);o.target=e,o.position.set(-50,50,50);const t=new k(o);e.add(o),e.add(t)}function b(){e=new B,e.background=new V(4473924),l=new q(75,n/r,1,1e3),c=new O({canvas:u.value,antialias:!0}),c.setSize(n,r),window.addEventListener("resize",P),e.add(new U(1e3)),e.add(new j(4210752)),p=A(),v.value.append(p.dom),m=new N(l,c.domElement),m.target=new R(0,0,0),m.object.position.set(40,40,40),m.update()}function x(){requestAnimationFrame(x),p.update(),w(),c.render(e,l)}function P(){n=window.innerWidth,r=window.innerHeight,l.aspect=n/r,l.updateProjectionMatrix(),c.setSize(n,r)}return(o,t)=>(J(),$("div",to,[y("div",{class:"canvasCon",ref_key:"canvasCon",ref:v},[y("canvas",{class:"canvas",ref_key:"canvasDom",ref:u},null,512)],512)]))}});export{no as default};
