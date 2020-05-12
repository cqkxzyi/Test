
console.info("加载index.js");
//let a_model=require("./a.js");
import * as a_model from  './a.js';


require("./css/index.css");
require('./css/index.less');

function getList(){
    alert("getList");
}

let func1=()=>{
    alert("func1");
}

class A{
    a=300;
}
let a=new A(10);
console.log(a.a);

a_model.gen();
a_model.TestJquery();
console.info("测试require完成");

//图片加载
import img2020 from './images/2020.jpg'//把图片引入，返回的是新图片地址
let image=new Image();
image.src=img2020;
document.body.appendChild(image);

//require('html-loader!./html/file.html')
