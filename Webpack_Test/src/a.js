module.expores="zy";
console.info("a.js被引用了");

class B{

}

function * gen (params){
    yield 1;
}

console.info(gen().next());