//import $ from 'expose-loader?$!jquery'
//import $ from 'jquery'

require("@babel/polyfill")

console.info("加载a.js");

console.log($);
//测试expose-loader 暴露全局的loader
console.log(window.$);


class B{

}

function * gen (params){
    yield 1;
}

console.info("gen函数：");
console.info(gen().next());

//语法解析测试
var result='abcdefg'.includes('a');
console.info(result);

function TestJquery(){
    var arr = [4, "Pete", 8, "John"];
    console.info("TestJquery结果："+ $.inArray(8, arr));
}



// //require/exports方式暴露
// var app = {
//     name: 'app',
//     version: '1.0.0',
//     sayName: function(name){
//         console.log(this.name);
//     }
// }
// module.exports = app;

//import/export方式暴露
export {
    B,
    gen,
    TestJquery
};
