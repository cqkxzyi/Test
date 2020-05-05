#安装webpack
    -yarn add webpack webpack-cli -D

#webpack配置

#打包命令
    npx webpack
#Code Runner模拟运行

#配置文件
    webpack.config.js，可以修改配置文件名称，编译时指定参数(npx webpack --config  webpack.config.js)

#配置package.json 
    运行脚本：npm run build 
#测试服务器配置
    yarn add webpack-dev-server -d
#css支持模块化
    loader(style-loader     css-loader    less-loader  等)
    mini-css-extract-plugin 单独生成css文件
    postcss-loader  autoprefixer    前缀抽离，postcss-loader需要引用autoprefixer
    optimize-css-assets-webpack-plugin      压缩css
    uglifyjs-webpack-plugin     压缩js
#js转换
    babel-loader    @babel/core (基础)    @babel/preset-env(es6转换成es5)    
    @babel/plugin-proposal-class-properties  类class关键字解析