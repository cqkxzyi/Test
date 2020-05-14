//express

var express=require("express");

let app=express();

app.get("/app/user",(req,res)=>{
    res.json({name:"我是zhangyi"});
});

app.listen(3000);
