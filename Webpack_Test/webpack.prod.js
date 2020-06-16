
let Webpack=require("webpack");//引入webpack
const merge = require("webpack-merge");
let base=require("./webpack.base.js");

let devConfig={
    mode: "production",
    devtool:'cheap-module-eval-source-map',
    plugins:[
        new Webpack.DefinePlugin({//环境变量配置
            DEV:JSON.stringify("prod"),
            FLAG:JSON.stringify("cccccc")
            })
    ]
};

module.exports=merge(devConfig,base);