var se=Object.defineProperty;var oe=(o,e,n)=>e in o?se(o,e,{enumerable:!0,configurable:!0,writable:!0,value:n}):o[e]=n;var W=(o,e,n)=>(oe(o,typeof e!="symbol"?e+"":e,n),n);import{d as _,M as ae,V as U,Q as Y,s as $,X as O,C as I,Y as q,Z as re,_ as le,k as ce,$ as ue,a0 as Q,a1 as pe,G as de,T as ve,R as Z,c as fe,f as N,B as he,e as me,g as we,h as ge,S as xe,F as ye,P as be,W as Me,A as Se,b as Pe,O as Le}from"./OrbitControls.863a14c5.js";import{t as Ve}from"./grasslight-big.a424f278.js";import{d as Ce,g as J,h as De,c as _e,f as K,o as Be}from"./index.6b0d4b2d.js";class A extends _{constructor(){super(A.Geometry,new ae({opacity:0,transparent:!0})),this.isLensflare=!0,this.type="Lensflare",this.frustumCulled=!1,this.renderOrder=1/0;const e=new U,n=new U,M=new Y(16,16,$),S=new Y(16,16,$),P=A.Geometry,B=new O({uniforms:{scale:{value:null},screenPosition:{value:null}},vertexShader:`

				precision highp float;

				uniform vec3 screenPosition;
				uniform vec2 scale;

				attribute vec3 position;

				void main() {

					gl_Position = vec4( position.xy * scale + screenPosition.xy, screenPosition.z, 1.0 );

				}`,fragmentShader:`

				precision highp float;

				void main() {

					gl_FragColor = vec4( 1.0, 0.0, 1.0, 1.0 );

				}`,depthTest:!0,depthWrite:!1,transparent:!1}),F=new O({uniforms:{map:{value:M},scale:{value:null},screenPosition:{value:null}},vertexShader:`

				precision highp float;

				uniform vec3 screenPosition;
				uniform vec2 scale;

				attribute vec3 position;
				attribute vec2 uv;

				varying vec2 vUV;

				void main() {

					vUV = uv;

					gl_Position = vec4( position.xy * scale + screenPosition.xy, screenPosition.z, 1.0 );

				}`,fragmentShader:`

				precision highp float;

				uniform sampler2D map;

				varying vec2 vUV;

				void main() {

					gl_FragColor = texture2D( map, vUV );

				}`,depthTest:!1,depthWrite:!1,transparent:!1}),H=new _(P,B),L=[],T=b.Shader,v=new O({uniforms:{map:{value:null},occlusionMap:{value:S},color:{value:new I(16777215)},scale:{value:new q},screenPosition:{value:new U}},vertexShader:T.vertexShader,fragmentShader:T.fragmentShader,blending:re,transparent:!0,depthWrite:!1}),G=new _(P,v);this.addElement=function(i){L.push(i)};const p=new q,a=new q,m=new le,t=new pe;this.onBeforeRender=function(i,f,s){i.getCurrentViewport(t);const V=t.w/t.z,c=t.z/2,d=t.w/2;let r=16/t.w;if(p.set(r*V,r),m.min.set(t.x,t.y),m.max.set(t.x+(t.z-16),t.y+(t.w-16)),n.setFromMatrixPosition(this.matrixWorld),n.applyMatrix4(s.matrixWorldInverse),!(n.z>0)&&(e.copy(n).applyMatrix4(s.projectionMatrix),a.x=t.x+e.x*c+c-8,a.y=t.y+e.y*d+d-8,m.containsPoint(a))){i.copyFramebufferToTexture(a,M);let u=B.uniforms;u.scale.value=p,u.screenPosition.value=e,i.renderBufferDirect(s,null,P,B,H,null),i.copyFramebufferToTexture(a,S),u=F.uniforms,u.scale.value=p,u.screenPosition.value=e,i.renderBufferDirect(s,null,P,F,H,null);const C=-e.x*2,w=-e.y*2;for(let z=0,R=L.length;z<R;z++){const g=L[z],x=v.uniforms;x.color.value.copy(g.color),x.map.value=g.texture,x.screenPosition.value.x=e.x+C*g.distance,x.screenPosition.value.y=e.y+w*g.distance,r=g.size/t.w;const E=t.w/t.z;x.scale.value.set(r*E,r),v.uniformsNeedUpdate=!0,i.renderBufferDirect(s,null,P,v,G,null)}}},this.dispose=function(){B.dispose(),F.dispose(),v.dispose(),M.dispose(),S.dispose();for(let i=0,f=L.length;i<f;i++)L[i].texture.dispose()}}}class b{constructor(e,n=1,M=0,S=new I(16777215)){this.texture=e,this.size=n,this.distance=M,this.color=S}}b.Shader={uniforms:{map:{value:null},occlusionMap:{value:null},color:{value:null},scale:{value:null},screenPosition:{value:null}},vertexShader:`

		precision highp float;

		uniform vec3 screenPosition;
		uniform vec2 scale;

		uniform sampler2D occlusionMap;

		attribute vec3 position;
		attribute vec2 uv;

		varying vec2 vUV;
		varying float vVisibility;

		void main() {

			vUV = uv;

			vec2 pos = position.xy;

			vec4 visibility = texture2D( occlusionMap, vec2( 0.1, 0.1 ) );
			visibility += texture2D( occlusionMap, vec2( 0.5, 0.1 ) );
			visibility += texture2D( occlusionMap, vec2( 0.9, 0.1 ) );
			visibility += texture2D( occlusionMap, vec2( 0.9, 0.5 ) );
			visibility += texture2D( occlusionMap, vec2( 0.9, 0.9 ) );
			visibility += texture2D( occlusionMap, vec2( 0.5, 0.9 ) );
			visibility += texture2D( occlusionMap, vec2( 0.1, 0.9 ) );
			visibility += texture2D( occlusionMap, vec2( 0.1, 0.5 ) );
			visibility += texture2D( occlusionMap, vec2( 0.5, 0.5 ) );

			vVisibility =        visibility.r / 9.0;
			vVisibility *= 1.0 - visibility.g / 9.0;
			vVisibility *=       visibility.b / 9.0;

			gl_Position = vec4( ( pos * scale + screenPosition.xy ).xy, screenPosition.z, 1.0 );

		}`,fragmentShader:`

		precision highp float;

		uniform sampler2D map;
		uniform vec3 color;

		varying vec2 vUV;
		varying float vVisibility;

		void main() {

			vec4 texture = texture2D( map, vUV );
			texture.a *= vVisibility;
			gl_FragColor = texture;
			gl_FragColor.rgb *= color;

		}`};A.Geometry=function(){const o=new ce,e=new Float32Array([-1,-1,0,0,0,1,-1,0,1,0,1,1,0,1,1,-1,1,0,0,1]),n=new ue(e,5);return o.setIndex([0,1,2,0,2,3]),o.setAttribute("position",new Q(n,3,0,!1)),o.setAttribute("uv",new Q(n,2,3,!1)),o}();const Fe="/learning-threejs/assets/lensflare0.cc6aae5d.png",Ge="/learning-threejs/assets/lensflare3.b777765d.png",ze={class:"viewCon"},We=Ce({__name:"07-lensflares",setup(o){const{random:e,PI:n,floor:M,ceil:S,min:P,max:B,sin:F,cos:H,sqrt:L,abs:T}=Math,v=J(),G=J();let p=window.innerWidth,a=window.innerHeight,m,t,i,f,s,V,c,d,r,u;De(()=>{z(),te(),R(),g(),X()});const C={rotationSpeed:.02,bouncingSpeed:.03},w={color:16777215,intensity:1};function z(){var y;let l=new de({autoPlace:!1,width:300});l.domElement.style.cssText="position: fixed; top: 0; right: 0;",(y=G.value)==null||y.appendChild(l.domElement),l.addColor(w,"color").onChange(h=>{r.spotLight.color.setHex(h),r.spotLightHelper.update(),r.lensflare.material.color.setHex(h)}),l.add(w,"intensity",0,2*w.intensity,.1).onChange(h=>{r.spotLight.intensity=h})}function R(){u=new ve;const l=u.load(Ve);l.wrapS=Z,l.wrapT=Z,l.repeat.set(4,4);const y=new fe(1e3,200,20,20),h=new N({map:l});V=new _(y,h),i.add(V),V.receiveShadow=!0,V.rotation.x=-.5*n;const j=new he(4,4,4),k=new N({color:14037041});c=new _(j,k),c.castShadow=!0,c.position.set(-10,4,0),i.add(c);const D=new me(4),ne=new N({color:623843});d=new _(D,ne),d.castShadow=!0,d.position.y=2,i.add(d)}function g(){r=new x(-30,30,-80)}class x{constructor(y,h,j){W(this,"spotLight");W(this,"spotLightHelper");W(this,"lensflare");this.spotLight=new we(w.color,w.intensity),this.spotLight.castShadow=!0,this.spotLight.position.set(y,h,j),this.spotLightHelper=new ge(this.spotLight);const k=u.load(Fe),D=u.load(Ge);this.lensflare=new A,this.lensflare.addElement(new b(k,700,0,new I().setHex(16777215))),this.lensflare.addElement(new b(D,60,.6)),this.lensflare.addElement(new b(D,70,.7)),this.lensflare.addElement(new b(D,120,.9)),this.lensflare.addElement(new b(D,70,1)),this.spotLight.add(this.lensflare),i.add(this.spotLight),i.add(this.spotLightHelper)}}let E=0;function ee(){c.rotation.x+=C.rotationSpeed,c.rotation.y+=C.rotationSpeed,c.rotation.z+=C.rotationSpeed,E+=C.bouncingSpeed,d.position.x=20+10*H(E),d.position.y=2+10*T(F(E))}function X(){requestAnimationFrame(X),m.update(),ee(),s.render(i,f)}function te(){i=new xe,i.fog=new ye(11184810,.01,200),f=new be(75,p/a,1,1e3),s=new Me({canvas:v.value,antialias:!0}),s.setClearColor(new I(11184895)),s.setSize(p,a),s.shadowMap.enabled=!0,window.addEventListener("resize",ie),i.add(new Se(1e3)),m=Pe(),G.value.append(m.dom),t=new Le(f,s.domElement),t.target=new U(10,0,0),t.object.position.set(-20,15,45),t.update()}function ie(){p=window.innerWidth,a=window.innerHeight,f.aspect=p/a,f.updateProjectionMatrix(),s.setSize(p,a)}return(l,y)=>(Be(),_e("div",ze,[K("div",{class:"canvasCon",ref_key:"canvasCon",ref:G},[K("canvas",{class:"canvas",ref_key:"canvasDom",ref:v},null,512)],512)]))}});export{We as default};
