var webpack = require('webpack');
var path = require('path');
var HtmlWebpackPlugin = require('html-webpack-plugin');
var ExtractTextPlugin = require('extract-text-webpack-plugin');
const CopyWebpackPlugin = require('copy-webpack-plugin');

module.exports = {
    entry: {
        styles: './src/styles.scss',
        polyfills: './src/polyfills.ts',
        vendor: './src/vendor.ts',
        app: './src/main.ts'
    },
    output: {
        path: path.resolve(__dirname + '/dist'),
        publicPath: '/',
        filename: '[name].[hash].js',
        chunkFilename: '[id].[hash].chunk.js'
    },
    resolve: {
        extensions: ['.ts', '.js']
    },
    module: {
        rules: [{
                test: /\.ts$/,
                loaders: [{
                    loader: 'awesome-typescript-loader',
                    options: {
                        configFileName: path.resolve(__dirname + '/tsconfig.json')
                    },
                }, 'angular2-template-loader'],
            },
            {
                test: /\.html$/,
                loader: 'html-loader'

            },
            {
                test: /\.scss$/,
                exclude: [/node_modules/, /styles.scss$/],
                loaders: ['raw-loader', 'sass-loader'] // sass-loader not scss-loader
            },
            {
                test: /\.(png|jp(e*)g|svg)$/,
                use: [{
                    loader: 'url-loader',
                    options: {
                        limit: 8000, // Convert images < 8kb to base64 strings
                        name: 'assets/images/[hash]-[name].[ext]'
                    }
                }]
            },
            {
                test: /\.json$/,
                loader: 'json-loader'
            }
        ]
    },
    plugins: [
        // Workaround for angular/angular#11580
        new webpack.ContextReplacementPlugin(
            // The (\\|\/) piece accounts for path separators in *nix and Windows
            /angular(\\|\/)core(\\|\/)@angular/,
            '/src',
            {} // a map of your routes
        ),
        new HtmlWebpackPlugin({
            template: 'src/index.html'
        }),
        new webpack.optimize.CommonsChunkPlugin({
            name: ['app', 'vendor', 'polyfills']
        })
        ,
        new CopyWebpackPlugin([
            {from:'src/assets/images',to:'images'},
            {from:'src/assets/videos',to:'videos'},
            {from:'src/app/locale',to:'data/[name].js' }
        ])
    ]
}