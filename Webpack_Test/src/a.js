module.expores="zy";
require("@babel/polyfill")

console.info("a.js被引用了");

class B{

}

function * gen (params){
    yield 1;
}

console.info(gen().next());


var result='abcdefg'.includes('a');
console.info(result);