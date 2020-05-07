//import $ from 'expose-loader?$!jquery'
//import $ from 'jquery'

console.info("a.js被引用了");
console.log($);
//测试expose-loader 暴露全局的loader
//console.log(window.$);
class B{

}

function * gen (params){
    yield 1;
}

console.info("gen函数：");
console.info(gen().next());

function TestJquery(){
    var arr = [4, "Pete", 8, "John"];
    //console.info("TestJquery结果："+ $.inArray(8, arr));
}



export {
    B,
    gen,
    TestJquery
};