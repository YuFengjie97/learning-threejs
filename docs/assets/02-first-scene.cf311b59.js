import{G as P,c as S,M as l,d as w,B as k,e as W,S as z,C as A,P as H,W as I,A as L,a as R,b as V,O as j,V as D}from"./OrbitControls.c43f435c.js";import{d as O,g as v,h as T,c as U,f as M,o as q}from"./index.0d0188e0.js";const F={class:"viewCon"},te=O({__name:"02-first-scene",setup(N){const{random:$,PI:_,floor:J,ceil:K,min:Q,max:X,sin:Y,cos:Z}=Math,m=v(),c=v();let n=window.innerWidth,t=window.innerHeight,d,r,e,a,s;T(()=>{C(),G(),b(),p()});function C(){var i;let o=new P({autoPlace:!1,width:300});o.domElement.style.cssText="position: fixed; top: 0; right: 0;",(i=c.value)==null||i.appendChild(o.domElement)}function b(){const o=new S(60,20),i=new l({color:13421772}),u=new w(o,i);u.rotation.x=-.5*_,e.add(u);const x=new k(4,4,4),E=new l({color:16711680,wireframe:!0}),h=new w(x,E);h.position.set(-14,2,0),e.add(h);const y=new W(4),B=new l({color:7829503,wireframe:!0}),f=new w(y,B);f.position.set(14,4,0),e.add(f)}function p(){requestAnimationFrame(p),d.update(),s.render(e,a)}function G(){e=new z,e.background=new A(4473924),a=new H(75,n/t,1,1e3),s=new I({canvas:m.value,antialias:!0}),s.setSize(n,t),window.addEventListener("resize",g),e.add(new L(1e3)),e.add(new R(4210752)),d=V(),c.value.append(d.dom),r=new j(a,s.domElement),r.target=new D(0,0,0),r.object.position.set(0,20,20),r.update()}function g(){n=window.innerWidth,t=window.innerHeight,a.aspect=n/t,a.updateProjectionMatrix(),s.setSize(n,t)}return(o,i)=>(q(),U("div",F,[M("div",{class:"canvasCon",ref_key:"canvasCon",ref:c},[M("canvas",{class:"canvas",ref_key:"canvasDom",ref:m},null,512)],512)]))}});export{te as default};
