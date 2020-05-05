
console.info("你好啊，1");
let str=require("./a.js");
console.info("你好啊，2");

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
