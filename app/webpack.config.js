const path = require("path");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const { CleanWebpackPlugin } = require("clean-webpack-plugin");
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
var CopyPlugin = require('copy-webpack-plugin');


module.exports = {
    entry: {
        configFormBuilder: ['./src/formBuilder/index.ts'],
        fetchData: ['./src/fetchData/index.ts'],
    },
    devtool: 'inline-source-map',
    module: {
        rules: [
            {
                test: /\.tsx?$/,
                use: 'ts-loader',
                exclude: /node_modules/,
            },
            {
                test: /\.css$/,
                use: [MiniCssExtractPlugin.loader, "css-loader"],
            },
        ],
    },
    resolve: {
        extensions: ['.tsx', '.ts', '.js'],
    },
    output: {
        library: {
            name: 'OrderMaker',
            type: 'var'
        },
        filename: 'js/[name].js',
        path: __dirname + '/wwwroot/lib/mtd-ordermaker/dist',
        publicPath: "/",
    },
    plugins: [
        /* new CleanWebpackPlugin(),*/

        new MiniCssExtractPlugin({
            insert: "#some-element",
            filename: "css/[name].css",
        }),

        new CopyPlugin({
            patterns: [
                { from: "src/**/*.png", to: __dirname + "/wwwroot/lib/mtd-ordermaker/images/[name][ext]" },
            ],
            options: {
                concurrency: 100,
            },
        }),
    ],
    optimization: {
        minimizer: [
            new CssMinimizerPlugin(),
        ],
        minimize: true,
    }
};
