
let Webpack=require("webpack");//引入webpack
const merge = require("webpack-merge");
let base=require("./webpack.base.js");

let devConfig ={
    mode: "development",
    devtool:'source-map',
    plugins:[
        new Webpack.DefinePlugin({//环境变量配置
            DEV:JSON.stringify("dev"),
            FLAG:JSON.stringify("bbbbb")
        })
    ]
};

module.exports=merge(devConfig,base);