//webpack 是node写出来的
let path = require('path');
let Webpack=require("webpack");//引入webpack
const { CleanWebpackPlugin }  = require("clean-webpack-plugin");//清理文件
let HtmlWebpackPlugin = require("html-webpack-plugin");//文件输出
let CopyWebpackPlugin = require('copy-webpack-plugin');
const HtmlLoader= require("html-loader");
let MiniCssExtractPlugin=require("mini-css-extract-plugin");//css生成单独文件

module.exports = {
    mode: "development",//production
    devtool:'source-map',//源码文件
    entry: {    //入口
        index:"./src/index.js"
        ,other:"./src/other.js"
    }
    // ,watch:true//实时监控代码、打包
    // ,watchOptions:{
    //     poll:1000,
    //     aggregateTimeout:2000,//防抖
    //     ignored:/node_modules/
    // }
    ,output: {
        filename: "[name][hash:6].js",//输出文件名 [hash:5]
        path: path.resolve(__dirname, "build")//必须是绝对路径
        //,publicPath: 'http://www.image.com' 资源前缀
    },
    devServer: { //开发服务器配置
        port: 3001,
        progress: true//进度展示
        ,contentBase: "./build" //path.join(__dirname, "dist")
    },
    plugins: [ //插件配置区域
        //new Webpack.ProgressPlugin(),//可有可无
        // HtmlLoader,
        new CleanWebpackPlugin(//文件清理
            { 
            dry: false
            ,verbose:false
            ,cleanOnceBeforeBuildPatterns:[
                path.join(process.cwd(), 'build/**/*')
                ,'！static-files * ']
            ,exclude:['assets']
          })
          ,new CopyWebpackPlugin([//文件copy
            {
                from: "./src/doc"
                ,to:"./doc"
                ,ignore: ['*.txt']
            }
        ])
        ,new HtmlWebpackPlugin({  //html文件输出配置
            template: "./src/html/index.html",
            filename: "index.html",
            chunks:["index","other"] //选择引用哪些js
            // ,minify: {
            //     removeAttributeQuotes:true
            // }//压缩
            //, hash: true//生成随机hash
        })
        ,new MiniCssExtractPlugin({//css单独生成文件
            filename:"css/main.css"
        })
        ,new Webpack.ProvidePlugin({//在每个模块中注入jquery
            $: 'jquery',
            jQuery: 'jquery'
        })
        ,new Webpack.BannerPlugin("zhangyi 2020-05-13")
    ]
    // ,externals:{//忽略模块配置，不打包编译模块   需要页面自行引用js
    //     jquery: 'jquery',
    //     $:'jquery'
    //   }
    ,module: {//模块配置
        //规则 css-loader 解析@import语法
        //style-loader 作用是：把css插入到head标签
        //loader特点： 功能单一、默认从右向左、从下至上执行
        rules: [
            // {
            //     test: /\.html/,//解析html中的图片资源(不能同时兼顾所有情况，暂时改用html-loader处理)
            //     use: "html-withimg-loader"
            // }
            {
                test: /\.(html)$/i,
                use: {
                  loader: 'html-loader'
                }
            } 
            ,{
                test: /\.(png|gif)$/,//使用base64进行转换
                use: {
                    loader: "url-loader",
                    options: {
                        limit: 1 * 1024
                        ,outputPath:"images/"
                        //,publicPath: 'http://www.image.com' 资源前缀
                    }
                }
            }
            ,{
                test:/\.(jpg|jpeg)$/,//图片处理（方式1）
                use: [
                    {
                        loader: 'file-loader',
                        options: {
                            esModule: false
                            ,outputPath:"images/"
                            //,publicPath: 'http://www.image.com' 资源前缀
                        },
                    },
                ]
            }
            ,{
                test: /\.css$/,
                use: [//可以是字符串、对象。区别是：对象可以设置更多配置
                    // {
                    //     loader: 'style-loader'
                    //     ,options: { insert: 'body' }
                    // }
                    MiniCssExtractPlugin.loader
                    , 'css-loader'
                    , 'postcss-loader'
                ]
            }
            , {
                test: /\.less$/,
                use: [
                    'style-loader'
                    , 'css-loader'
                    , 'postcss-loader'
                    , 'less-loader']
            }
            ,{
                test: /\.js$/
                ,include:path.resolve(__dirname, "src") //js处理范围
                ,exclude: /node_modules/  //js排除范围
                ,use:{
                    loader: "babel-loader"
                    ,options:{
                        presets:[
                            '@babel/preset-env'
                        ],
                        plugins:[
                            '@babel/plugin-proposal-class-properties' //类class关键字解析
                            ,'@babel/plugin-transform-runtime' //高级语法解析
                        ]
                    }
              }
            }
            //   ,{
            //       test:/\.js$/ //效验js
            //       ,exclude: /node_modules/  //js排除范围
            //       ,use:{
            //         loader:"eslint-loader" 
            //         ,options:{
            //             enforce:'pre' //强制之前执行previous   之后是post
            //         }
            //       }
            //   }
        ]
    }

};