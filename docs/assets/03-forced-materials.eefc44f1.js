import{d as p,B as E,f as w,G as L,c as H,g as W,h as B,S as R,P as k,W as A,A as I,b as V,O as D,V as T}from"./OrbitControls.9262050a.js";import{d as U,g as C,h as q,c as F,f as S,o as N}from"./index.bd6dc507.js";const $={class:"viewCon"},ne=U({__name:"03-forced-materials",setup(J){const{random:r,PI:O,floor:x,ceil:K,min:Q,max:X,sin:Y,cos:Z}=Math,f=C(),h=C();let c=window.innerWidth,d=window.innerHeight,m,u,t,l,a,i;q(()=>{G(),_(),j(),o.numberOfObjects=t.children.length,M(),b()});const o={rotationSpeed:.02,numberOfObjects:0,removeCube(){const e=t.children,n=e[e.length-1];n instanceof p&&(t.remove(n),this.numberOfObjects--)},addCube(){const e=r()*3,n=new E(e,e,e),z=new w({color:r()*16777215}),s=new p(n,z);s.castShadow=!0,s.name="cube-"+t.children.length;const{width:v,height:g}=i.geometry.parameters;s.position.x=-v/2+r()*v,s.position.y=x(r()*3)+3,s.position.z=-g/2+r()*g,t.add(s),this.numberOfObjects++},outputObjects(){console.log(t.children)}};function M(){var n;let e=new L({autoPlace:!1,width:300});e.domElement.style.cssText="position: fixed; top: 0; right: 0;",(n=h.value)==null||n.appendChild(e.domElement),e.add(o,"rotationSpeed",0,.5),e.add(o,"addCube"),e.add(o,"removeCube"),e.add(o,"outputObjects"),e.add(o,"numberOfObjects").listen()}function _(){const e=new H(60,20,1,1),n=new w({color:16777215});i=new p(e,n),i.receiveShadow=!0,i.rotation.x=-.5*O,i.position.set(0,0,0),t.add(i)}function j(){const e=new W(16777215);e.position.set(-20,20,0),e.castShadow=!0;const n=new B(e);t.add(n),t.add(e)}function y(){t.traverse(function(e){e instanceof p&&e!==i&&(e.rotation.x+=o.rotationSpeed,e.rotation.y+=o.rotationSpeed,e.rotation.z+=o.rotationSpeed)})}function b(){requestAnimationFrame(b),m.update(),y(),a.render(t,l)}function G(){t=new R,t.overrideMaterial=new w({color:16777215}),l=new k(45,c/d,1,1e3),a=new A({canvas:f.value,antialias:!0}),a.setClearColor(4473924,1),a.setSize(c,d),a.shadowMap.enabled=!0,a.setPixelRatio(window.devicePixelRatio),window.addEventListener("resize",P),t.add(new I(1e3)),m=V(),h.value.append(m.dom),u=new D(l,a.domElement),u.target=new T(0,0,0),u.object.position.set(-50,20,20),u.update()}function P(){c=window.innerWidth,d=window.innerHeight,l.aspect=c/d,l.updateProjectionMatrix(),a.setSize(c,d)}return(e,n)=>(N(),F("div",$,[S("div",{class:"canvasCon",ref_key:"canvasCon",ref:h},[S("canvas",{class:"canvas",ref_key:"canvasDom",ref:f},null,512)],512)]))}});export{ne as default};
